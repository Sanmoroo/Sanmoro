//-----------------------------------------------------------------------
// Copyright 2021 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using System.Collections.Generic;
using Tobii.GameIntegration.Net;
using UnityEngine;
using Tobii.G2OM;

namespace Tobii.Gaming.Internal
{
    internal class TobiiHost : MonoBehaviour, ITobiiHost
    {
        private static TobiiHost _instance;
        private static bool _isShuttingDown;

        private GameViewBoundsProvider _gameViewBoundsProvider;
        private GameViewInfo _gameViewInfo = GameViewInfo.DefaultGameViewInfo;

        private GazePointDataProvider _gazePointDataProvider;
        private HeadPoseDataProvider _headPoseDataProvider;
        private int _lastUpdatedFrame;
        
        private static bool _hasDisplayedEulaError;

        private G2OM.G2OM _g2om;
        private Camera _userViewPointCamera = null;
        private TobiiSettings _settings = new TobiiSettings();
        private ExtendedViewSimpleSettings _extendedViewSettings = new ExtendedViewSimpleSettings();


        //--------------------------------------------------------------------
        // Public Function and Properties
        //--------------------------------------------------------------------

        public static ITobiiHost GetInstance()
        {
            if (_isShuttingDown)
            {
                return new TobiiHostStub();
            }

#if UNITY_EDITOR
            if (!(UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.StandaloneWindows64
                || UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.StandaloneWindows))
            {
                return new TobiiHostStub();
            }
#endif

			if (!TobiiEula.IsEulaAccepted())
			{
				if (!_hasDisplayedEulaError)
				{
					Debug.LogWarning("You need to accept EULA to be able to use Tobii Unity SDK.");
					_hasDisplayedEulaError = true;
				}
				return new TobiiHostStub();
			}

			if (_instance != null) return _instance;

            var newGameObject = new GameObject("TobiiHost");
            DontDestroyOnLoad(newGameObject);
            _instance = newGameObject.AddComponent<TobiiHost>();
            return _instance;
        }


        public void Shutdown()
        {
            if (_g2om != null)
            {
                _g2om.Destroy();
                _g2om = null;
            }

            _isShuttingDown = true;

            if (IsInitialized)
            {
#if !UNITY_EDITOR
				TobiiGameIntegrationApi.Shutdown();
#endif
                IsInitialized = false;
            }
        }

        private Camera UserViewPointCamera
        {
            get
            {
                return _userViewPointCamera == null ? Camera.main : _userViewPointCamera;
            }
        }
        
        private static G2OM_DeviceData CreateG2OMData(Camera camera, GazePoint data)
        {
            if (camera == null)
            {
                return new G2OM_DeviceData();
            }

            var cameraTransform = camera.transform;
            var ray = data.IsValid ? camera.ScreenPointToRay(data.Screen) : new Ray(cameraTransform.position, cameraTransform.forward);
            var up = cameraTransform.up;
            var right = cameraTransform.right;
            return new G2OM_DeviceData
            {
                timestamp = data.Timestamp,
                gaze_ray_world_space = new G2OM_GazeRay
                {
                    is_valid = data.IsValid.ToByte(),
                    ray = G2OM_UnityExtensionMethods.CreateRay(ray.origin, ray.direction),
                },
                camera_up_direction_world_space = up.AsG2OMVector3(),
                camera_right_direction_world_space = right.AsG2OMVector3()
            };
        }
        
        private void TrackWindow()
        {
            if (_gameViewBoundsProvider.Hwnd != IntPtr.Zero)
            {
                TobiiGameIntegrationApi.TrackWindow(_gameViewBoundsProvider.Hwnd);
                IsInitialized = true;
            }
        }
        
        private void Tick()
        {
            if (Time.frameCount == _lastUpdatedFrame) return;

            _lastUpdatedFrame = Time.frameCount;

            TrackWindow();

            var gameViewBounds = _gameViewBoundsProvider.GetGameViewClientAreaNormalizedBounds();
            _gameViewInfo = new GameViewInfo(gameViewBounds);

            TobiiGameIntegrationApi.UpdateExtendedViewSimpleSettings(_extendedViewSettings);
            TobiiGameIntegrationApi.Update();

            _gazePointDataProvider.Tick();
            _headPoseDataProvider.Tick();
            
            //G2OM
            if (_g2om != null)
            {
                if (UserViewPointCamera != null)
                {
                    var g2omData = CreateG2OMData(UserViewPointCamera, _gazePointDataProvider.Last);
                    _g2om.Tick(g2omData);
                }
            }
        }


        public void SetCurrentUserViewPointCamera(Camera camera)
        {
            _userViewPointCamera = camera;
        }

        public bool Initialize(TobiiSettings settings)
        {
            // Create default settings if none were provided
            if (settings == null)
                settings = new TobiiSettings();
            _settings = settings;

            // Use default extended view settings
            _extendedViewSettings = TobiiGameIntegrationApi.GetDefaultExtendedViewSimpleSettings();

            // Setup G2OM
            if (_g2om == null)
            {
                _g2om = Tobii.G2OM.G2OM.Create(new G2OM_Description
                {
                    LayerMask = _settings.LayerMask,
                    HowLongToKeepCandidatesInSeconds = _settings.HowLongToKeepCandidatesInSeconds
                });
            }

            return true;
        }

        public List<FocusedCandidate> GetFocusedObjects()
        {
            if (_g2om == null) 
                return new List<FocusedCandidate>();

            return _g2om.GazeFocusedObjects;
        }
        
        public DisplayInfo DisplayInfo
        {
            get
            {
                var trackerInfo = TobiiGameIntegrationApi.GetTrackerInfo();
                var displaySize = trackerInfo.DisplaySizeMm;
                return new DisplayInfo(displaySize.Width, displaySize.Height);
            }
        }

        public GameViewInfo GameViewInfo
        {
            get { return _gameViewInfo; }
        }

        public UserPresence UserPresence
        {
            get
            {
                return TobiiGameIntegrationApi.IsTrackerConnected()
                    ? (TobiiGameIntegrationApi.IsPresent() ? UserPresence.Present : UserPresence.NotPresent)
                    : UserPresence.Unknown;
            }
        }

        public TobiiExtendedViewAngles ExtendedViewAngles
        {
            get
            {
                if (TobiiGameIntegrationApi.IsTrackerConnected())
                {
                    const float rad2deg = 180f / (float)Math.PI;
                    var t = TobiiGameIntegrationApi.GetExtendedViewTransformation();
                    return new TobiiExtendedViewAngles()
                    {
                        Yaw = rad2deg * t.Rotation.Yaw,
                        Pitch = rad2deg * -t.Rotation.Pitch
                    };
                }
                return new TobiiExtendedViewAngles();
            }
        }

        public TobiiExtendedViewSettings ExtendedViewSettings
        {
            get 
            {
                return new TobiiExtendedViewSettings()
                {
                    EyeHeadTrackingRatio    = _extendedViewSettings.EyeHeadTrackingRatio,
                    CameraMaxAngleYaw       = _extendedViewSettings.CameraMaxAngleYaw,
                    CameraMaxAnglePitchUp   = _extendedViewSettings.CameraMaxAnglePitchUp,
                    CameraMaxAnglePitchDown = _extendedViewSettings.CameraMaxAnglePitchDown,
                    GazeResponsiveness      = _extendedViewSettings.GazeResponsiveness,
                    HeadSensitivityPitch    = _extendedViewSettings.HeadSensitivityPitch,
                    HeadSensitivityYaw      = _extendedViewSettings.HeadSensitivityYaw
                };
            }
            set
            {
                _extendedViewSettings.EyeHeadTrackingRatio      = value.EyeHeadTrackingRatio;
                _extendedViewSettings.CameraMaxAngleYaw         = value.CameraMaxAngleYaw;
                _extendedViewSettings.CameraMaxAnglePitchUp     = value.CameraMaxAnglePitchUp;
                _extendedViewSettings.CameraMaxAnglePitchDown   = value.CameraMaxAnglePitchDown;
                _extendedViewSettings.GazeResponsiveness        = value.GazeResponsiveness;
                _extendedViewSettings.HeadSensitivityPitch      = value.HeadSensitivityPitch;
                _extendedViewSettings.HeadSensitivityYaw        = value.HeadSensitivityYaw;
            }
        }

        public bool IsInitialized { get; private set; }

        public bool IsConnected { get { return TobiiGameIntegrationApi.IsTrackerConnected(); } }

        public IDataProvider<GazePoint> GetGazePointDataProvider()
        {
            Tick();
            return _gazePointDataProvider;
        }

        public IDataProvider<HeadPose> GetHeadPoseDataProvider()
        {
            Tick();
            return _headPoseDataProvider;
        }

        //--------------------------------------------------------------------
        // MonoBehaviour Messages
        //--------------------------------------------------------------------

        void Awake()
        {
#if UNITY_EDITOR
            _gameViewBoundsProvider = CreateEditorScreenHelper();
#else
			_gameViewBoundsProvider = new UnityPlayerGameViewBoundsProvider();
#endif

            _gazePointDataProvider = new GazePointDataProvider(this);
            _headPoseDataProvider = new HeadPoseDataProvider();

            TrackWindow();
        }
        
        void Update()
        {
            Tick();
        }

        void OnDestroy()
        {
            Shutdown();
        }

        void OnApplicationQuit()
        {
            Shutdown();
        }

#if UNITY_EDITOR
        private static GameViewBoundsProvider CreateEditorScreenHelper()
        {
#if UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1
			return new LegacyEditorGameViewBoundsProvider();
#else
            return new EditorGameViewBoundsProvider();
#endif
        }
#endif
    }
}

#else
using Tobii.Gaming.Stubs;

namespace Tobii.Gaming.Internal
{
    internal partial class TobiiHost : TobiiHostStub
    {
        // all implementation in the stub
    }
}
#endif
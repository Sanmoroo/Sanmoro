using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Tobii.GameIntegration.Net.Extensions;


namespace Tobii.GameIntegration.Net
{
	public class ExtendedViewSettings{};
	namespace Extensions
	{
		internal static class IntPtrExtensions
		{
			public static bool IsZero(this IntPtr @this)
			{
				return @this.Equals(IntPtr.Zero);
			}

			public static bool IsNotZero(this IntPtr @this)
			{
				return !@this.IsZero();
			}

			public static IntPtr Add(this IntPtr pointer, int offset)
			{
				var pointerInt = IntPtr.Size == 8 ? pointer.ToInt64() : pointer.ToInt32();
				return new IntPtr(pointerInt + offset);
			}

			public static List<T> ToList<T>(this IntPtr pointerToFirstItem, int numberOfItems)
			{
				if(pointerToFirstItem.IsNotZero())
				{
					var items = new List<T>(numberOfItems);
					var itemSize = Marshal.SizeOf(typeof(T));

					for(var i = 0; i < numberOfItems; i++)
					{
						var itemPointer = pointerToFirstItem.Add((i * itemSize));
						items.Add((T) Marshal.PtrToStructure(itemPointer, typeof(T)));
					}

					return items;
				}

				return default(List<T>);
			}

			public static string ToAnsiString(this IntPtr pointerToAnsiString)
			{
				return pointerToAnsiString.IsNotZero() ? Marshal.PtrToStringAnsi(pointerToAnsiString) : default(string);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct TobiiRectangle
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2d
	{
		public float X;
		public float Y;
	}

	public enum TrackerType
	{
		None = 0,
		PC,
		HeadMountedDisplay
	}

	public enum StreamType
	{
		Presence = 0,
		Head = 1,
		GazeOS = 2,
		Gaze = 3,
		Foveation = 4,
		EyeInfo = 5,
		HMD = 6,
		UnfilteredGaze = 7,
		Count = 8
	}

	[Flags]
	public enum CapabilityFlags
	{
		None = 0,
		Presence = 1 << 0,
		Head = 1 << 1,
		Gaze = 1 << 2,
		Foveation = 1 << 3,
		EyeInfo = 1 << 4,
		HMD = 1 << 5
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WidthHeight
	{
		public int Width;
		public int Height;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct TrackerInfoInterop
	{
		public TrackerType Type;
		public CapabilityFlags Capabilities;
		public TobiiRectangle DisplayRectInOSCoordinates;
		public WidthHeight DisplaySizeMm;
		public IntPtr Url;
		public IntPtr FriendlyName;
		public IntPtr MonitorNameInOS;
		public IntPtr ModelName;
		public IntPtr Generation;
		public IntPtr SerialNumber;
		public IntPtr FirmwareVersion;
		public bool IsAttached;
	}

	public class TrackerInfo
	{
		internal TrackerInfo(TrackerInfoInterop trackerInfoInterop)
		{
			Type = trackerInfoInterop.Type;
			Capabilities = trackerInfoInterop.Capabilities;
			DisplayRectInOSCoordinates = trackerInfoInterop.DisplayRectInOSCoordinates;
			DisplaySizeMm = trackerInfoInterop.DisplaySizeMm;
			Url = trackerInfoInterop.Url.ToAnsiString();
			FriendlyName = trackerInfoInterop.FriendlyName.ToAnsiString();
			MonitorNameInOS = trackerInfoInterop.MonitorNameInOS.ToAnsiString();
			ModelName = trackerInfoInterop.ModelName.ToAnsiString();
			Generation = trackerInfoInterop.Generation.ToAnsiString();
			SerialNumber = trackerInfoInterop.SerialNumber.ToAnsiString();
			FirmwareVersion = trackerInfoInterop.FirmwareVersion.ToAnsiString();
			IsAttached = trackerInfoInterop.IsAttached;
		}

		internal static TrackerInfo Create(TrackerInfoInterop trackerInfoInterop)
		{
			return new TrackerInfo(trackerInfoInterop);
		}

		public TrackerType Type { get;  }
		public CapabilityFlags Capabilities { get; }
		public TobiiRectangle DisplayRectInOSCoordinates { get; }
		public WidthHeight DisplaySizeMm { get; }
		public string Url { get; }
		public string FriendlyName { get; }
		public string MonitorNameInOS { get; }
		public string ModelName { get; }
		public string Generation { get; }
		public string SerialNumber { get; }
		public string FirmwareVersion { get; }
		public bool IsAttached { get; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Rotation
	{
		public float Yaw;
		public float Pitch;
		public float Roll;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Position
	{
		public float X;
		public float Y;
		public float Z;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct HeadPose
	{
		public Rotation Rotation;
		public Position Position;
		public long TimeStampMicroSeconds;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GazePoint
	{
		public long TimeStampMicroSeconds;
		public float X;
		public float Y;
	}

	public enum StatisticsLiteral
	{
		IsEnabled,
		IsDisabled,
		ChangedToEnabled,
		ChangedToDisabled,
		GameStarted,
		GameStopped,
		Separator
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Transformation
	{
		public Rotation Rotation;
		public Position Position;
	}

	public enum HeadViewType: int
	{
		None = 0,
		Direct = 1,
		Dynamic = 2
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SensitivityGradientSettings
	{
		public float Exponent;
		public float InflectionPoint;
		public float StartPoint;
		public float EndPoint;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct HeadViewAutoCenterSettings
	{
		[MarshalAs(UnmanagedType.I1)] public bool IsEnabled;
		public float NormalizeFasterGazeDeadZoneNormalized;
		public float ExtendedViewAngleFasterDeadZoneDegrees;
		public float MaxDistanceFromMasterCm;
		public float MaxAngularDistanceDegrees;
		public float FasterNormalizationFactor;
		public float PositionCompensationSpeed;
		public float RotationCompensationSpeed;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct ExtendedViewAdvancedSettings
	{
		public ExtendedViewAdvancedSettings(bool getCDefaults = true)
		{
			if (getCDefaults) { this = TobiiGameIntegrationApi.GetDefaultExtendedViewAdvancedSettings(); }
			else { this = default(ExtendedViewAdvancedSettings); }
		}

		public HeadViewType HeadViewType;
		public float AspectRatioCorrectionFactor;
		public float HeadViewPitchCorrectionFactor;
		public SensitivityGradientSettings GazeViewSensitivityGradientSettings;
		public float GazeViewResponsiveness;
		public float GazeViewMaxCameraAngleYaw;
		public float GazeViewMaxCameraAnglePitchUp;
		public float GazeViewMaxCameraAnglePitchDown;
		public SensitivityGradientSettings HeadViewSensitivityGradientSettings;
		public float HeadViewResponsiveness;
		public float HeadViewMaxCameraAngleYaw;
		public float HeadViewMaxCameraAnglePitchUp;
		public float HeadViewMaxCameraAnglePitchDown;
		public float HeadViewMaxCameraAngleRoll;
		public HeadViewAutoCenterSettings HeadViewAutoCenter;
		public float HeadSensitivityYaw;
		public float HeadSensitivityPitch;
		public float HeadSensitivityRoll;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ExtendedViewSimpleSettings
	{
		public ExtendedViewSimpleSettings(bool getCDefaults = true)
		{
			if (getCDefaults) { this = TobiiGameIntegrationApi.GetDefaultExtendedViewSimpleSettings(); }
			else { this = default(ExtendedViewSimpleSettings); }
		}

		public float EyeHeadTrackingRatio;
		public float CameraMaxAngleYaw;
		public float CameraMaxAnglePitchUp;
		public float CameraMaxAnglePitchDown;
		public float GazeResponsiveness;
		public float HeadSensitivityYaw;
		public float HeadSensitivityPitch;
	}

	public static class TobiiGameIntegrationApi
	{
		private static bool Is64BitProcess => IntPtr.Size == 8;

		public static string LoadedDll => Is64BitProcess ? x64.TobiiGameIntegrationDll : x86.TobiiGameIntegrationDll;

		public static void PrelinkAll(){
			if (Is64BitProcess)
			{
				Marshal.PrelinkAll(typeof(x64));
			} 
			else
			{
				Marshal.PrelinkAll(typeof(x86));
			}
		}

		public static void SetApplicationName(string fullApplicationName)
		{
			if (Is64BitProcess)
			{
				x64.SetApplicationName(fullApplicationName);
			}
			else
			{
				x86.SetApplicationName(fullApplicationName);
			}
		}

		#region ITobiiGameIntegrationApi

		public static bool IsApiInitialized()
		{
			if(Is64BitProcess)
			{
				return x64.IsApiInitialized();
			}
			else
			{
				return x86.IsApiInitialized();
			}
		}

		public static void Update()
		{
			if(Is64BitProcess)
			{
				x64.Update();
			}
			else
			{
				x86.Update();
			}
		}

		public static void Shutdown()
		{
			if(Is64BitProcess)
			{
				x64.Shutdown();
			}
			else
			{
				x86.Shutdown();
			}
		}

		#endregion


		#region ITrackerController

		public static TrackerInfo GetTrackerInfo()
		{
			TrackerInfoInterop trackerInfoInterop;
			bool isTrackerInfoAvialable;

			if (Is64BitProcess)
			{
				isTrackerInfoAvialable = x64.GetTrackerInfo(out trackerInfoInterop);
			}
			else
			{
				isTrackerInfoAvialable = x86.GetTrackerInfo(out trackerInfoInterop);
			}
			
			return isTrackerInfoAvialable ? new TrackerInfo(trackerInfoInterop) : default(TrackerInfo);
		}

		public static TrackerInfo GetTrackerInfo(string trackerUrl)
		{
			TrackerInfoInterop trackerInfoInterop;
			bool isTrackerInfoAvialable;

			if (Is64BitProcess)
			{
				isTrackerInfoAvialable = x64.GetTrackerInfoByUrl(trackerUrl, out trackerInfoInterop);
			}
			else
			{
				isTrackerInfoAvialable = x86.GetTrackerInfoByUrl(trackerUrl, out trackerInfoInterop);
			}

			return isTrackerInfoAvialable ? new TrackerInfo(trackerInfoInterop) : default(TrackerInfo);
		}

		public static void UpdateTrackerInfos()
		{
			if (Is64BitProcess)
			{
				x64.UpdateTrackerInfos();
			}
			else
			{
				x86.UpdateTrackerInfos();
			}
		}

		public static List<TrackerInfo> GetTrackerInfos()
		{
			IntPtr trackerInfoInteropsPointer;
			bool hasAsyncUpdateFinished;
			int numberOfTrackerInfos;

			if (Is64BitProcess)
			{
				hasAsyncUpdateFinished = x64.GetTrackerInfos(out trackerInfoInteropsPointer, out numberOfTrackerInfos);
			}
			else
			{
				hasAsyncUpdateFinished = x86.GetTrackerInfos(out trackerInfoInteropsPointer, out numberOfTrackerInfos);
			}

			return hasAsyncUpdateFinished ? trackerInfoInteropsPointer.ToList<TrackerInfoInterop>(numberOfTrackerInfos).Select(TrackerInfo.Create).ToList() : default(List<TrackerInfo>);
		}
		
		public static bool TrackHMD()
		{
			if(Is64BitProcess)
			{
				return x64.TrackHMD();
			}
			else
			{
				return x86.TrackHMD();
			}
		}

		public static bool TrackRectangle(TobiiRectangle rectangle)
		{
			if(Is64BitProcess)
			{
				return x64.TrackRectangle(rectangle);
			}
			else
			{
				return x86.TrackRectangle(rectangle);
			}
		}

		public static bool TrackWindow(IntPtr windowHandle)
		{
			if(Is64BitProcess)
			{
				return x64.TrackWindow(windowHandle);
			}
			else
			{
				return x86.TrackWindow(windowHandle);
			}
		}

		public static bool TrackTracker(string trackerUrl)
		{
			if (Is64BitProcess)
			{
				return x64.TrackTracker(trackerUrl);
			}
			else
			{
				return x86.TrackTracker(trackerUrl);
			}
		}


		public static bool IsTrackerConnected()
		{
			if(Is64BitProcess)
			{
				return x64.IsConnected();
			}
			else
			{
				return x86.IsConnected();
			}
		}

		public static bool IsTrackerEnabled()
		{
			if (Is64BitProcess)
			{
				return x64.IsDeviceEnabled();
			}
			else
			{
				return x86.IsDeviceEnabled();
			}
		}

		public static void StopTracking()
		{
			if(Is64BitProcess)
			{
				x64.StopTracking();
			}
			else
			{
				x86.StopTracking();
			}
		}

		#endregion


		#region IStreamsProvider

		public static bool TryGetLatestHeadPose(out HeadPose headPose)
		{
			if(Is64BitProcess)
			{
				return x64.GetLatestHeadPose(out headPose);
			}
			else
			{
				return x86.GetLatestHeadPose(out headPose);
			}
		}

		public static bool TryGetLatestGazePoint(out GazePoint gazePoint)
		{
			if(Is64BitProcess)
			{
				return x64.GetLatestGazePoint(out gazePoint);
			}
			else
			{
				return x86.GetLatestGazePoint(out gazePoint);
			}
		}

		public static List<GazePoint> GetGazePoints()
		{
			IntPtr gazePoints;
			int numberOfAvailableGazePoints;

			if(Is64BitProcess)
			{
				numberOfAvailableGazePoints = x64.GetGazePoints(out gazePoints);
			}
			else
			{
				numberOfAvailableGazePoints = x86.GetGazePoints(out gazePoints);
			}

			return gazePoints.ToList<GazePoint>(numberOfAvailableGazePoints);
		}

		public static List<HeadPose> GetHeadPoses()
		{
			IntPtr headPoses;
			int numberOfAvailableHeadPoses;

			if(Is64BitProcess)
			{
				numberOfAvailableHeadPoses = x64.GetHeadPoses(out headPoses);
			}
			else
			{
				numberOfAvailableHeadPoses = x86.GetHeadPoses(out headPoses);
			}

			return headPoses.ToList<HeadPose>(numberOfAvailableHeadPoses);
		}

		public static bool IsPresent()
		{
			if(Is64BitProcess)
			{
				return x64.IsPresent();
			}
			else
			{
				return x86.IsPresent();
			}
		}

		public static void SetAutoUnsubscribe(StreamType capability, float timeout)
		{
			if(Is64BitProcess)
			{
				x64.SetAutoUnsubscribe(capability, timeout);
			}
			else
			{
				x86.SetAutoUnsubscribe(capability, timeout);
			}
		}

		public static void UnsetAutoUnsubscribe(StreamType capability)
		{
			if(Is64BitProcess)
			{
				x64.UnsetAutoUnsubscribe(capability);
			}
			else
			{
				x86.UnsetAutoUnsubscribe(capability);
			}
		}

		#endregion


		#region IStatisticsRecorder

		public static string GetStatisticsLiteral(StatisticsLiteral statisticsLiteral)
		{
			IntPtr statisticsLiteralPtr;

			if(Is64BitProcess)
			{
				statisticsLiteralPtr = x64.GetStatisticsLiteral(statisticsLiteral);
			}
			else
			{
				statisticsLiteralPtr = x86.GetStatisticsLiteral(statisticsLiteral);
			}

			return Marshal.PtrToStringAnsi(statisticsLiteralPtr);
		}

		#endregion


		#region IExtendedView

		public static Transformation GetExtendedViewTransformation()
		{
			Transformation extendedViewTransformation;

			if(Is64BitProcess)
			{
				x64.GetExtendedViewTransformation(out extendedViewTransformation);
			}
			else
			{
				x86.GetExtendedViewTransformation(out extendedViewTransformation);
			}

			return extendedViewTransformation;
		}

		public static bool UpdateExtendedViewAdvancedSettings(ExtendedViewAdvancedSettings settings)
		{
			if(Is64BitProcess)
			{
				return x64.UpdateExtendedViewAdvancedSettings(ref settings);
			}
			else
			{
				return x86.UpdateExtendedViewAdvancedSettings(ref settings);
			}
		}

		public static void ResetExtendedViewDefaultHeadPose()
		{
			if(Is64BitProcess)
			{
				x64.ResetExtendedViewDefaultHeadPose();
			}
			else
			{
				x86.ResetExtendedViewDefaultHeadPose();
			}
		}

		public static void SetExtendedViewPaused(bool paused)
		{
			if(Is64BitProcess)
			{
				x64.SetExtendedViewPaused(paused);
			}
			else
			{
				x86.SetExtendedViewPaused(paused);
			}
		}

		public static void SetExtendedViewPausedAfterRecentering()
		{
			if(Is64BitProcess)
			{
				x64.SetExtendedViewPausedAfterRecentering();
			}
			else
			{
				x86.SetExtendedViewPausedAfterRecentering();
			}
		}

		public static bool GetExtendedViewPaused()
		{
			if(Is64BitProcess)
			{
				return x64.GetExtendedViewPaused();
			}
			else
			{
				return x86.GetExtendedViewPaused();
			}
		}

		public static bool UpdateExtendedViewSimpleSettings(ExtendedViewSimpleSettings settings)
		{
			if(Is64BitProcess)
			{
				return x64.UpdateExtendedViewSimpleSettings(ref settings);
			}
			else
			{
				return x86.UpdateExtendedViewSimpleSettings(ref settings);
			}
		}

		public static ExtendedViewAdvancedSettings GetDefaultExtendedViewAdvancedSettings()
		{
			ExtendedViewAdvancedSettings settings;
			if (Is64BitProcess)
			{
				x64.GetDefaultExtendedViewAdvancedSettings(out settings);
			}
			else
			{
				x86.GetDefaultExtendedViewAdvancedSettings(out settings);
			}
			return settings;
		}

		public static ExtendedViewAdvancedSettings GetExtendedViewAdvancedSettings()
		{
			ExtendedViewAdvancedSettings settings;
			if(Is64BitProcess)
			{
				x64.GetExtendedViewAdvancedSettings(out settings);
			}
			else
			{
				x86.GetExtendedViewAdvancedSettings(out settings);
			}
			return settings;
		}

		public static ExtendedViewSimpleSettings GetDefaultExtendedViewSimpleSettings()
		{
			ExtendedViewSimpleSettings settings;
			if (Is64BitProcess)
			{
				x64.GetDefaultExtendedViewSimpleSettings(out settings);
			}
			else
			{
				x86.GetDefaultExtendedViewSimpleSettings(out settings);
			}
			return settings;
		}

		public static ExtendedViewSimpleSettings GetExtendedViewSimpleSettings()
		{
			ExtendedViewSimpleSettings settings;
			if(Is64BitProcess)
			{
				x64.GetExtendedViewSimpleSettings(out settings);
			}
			else
			{
				x86.GetExtendedViewSimpleSettings(out settings);
			}
			return settings;
		}

		public static int GetExtendedViewSensitivityGradientAsPolyLine([In, Out] Vector2d[] allocatedPointsBuf, SensitivityGradientSettings sensitivitySettings)
		{
			int maxPoints = allocatedPointsBuf.Length;
			if(Is64BitProcess)
			{
				return x64.GetExtendedViewSensitivityGradientAsPolyLine(allocatedPointsBuf, maxPoints, ref sensitivitySettings);
			}
			else
			{
				return x86.GetExtendedViewSensitivityGradientAsPolyLine(allocatedPointsBuf, maxPoints, ref sensitivitySettings);
			}
		}

		#endregion



		private static class x86
		{
#if DEBUG
			internal const string TobiiGameIntegrationDll = "tobii_gameintegration_x86.dll";
#else
			internal const string TobiiGameIntegrationDll = "tobii_gameintegration_x86.dll";
#endif
			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetApplicationName([MarshalAs(UnmanagedType.LPStr)]string fullApplicationName);

			#region ITobiiGameIntegrationApi

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsApiInitialized();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void Update();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void Shutdown();

			#endregion


			#region ITrackerController

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetTrackerInfo(out TrackerInfoInterop trackerInfoInterop);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetTrackerInfoByUrl([MarshalAs(UnmanagedType.LPStr)] string trackerUrl, out TrackerInfoInterop trackerInfoInterop);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void UpdateTrackerInfos();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetTrackerInfos(out IntPtr trackerInfosPointer, out int numberOfTrackerInfos);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackHMD();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackRectangle(TobiiRectangle rectangle);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackWindow(IntPtr windowHandle);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackTracker([MarshalAs(UnmanagedType.LPStr)]string trackerUrl);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void StopTracking();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsConnected();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsDeviceEnabled();

			#endregion


			#region IStreamsProvider

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetLatestGazePoint(out GazePoint gazePoint);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetLatestHeadPose(out HeadPose headPose);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int GetGazePoints(out IntPtr gazePoints);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int GetHeadPoses(out IntPtr headPoses);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsPresent();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetAutoUnsubscribe(StreamType capability, float timeout);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void UnsetAutoUnsubscribe(StreamType capability);

			// TODO: Implement
			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern bool GetLatestHMDGaze(out HMDGazePoint hmdGazePoint);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern int GetHMDGaze(out IntPtr hmdGazePoints);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void ConvertGazePoint(GazePoint fromGazePoint, GazePoint toGazePoint, UnitType fromUnit, UnitType toUnit);

			#endregion


			#region IStatisticsRecorder

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr GetStatisticsLiteral(StatisticsLiteral statisticsLiteral);

			#endregion


			#region IFeaturesEnabledSettings

			// TODO: Implement and test

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern bool AreSettingsInitialized();

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void InitializeSettings(Feature[] features, int numberOfFeatures);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern bool GetFeatureEnabled(Feature feature);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void SetFeatureEnabled(Feature feature, bool enabled, bool recordStatistics = true);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void RecordSettingsStateToStatistics();

			#endregion


			#region IExtendedView

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetExtendedViewTransformation(out Transformation transformation);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool UpdateExtendedViewAdvancedSettings(ref ExtendedViewAdvancedSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void ResetExtendedViewDefaultHeadPose();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetExtendedViewPaused([MarshalAs(UnmanagedType.I1)] bool paused);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetExtendedViewPausedAfterRecentering();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetExtendedViewPaused();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool UpdateExtendedViewSimpleSettings(ref ExtendedViewSimpleSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetDefaultExtendedViewAdvancedSettings(out ExtendedViewAdvancedSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetExtendedViewAdvancedSettings(out ExtendedViewAdvancedSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetDefaultExtendedViewSimpleSettings(out ExtendedViewSimpleSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetExtendedViewSimpleSettings(out ExtendedViewSimpleSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int GetExtendedViewSensitivityGradientAsPolyLine([In,Out] Vector2d [] allocatedPointsBuf, int maxPoints, ref SensitivityGradientSettings sensitivitySettings);
			
			#endregion


			#region IFilters

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern ResponsiveFilterSettings GetResponsiveFilterSettings();

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void SetResponsiveFilterSettings(ResponsiveFilterSettings responsiveFilterSettings);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern AimAtGazeFilterSettings GetAimAtGazeFilterSettings();

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static void SetAimAtGazeFilterSettings(AimAtGazeFilterSettings settings);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static void GetResponsiveFilterGazePoint(out GazePoint gazePoint);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static void GetAimAtGazeFilterGazePoint(out GazePoint gazePoint, out float gazePointStability);

			#endregion

		}

		private static class x64
		{
#if DEBUG
			internal const string TobiiGameIntegrationDll = "tobii_gameintegration_x64.dll";
#else
			internal const string TobiiGameIntegrationDll = "tobii_gameintegration_x64.dll";
#endif

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetApplicationName([MarshalAs(UnmanagedType.LPStr)]string fullApplicationName);

			#region ITobiiGameIntegrationApi

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsApiInitialized();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void Update();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void Shutdown();

			#endregion


			#region ITrackerController

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetTrackerInfo(out TrackerInfoInterop trackerInfoInterop);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetTrackerInfoByUrl([MarshalAs(UnmanagedType.LPStr)] string trackerUrl, out TrackerInfoInterop trackerInfoInterop);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void UpdateTrackerInfos();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetTrackerInfos(out IntPtr trackerInfosPointer, out int numberOfTrackerInfos);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackHMD();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackRectangle(TobiiRectangle rectangle);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackWindow(IntPtr windowHandle);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool TrackTracker([MarshalAs(UnmanagedType.LPStr)]string trackerUrl);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void StopTracking();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsConnected();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsDeviceEnabled();

			#endregion


			#region IStreamsProvider

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetLatestGazePoint(out GazePoint gazePoint);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetLatestHeadPose(out HeadPose headPose);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int GetGazePoints(out IntPtr gazePoints);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int GetHeadPoses(out IntPtr headPoses);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool IsPresent();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetAutoUnsubscribe(StreamType capability, float timeout);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void UnsetAutoUnsubscribe(StreamType capability);

			// TODO: Implement
			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern bool GetLatestHMDGaze(out HMDGazePoint hmdGazePoint);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern int GetHMDGaze(out IntPtr hmdGazePoints);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void ConvertGazePoint(GazePoint fromGazePoint, GazePoint toGazePoint, UnitType fromUnit, UnitType toUnit);

			#endregion


			#region IStatisticsRecorder

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr GetStatisticsLiteral(StatisticsLiteral statisticsLiteral);

			#endregion


			#region IFeaturesEnabledSettings

			// TODO: Implement and test

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern bool AreSettingsInitialized();

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void InitializeSettings(Feature[] features, int numberOfFeatures);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern bool GetFeatureEnabled(Feature feature);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void SetFeatureEnabled(Feature feature, bool enabled, bool recordStatistics = true);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void RecordSettingsStateToStatistics();

			#endregion


			#region IExtendedView

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetExtendedViewTransformation(out Transformation transformation);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool UpdateExtendedViewAdvancedSettings(ref ExtendedViewAdvancedSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void ResetExtendedViewDefaultHeadPose();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetExtendedViewPaused([MarshalAs(UnmanagedType.I1)] bool paused);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void SetExtendedViewPausedAfterRecentering();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool GetExtendedViewPaused();

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern bool UpdateExtendedViewSimpleSettings(ref ExtendedViewSimpleSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetDefaultExtendedViewAdvancedSettings(out ExtendedViewAdvancedSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetExtendedViewAdvancedSettings(out ExtendedViewAdvancedSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetDefaultExtendedViewSimpleSettings(out ExtendedViewSimpleSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void GetExtendedViewSimpleSettings(out ExtendedViewSimpleSettings settings);

			[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int GetExtendedViewSensitivityGradientAsPolyLine([In, Out] Vector2d[] allocatedPointsBuf, int maxPoints, ref SensitivityGradientSettings sensitivitySettings);

			#endregion


			#region IFilters

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern ResponsiveFilterSettings GetResponsiveFilterSettings();

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern void SetResponsiveFilterSettings(ResponsiveFilterSettings responsiveFilterSettings);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static extern AimAtGazeFilterSettings GetAimAtGazeFilterSettings();

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static void SetAimAtGazeFilterSettings(AimAtGazeFilterSettings settings);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static void GetResponsiveFilterGazePoint(out GazePoint gazePoint);

			//[DllImport(TobiiGameIntegrationDll, CallingConvention = CallingConvention.Cdecl)]
			//internal static void GetAimAtGazeFilterGazePoint(out GazePoint gazePoint, out float gazePointStability);

			#endregion
		}
	}
}
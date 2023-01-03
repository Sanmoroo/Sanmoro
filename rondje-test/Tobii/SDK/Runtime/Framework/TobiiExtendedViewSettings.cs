using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.GameIntegration.Net;
using UnityEngine;

namespace Tobii.Gaming
{
    /// <summary>
    /// Extended View settings. 
    /// 
    /// Be adviced, this is only a subset of all settings available in the the underlying API. 
    /// </summary>
    /// <seealso cref="GameIntegration.Net.ExtendedViewSimpleSettings"/>
    /// <seealso cref="GameIntegration.Net.ExtendedViewAdvancedSettings"/>
    /// 
    [Serializable]
    public class TobiiExtendedViewSettings
    {
        [Tooltip("Ratio between eye and head tracking contribution. 0 for Eye Tracking only, 1 for Head Tracking only. Default 0.85.")]
        public float EyeHeadTrackingRatio       = 0.85f;

        [Tooltip("Controls how much camera will move due to eye movements. Default 0.5.")]
        public float GazeResponsiveness         = 0.5f;

        [Tooltip("Limit horizonal camera rotation in degrees. Default 90deg.")]
        public float CameraMaxAngleYaw          = 90.0f;

        [Tooltip("Limit upwards camera rotation in degrees. Default 70deg.")]
        public float CameraMaxAnglePitchUp      = 70.0f;

        [Tooltip("Limit downwards camera rotation in degrees. Default 40deg.")]
        public float CameraMaxAnglePitchDown    = 40.0f;

        [Tooltip("Controls how much camera will move due to horizonal head movements. Default 1.0.")]
        public float HeadSensitivityPitch       = 1.0f;

        [Tooltip("Controls how much camera will move due to vertical head movements. Default 1.0.")]
        public float HeadSensitivityYaw         = 1.0f;


        public void Clamp()
        {
            EyeHeadTrackingRatio    = Mathf.Clamp(EyeHeadTrackingRatio,     0, 1);
            GazeResponsiveness      = Mathf.Clamp(GazeResponsiveness,       0, 1);
            CameraMaxAngleYaw       = Mathf.Clamp(CameraMaxAngleYaw,        0, 180);
            CameraMaxAnglePitchUp   = Mathf.Clamp(CameraMaxAnglePitchUp,    0, 90);
            CameraMaxAnglePitchDown = Mathf.Clamp(CameraMaxAnglePitchDown,  0, 90);
            HeadSensitivityYaw      = Mathf.Clamp(HeadSensitivityYaw,       0, 5);
            HeadSensitivityPitch    = Mathf.Clamp(HeadSensitivityPitch,     0, 5);
        }
    }
}
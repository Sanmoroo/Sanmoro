// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved

using UnityEngine;

namespace Tobii.Gaming
{
    /// <summary>
    /// Optional convenience initializer for Tobii. Used by the Tobii Initializer prefab.
    ///
    /// Feel free to replace this script with a manual call to TobiiAPI.Start.
    ///
    /// </summary>
    [DefaultExecutionOrder(-10)]
    public class TobiiInitializer : MonoBehaviour
    {
        public TobiiSettings Settings;

        private void Awake()
        {
            TobiiAPI.Start(Settings);
        }
    }
}
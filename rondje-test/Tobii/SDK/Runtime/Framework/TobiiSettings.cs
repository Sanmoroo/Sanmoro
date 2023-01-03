//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.IO;
using Tobii.G2OM;
using UnityEngine;

namespace Tobii.Gaming
{
	[Serializable]
	public class TobiiSettings
	{
		private const int MaximumLayersInUnity = 32;

		/// <summary>
		/// Layers to detect gaze focus on.
		/// </summary>
		public LayerMask LayerMask = G2OM_Description.DefaultLayerMask;


		/// <summary>
		/// Maximum distance to detect gaze focus within.
		/// </summary>
		public float HowLongToKeepCandidatesInSeconds = G2OM_Description.DefaultCandidateMemoryInSeconds;
	}
}

//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using Tobii.G2OM;
using UnityEngine;

namespace Tobii.Gaming
{
	/// <summary>
	/// Component that makes the game object GazeAware, meaning aware if the
	/// user's eye-gaze is on it or not.
	/// </summary>
	[AddComponentMenu("Tobii/Gaze Aware")]
	public class GazeAware : MonoBehaviour, IGazeFocusable
	{
		public bool HasGazeFocus { get; private set; }

		/// <summary>
		/// Function called from the gaze focus handler when the gaze focus for
		/// this object changes. Since the implementation is explicit, it will
		/// not be visible on instances of this component (unless cast to
		/// <see cref="IGazeFocusable"/>).
		/// </summary>
		/// <param name="hasFocus"></param>
		public void GazeFocusChanged(bool hasFocus)
		{
			HasGazeFocus = hasFocus;
		}
	}
}

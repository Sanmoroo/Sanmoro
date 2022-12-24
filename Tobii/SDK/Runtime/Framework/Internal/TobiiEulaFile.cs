using System;
using UnityEngine;

namespace Tobii.Gaming.Internal {

	public class TobiiEulaFile : ScriptableObject
	{
		[SerializeField]
		private bool _tobiiSDKEulaAccepted = false;
		private static readonly string TobiiEulaFilePath = typeof(TobiiEulaFile).Name;
		public static Func<TobiiEulaFile> LoadEulaFile = () => Resources.Load<TobiiEulaFile>(TobiiEulaFilePath);

		public static TobiiEulaFile CreateEulaFile(out bool resourceExists)
		{
			var eulaFile = LoadEulaFile != null ? LoadEulaFile() : null;
			resourceExists = eulaFile != null;
			return resourceExists ? eulaFile : ScriptableObject.CreateInstance<TobiiEulaFile>();
		}

		public bool IsEulaAccepted()
		{
			return _tobiiSDKEulaAccepted;
		}
		
#if UNITY_EDITOR
		public void SetEulaAccepted()
		{
			_tobiiSDKEulaAccepted = true;
			UnityEditor.EditorUtility.SetDirty(this);
			UnityEditor.AssetDatabase.SaveAssets();
		}
#endif
	}
}
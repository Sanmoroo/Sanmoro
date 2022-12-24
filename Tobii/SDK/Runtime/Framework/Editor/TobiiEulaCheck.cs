﻿using System.IO;
using Tobii.Gaming.Internal;

namespace Tobii.Gaming
{
    using UnityEngine;
    using UnityEditor;

    [InitializeOnLoad]
    public class TobiiEulaCheck : EditorWindow
    {
        private static readonly string TobiiEulaFileAssetPath = PathHelper.PathCombine("Assets", "Resources", nameof(TobiiEulaFile) + ".asset");
        private const string EulaUrl = "https://developer.tobii.com/license-agreement/";
        private static TobiiEulaCheck _window;
        private static Texture2D _logo;

        private static string LicenseTextColor
        {
            get { return EditorGUIUtility.isProSkin ? "white" : "black"; }
        }

        private Vector2 scroll;

        private string LicenseTitle
        {
            get { return @"<size=24><color=" + LicenseTextColor + @">Tobii Unity SDK License and Use Agreement</color></size>"; }
        }

        private static string LicenseText
        {
            get
            {
                return
@"<size=12><color=" + LicenseTextColor +
@">Thanks for choosing to use our SDK to add eye tracking functionality to your application!

We hope you want to make your Tobii eye tracking enabled application widely available,
and we want to be transparent about the terms and conditions for using our SDK to
build your software.

Please be sure to click the link at the end of this text and read the agreement in
full before agreeing to it. We’ve tried to make it as easy to read as possible!

A couple of things to know:
When you download a Tobii SDK you agree to an SDK License and Use Agreement
that (with a few exceptions) allows you add eye tracking to your application,
and distribute the necessary files.

One limitation is that you may only use the Tobii SDK to develop applications
using eye tracking for interactive experiences, unless you first obtain a special license.

If you want to develop software which stores or transfers eye tracking data,
such as for behaviour research, advertisement research, usability testing, etc
please contact Tobii licensing at </color><color=#0080ff>sdklicensing@tobii.com</color>

<color=" + LicenseTextColor + @">Read the full Tobii Unity SDK License and Use Agreement here.

By clicking the Accept and Download button below, you are stating that you have read,
and agree to be bound, by the Tobii Unity SDK License and Use Agreement.</color></size>";
            }
        }

        static TobiiEulaCheck()
        {
            TobiiEula.SetEulaFile(LoadOrCreateEulaFile());

            if (TobiiEula.IsEulaAccepted() == false)
            {
                EditorApplication.update += Update;
#if UNITY_2017_2_OR_NEWER
                EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
#else
                EditorApplication.playmodeStateChanged += HandleOnPlayModeChanged;
#endif
            }
        }

        private static string AssetPathFromFullPath(string path)
        {
            var rootPath = Application.dataPath;
            path = path.Replace(rootPath, "Assets");
            path = path.Replace(rootPath.Replace("/", "\\"), "Assets");

            return path;
        }

        private static TobiiEulaFile LoadOrCreateEulaFile()
        {
            var eulaFile = TobiiEulaFile.CreateEulaFile(out var resourceExists);

            if (!resourceExists)
            {
                var assetPath = AssetPathFromFullPath(TobiiEulaFileAssetPath);

                if (File.Exists(TobiiEulaFileAssetPath))
                {
                    eulaFile = AssetDatabase.LoadAssetAtPath<TobiiEulaFile>(assetPath);
                    return eulaFile;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
                AssetDatabase.CreateAsset(eulaFile, assetPath);
                AssetDatabase.SaveAssets();
            }

            return eulaFile;
        }


#if UNITY_2017_2_OR_NEWER
        private static void HandleOnPlayModeChanged(PlayModeStateChange state)
#else
        private static void HandleOnPlayModeChanged()
#endif
        {
            if (EditorApplication.isPlaying && TobiiEula.IsEulaAccepted() == false)
            {
                ShowWindow();
            }
        }

        private static void Update()
        {
            ShowWindow();
            EditorApplication.update -= Update;
        }

        private static void ShowWindow()
        {
#if UNITY_STANDALONE
            _window = GetWindow<TobiiEulaCheck>(true);
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
            _window.titleContent = new GUIContent("Tobii Unity SDK License and Use Agreement");
#else
            _window.title = "Tobii Unity SDK License and Use Agreement";
#endif
            _window.minSize = new Vector2(600, 790);
            _window.position = new Rect(100, 75, 600, 790);
#endif
            _logo = Resources.Load<Texture2D>("TobiiLogo");
        }

        public void OnGUI()
        {

            EditorGUILayout.BeginVertical(EditorStyles.label);

            var rect = GUILayoutUtility.GetRect(position.width, 150, GUI.skin.box);
            if (_logo != null)
            {
                GUI.DrawTexture(rect, _logo, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.LabelField("");


            var style = new GUIStyle();
            style.richText = true;
            EditorGUILayout.LabelField(LicenseTitle, style, GUILayout.Height(30));

            EditorGUILayout.LabelField("");

            EditorGUILayout.BeginVertical("Box");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.SelectableLabel(LicenseText, style, GUILayout.Height(400));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();

            EditorGUILayout.LabelField("");

            EditorGUILayout.HelpBox("To use this package please read and accept the Tobii Unity SDK License and Use Agreement.", MessageType.Info);

            EditorGUILayout.LabelField("");

            if (GUILayout.Button("Read the Tobii Unity SDK License and Use Agreement", GUILayout.Height(30)))
            {
                Application.OpenURL(EulaUrl);
            }

            EditorGUILayout.LabelField("");

            EditorGUILayout.BeginHorizontal(EditorStyles.label, GUILayout.Height(40));

            if (GUILayout.Button("Decline", GUILayout.Height(30)))
            {
                _window.Close();
            }

            GUILayout.Button("", EditorStyles.miniBoldLabel);

            if (GUILayout.Button("Accept", GUILayout.Height(30)))
            {
#if UNITY_2017_2_OR_NEWER
                EditorApplication.playModeStateChanged -= HandleOnPlayModeChanged;
#else
                EditorApplication.playmodeStateChanged -= HandleOnPlayModeChanged;
#endif
                TobiiEula.SetEulaAccepted();
                _window.Close();
            }



            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}

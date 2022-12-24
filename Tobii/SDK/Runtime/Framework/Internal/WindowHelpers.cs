//-----------------------------------------------------------------------
// Copyright 2015 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using System.Text;
using System.Diagnostics;
using AOT;

namespace Tobii.Gaming.Internal
{
	/// <summary>
	/// Contains utility functions for window handling.
	/// </summary>
	internal static class WindowHelpers
	{
		private const string GameViewCaption = "UnityEditor.GameView";
		private const string UnityContainerClassName = "UnityContainerWndClass";
		private static IntPtr Window;
		private static int ProcessId;
		/// <summary>
		/// Shows the current window.
		/// </summary>
		internal static void ShowCurrentWindow()
		{
			IntPtr hwnd = FindWindowWithThreadProcessId(Process.GetCurrentProcess().Id);
			Win32Helpers.ShowWindowAsync(hwnd, Win32Helpers.SW_SHOWDEFAULT);
		}

		[MonoPInvokeCallback(typeof(Win32Helpers.EnumWindowsProc))]
		private static bool FindWindowWithThreadProcessIdCallback(IntPtr wnd, IntPtr param)
		{
			var windowProcessId = 0;
			Win32Helpers.GetWindowThreadProcessId(wnd, out windowProcessId);
			if (windowProcessId != ProcessId || !IsMainWindow(wnd))
			{
				return true;
			}

			Window = wnd;
			return false;
		}
		
		internal static IntPtr FindWindowWithThreadProcessId(int processId)
		{
			ProcessId = processId;
			Window = new IntPtr();

			Win32Helpers.EnumWindows(FindWindowWithThreadProcessIdCallback, IntPtr.Zero);

			if (Window.Equals(IntPtr.Zero))
			{
				UnityEngine.Debug.LogError("Could not find any window with process id " + processId);
			}

			return Window;
		}

		private static bool IsMainWindow(IntPtr hwnd)
		{
			return (Win32Helpers.GetWindow(hwnd, Win32Helpers.GW_OWNER) == IntPtr.Zero) && Win32Helpers.IsWindowVisible(hwnd);
		}

		[MonoPInvokeCallback(typeof(Win32Helpers.EnumWindowsProc))]
		private static bool GameViewWindowHandleCallback(IntPtr hWnd, IntPtr lParam)
		{
			if (!Win32Helpers.IsWindowVisible(hWnd))
			{
				return true;
			}

			var windowProcessId = 0;
			Win32Helpers.GetWindowThreadProcessId(hWnd, out windowProcessId);

			if (windowProcessId == ProcessId)
			{
				StringBuilder builder = new StringBuilder(256);
				Win32Helpers.GetClassName(hWnd, builder, 256);

				if (builder.ToString() == UnityContainerClassName)
				{
					//Ok, we found one of our containers, let's try to find the game view handle among the children
					Win32Helpers.EnumChildWindows(hWnd, delegate(IntPtr childHwnd, IntPtr childParam)
					{
						if (!Win32Helpers.IsWindowVisible(childHwnd))
						{
							return true;
						}

						int childLength = Win32Helpers.GetWindowTextLength(childHwnd);
						if (childLength == 0)
						{
							return true;
						}

						StringBuilder childBuilder = new StringBuilder(childLength);
						Win32Helpers.GetWindowText(childHwnd, childBuilder, childLength + 1);

						if (childBuilder.ToString() == GameViewCaption)
						{
							//Found it!
							Window = childHwnd;
							return false;
						}

						return true;
					}, IntPtr.Zero);
				}
			}

			return true;
		}
		
		internal static IntPtr GetGameViewWindowHandle(int processId)
		{
			ProcessId = processId;
			Window = new IntPtr();

			Win32Helpers.EnumWindows(GameViewWindowHandleCallback, IntPtr.Zero);

			if (Window.Equals(IntPtr.Zero))
			{
				UnityEngine.Debug.LogError("Could not find game view!");
			}

			return Window;
		}
	}

}

#else
using System;
namespace Tobii.Gaming.Internal
{
    internal static class WindowHelpers
    {
        public static void ShowCurrentWindow()
        {
            throw new InvalidOperationException("Not available on this platform.");
        }
    }
}
#endif

using System;
using System.Linq;
using System.Reflection;
using NGraphics.Custom;
using NGraphics.Custom.Interfaces;
#if __ANDROID__
using NGraphics.Android.Custom;
#elif NETFX_CORE && !__NETCORE__
using NGraphics.WindowsStore.Custom;
#elif __IOS__
using NGraphics.iOS.Custom;
#elif MAC
using NGraphics.iOS.Custom;
#else
using NGraphics.Net.Custom;
#endif

namespace NGraphics.Custom
{
	public static class Platforms
	{
		static readonly IPlatform nulll = new NullPlatform ();
		static IPlatform current = null;

		public static IPlatform Null { get { return nulll; } }

		public static IPlatform Current {
			get {
				if (current == null) {
#if MAC
					current = new ApplePlatform ();
#elif __IOS__
					current = new ApplePlatform ();
#elif __ANDROID__
					current = new AndroidPlatform ();
#elif NETFX_CORE && !__NETCORE__
          current = new WinRTPlatform();
#else
                    current = new SystemDrawingPlatform ();
          #endif
                             }
				return current;
			}
		}
	}
}

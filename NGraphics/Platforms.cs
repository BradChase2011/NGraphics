using System;
using System.Linq;
using System.Reflection;
using NGraphics.Interfaces;
using NGraphics.Net;
#if __ANDROID__
using NGraphics.Android;
#endif

namespace NGraphics
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
					#else
					current = new SystemDrawingPlatform ();
					#endif
				}
				return current;
			}
		}
	}
}

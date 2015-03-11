﻿using NUnit.Framework;
using System.IO;
using System;
using System.Reflection;
using NGraphics.Interfaces;

namespace NGraphics.Test
{
	[TestFixture]
	public class ColorTests : PlatformTest
	{
		[Test]
		public void HSB ()
		{
			// http://en.wikipedia.org/wiki/HSL_and_HSV#Examples
			var tests = new [] {
				Tuple.Create (new Color (1.000, 1.000, 1.000), Color.FromHSB (  0.0/360.0, 0.000, 1.000)),
				Tuple.Create (new Color (0.500, 0.500, 0.500), Color.FromHSB (  0.0/360.0, 0.000, 0.500)),
				Tuple.Create (new Color (0.000, 0.000, 0.000), Color.FromHSB (  0.0/360.0, 0.000, 0.000)),
				Tuple.Create (new Color (1.000, 0.000, 0.000), Color.FromHSB (  0.0/360.0, 1.000, 1.000)),
				Tuple.Create (new Color (0.750, 0.750, 0.000), Color.FromHSB ( 60.0/360.0, 1.000, 0.750)),
				Tuple.Create (new Color (0.000, 0.500, 0.000), Color.FromHSB (120.0/360.0, 1.000, 0.500)),
				Tuple.Create (new Color (0.500, 1.000, 1.000), Color.FromHSB (180.0/360.0, 0.500, 1.000)),
				Tuple.Create (new Color (0.500, 0.500, 1.000), Color.FromHSB (240.0/360.0, 0.500, 1.000)),
				Tuple.Create (new Color (0.750, 0.250, 0.750), Color.FromHSB (300.0/360.0, 0.667, 0.750)),
				Tuple.Create (new Color (0.628, 0.643, 0.142), Color.FromHSB ( 61.8/360.0, 0.779, 0.643)),
				Tuple.Create (new Color (0.255, 0.104, 0.918), Color.FromHSB (251.1/360.0, 0.887, 0.918)),
				Tuple.Create (new Color (0.116, 0.675, 0.255), Color.FromHSB (134.9/360.0, 0.828, 0.675)),
				Tuple.Create (new Color (0.941, 0.785, 0.053), Color.FromHSB ( 49.5/360.0, 0.944, 0.941)),
				Tuple.Create (new Color (0.704, 0.187, 0.897), Color.FromHSB (283.7/360.0, 0.792, 0.897)),
				Tuple.Create (new Color (0.931, 0.463, 0.316), Color.FromHSB ( 14.3/360.0, 0.661, 0.931)),
				Tuple.Create (new Color (0.998, 0.974, 0.532), Color.FromHSB ( 56.9/360.0, 0.467, 0.998)),
				Tuple.Create (new Color (0.099, 0.795, 0.591), Color.FromHSB (162.4/360.0, 0.875, 0.795)),
				Tuple.Create (new Color (0.211, 0.149, 0.597), Color.FromHSB (248.3/360.0, 0.750, 0.597)),
				Tuple.Create (new Color (0.495, 0.493, 0.721), Color.FromHSB (240.5/360.0, 0.316, 0.721)),
			};
			var s = 32;
			var canvas = Platforms.Current.CreateImageCanvas (new Size (s * tests.Length, s));

			foreach (var t in tests) {
				var rect = new Rect (0, 0, s, s);
				canvas.FillRectangle (rect, t.Item2);
				canvas.Translate (s, 0);

				Assert.That (t.Item2.R, Is.InRange (t.Item1.R - 1, t.Item1.R + 1));
				Assert.That (t.Item2.G, Is.InRange (t.Item1.G - 1, t.Item1.G + 1));
				Assert.That (t.Item2.B, Is.InRange (t.Item1.B - 1, t.Item1.B + 1));
			}

			canvas.GetImage ().SaveAsPng (GetPath ("Color.HSB.png"));
		}
	}
}


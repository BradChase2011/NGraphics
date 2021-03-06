﻿#if VSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;
#else
using NUnit.Framework;
#endif
using System.IO;
using System;
using System.Reflection;
using System.Threading.Tasks;
using NGraphics.Custom.Codes;
using NGraphics.Custom.Models;
using NGraphics.Custom.Models.Brushes;

namespace NGraphics.Test
{
  [TestFixture]
  public class BrushTests : PlatformTest
  {
    [Test]
    public async Task RectLinearGradient()
    {
      var canvas = Platforms.Current.CreateImageCanvas(new Size(100));

      var rect = new Rect(0, 10, 100, 80);
      var brush = new LinearGradientBrush(
        Point.Zero,
        Point.OneY,
        Colors.Green,
        Colors.LightGray);

      canvas.DrawRectangle(rect, brush: brush);

      await SaveImage(canvas, "Brush.RectLinearGradient.png");
    }

    [Test]
    public async Task RectAbsLinearGradient()
    {
      var canvas = Platforms.Current.CreateImageCanvas(new Size(100));

      var rect = new Rect(0, 10, 100, 80);
      var brush = new LinearGradientBrush(
        Point.Zero,
        new Point(0, 200),
        Colors.Yellow,
        Colors.Red);
      brush.Absolute = true;

      canvas.DrawRectangle(rect, brush: brush);

      await SaveImage(canvas, "Brush.RectAbsLinearGradient.png");
    }

    [Test]
    public async Task EllipseRadialGradient()
    {
      var canvas = Platforms.Current.CreateImageCanvas(new Size(100));

      var rect = new Rect(0, 10, 100, 80);
      var brush = new RadialGradientBrush(
        new Point(0.5, 0.5),
        new Size(0.5, 0.125),
        Colors.Green,
        Colors.LightGray);

      canvas.DrawEllipse(rect, brush: brush);

      await SaveImage(canvas, "Brush.EllipseRadialGradient.png");
    }
  }
}

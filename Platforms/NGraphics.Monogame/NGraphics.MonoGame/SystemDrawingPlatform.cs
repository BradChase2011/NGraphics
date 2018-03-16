using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using NGraphics.Custom.Codes;
using NGraphics.Custom.Interfaces;
using NGraphics.Custom.Models;
using NGraphics.Custom.Models.Brushes;
using NGraphics.Custom.Models.Operations;
using NGraphics.Custom.Models.Transforms;
using Color = NGraphics.Custom.Models.Color;
using Font = NGraphics.Custom.Models.Font;
using LinearGradientBrush = NGraphics.Custom.Models.Brushes.LinearGradientBrush;
using Pen = NGraphics.Custom.Models.Pen;
using Point = NGraphics.Custom.Models.Point;
using Size = NGraphics.Custom.Models.Size;
using SolidBrush = NGraphics.Custom.Models.Brushes.SolidBrush;

namespace NGraphics.Net.Custom
{
  public class SystemDrawingPlatform : IPlatform
  {
    public string Name
    {
      get { return "Net"; }
    }

    public IImageCanvas CreateImageCanvas(Size size, double scale = 1.0, bool transparency = true)
    {
      var pixelWidth = (int) Math.Ceiling(size.Width*scale);
      var pixelHeight = (int) Math.Ceiling(size.Height*scale);
      var format = transparency ? PixelFormat.Format32bppPArgb : PixelFormat.Format24bppRgb;
      var bitmap = new Bitmap(pixelWidth, pixelHeight, format);
      return new BitmapCanvas(bitmap, scale);
    }

    public IImage LoadImage(Stream stream)
    {
      var image = Image.FromStream(stream);
      return new ImageImage(image);
    }

    public IImage LoadImage(string path)
    {
      var image = Image.FromFile(path);
      return new ImageImage(image);
    }

    public IImage CreateImage(Color[] colors, int width, double scale = 1.0)
    {
      var pixelWidth = width;
      var pixelHeight = colors.Length/width;
      var format = PixelFormat.Format32bppArgb;
      Bitmap bitmap;
      unsafe
      {
        fixed (Color* c = colors)
        {
          bitmap = new Bitmap(pixelWidth, pixelHeight, pixelWidth*4, format, new IntPtr(c));
        }
      }
      return new ImageImage(bitmap);
    }
  }

  public class ImageImage : IImage
  {
      readonly Image image;

      public Image Image
      {
          get
          {
              return image;
          }
      }

      public ImageImage(Image image)
      {
          this.image = image;
      }

      public Size Size
      {
          get { throw new NotImplementedException(); }
      }

      public double Scale
      {
          get { throw new NotImplementedException(); }
      }

      public void SaveAsPng(string path)
      {
          image.Save(path, ImageFormat.Png);
      }

      public void SaveAsPng(Stream stream)
      {
          image.Save(stream, ImageFormat.Png);
      }
  }

  public class BitmapCanvas : GraphicsCanvas, IImageCanvas
  {
      readonly Bitmap bitmap;
      readonly double scale;

      public Size Size { get { return new Size(bitmap.Width / scale, bitmap.Height / scale); } }
      public double Scale { get { return scale; } }

      public BitmapCanvas(Bitmap bitmap, double scale = 1.0)
          : base(Graphics.FromImage(bitmap))
      {
          this.bitmap = bitmap;
          this.scale = scale;

          graphics.ScaleTransform((float)scale, (float)scale);
      }

      public IImage GetImage()
      {
          return new ImageImage(bitmap);
      }
  }

  public class GraphicsCanvas : ICanvas
  {
    protected readonly Graphics graphics;
    private readonly Stack<GraphicsState> stateStack = new Stack<GraphicsState>();

    public GraphicsCanvas(Graphics graphics)
    {
      this.graphics = graphics;

      graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
      graphics.SmoothingMode = SmoothingMode.HighQuality;
    }

    public void SaveState()
    {
      var s = graphics.Save();
      stateStack.Push(s);
    }

    public void Transform(Transform transform)
    {
        try
        {
            graphics.MultiplyTransform(new Matrix(
                (float)transform.A, (float)transform.B,
                (float)transform.C, (float)transform.D,
                (float)transform.E, (float)transform.F), MatrixOrder.Prepend);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public void RestoreState()
    {
      if (stateStack.Count > 0)
      {
        var s = stateStack.Pop();
        graphics.Restore(s);
      }
    }

    public void DrawText(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left,
      Pen pen = null, BaseBrush baseBrush = null)
    {
      if (baseBrush == null)
        return;
      var sdfont = new System.Drawing.Font(font.Family, (float) font.Size, FontStyle.Regular, GraphicsUnit.Pixel);
      var sz = graphics.MeasureString(text, sdfont);
      var point = frame.Position;
      var fr = new Rect(point, new Size(sz.Width, sz.Height));
      graphics.DrawString(text, sdfont, baseBrush.GetBrush(fr),
        Conversions.GetPointF(point - new Point(0, sdfont.Height)));
    }

    public void DrawPath(IEnumerable<PathOperation> ops, Pen pen = null, BaseBrush baseBrush = null)
    {
      using (var path = new GraphicsPath())
      {
        var bb = new BoundingBoxBuilder();

        var position = Point.Zero;

        foreach (var op in ops)
        {
          var start = op as StartFigure;

          if (start != null)
          {
            path.StartFigure();
            continue;
          }

          var moveTo = op as MoveTo;
          if (moveTo != null)
          {
              path.StartFigure();
            continue;
          }
          var lineTo = op as LineTo;
          if (lineTo != null)
          {
            var p = lineTo.Start;
            path.AddLine(Conversions.GetPointF(lineTo.Start), Conversions.GetPointF(lineTo.End));
            position = p;
            bb.Add(p);
            continue;
          }
          var arcTo = op as ArcTo;
          if (arcTo != null)
          {
            var p = arcTo.Point;
            path.AddLine(Conversions.GetPointF(position), Conversions.GetPointF(p));
            position = p;
            bb.Add(p);
            continue;
          }
          var curveTo = op as CurveTo;
          if (curveTo != null)
          {
            path.AddBezier(Conversions.GetPointF(curveTo.Start), Conversions.GetPointF(curveTo.FirstControlPoint),
              Conversions.GetPointF(curveTo.SecondControlPoint), Conversions.GetPointF(curveTo.End));
            bb.Add(curveTo.Start);
            bb.Add(curveTo.FirstControlPoint);
            bb.Add(curveTo.SecondControlPoint);
            bb.Add(curveTo.End);
            continue;
          }
          var closePath = op as ClosePath;
          if (closePath != null)
          {
            path.CloseFigure();
            continue;
          }

          throw new NotSupportedException("Path Op " + op);
        }

        var frame = bb.BoundingBox;
        if (baseBrush != null)
        {
          graphics.FillPath(baseBrush.GetBrush(frame), path);
        }
        if (pen != null)
        {
          var r = Conversions.GetRectangleF(frame);
          graphics.DrawPath(pen.GetPen(), path);
        }
      }
    }

    public void DrawRectangle(Rect frame, Pen pen = null, BaseBrush baseBrush = null)
    {
      if (baseBrush != null)
      {
        graphics.FillRectangle(baseBrush.GetBrush(frame), Conversions.GetRectangleF(frame));
      }
      if (pen != null)
      {
        var r = Conversions.GetRectangleF(frame);
        graphics.DrawRectangle(pen.GetPen(), r.X, r.Y, r.Width, r.Height);
      }
    }

    public void DrawEllipse(Rect frame, Pen pen = null, BaseBrush baseBrush = null)
    {
      if (baseBrush != null)
      {
        graphics.FillEllipse(baseBrush.GetBrush(frame), Conversions.GetRectangleF(frame));
      }
      if (pen != null)
      {
        graphics.DrawEllipse(pen.GetPen(), Conversions.GetRectangleF(frame));
      }
    }

    public void DrawImage(IImage image, Rect frame, double alpha = 1.0)
    {
        var ii = image as ImageImage;
        if (ii != null)
        {
            if (alpha < 0.999)
            {
                var i = new ImageAttributes();
                var mat = new ColorMatrix(new float[][] { 
						new[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
						new[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
						new[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
						new[] { 0.0f, 0.0f, 0.0f, (float)alpha, 0.0f },
						new[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
					});
                i.SetColorMatrix(mat);
                var size = ii.Image.Size;
                graphics.DrawImage(ii.Image, Conversions.GetRectangle(frame),
                    0, 0, size.Width, size.Height, GraphicsUnit.Pixel, i);
            }
            else
            {
                graphics.DrawImage(ii.Image, Conversions.GetRectangleF(frame));
            }
        }
    }
  }

  public static class Conversions
  {
    public static System.Drawing.Color GetColor(this Color color)
    {
      return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    public static System.Drawing.Pen GetPen(this Pen pen)
    {
      var windowsPen =  new System.Drawing.Pen(GetColor(pen.Color), (float) pen.Width);

      switch (pen.LineJoin)
      {
        case SvgStrokeLineJoin.Round:
          windowsPen.LineJoin = LineJoin.Round;
          break;
        case SvgStrokeLineJoin.Bevel:
          windowsPen.LineJoin = LineJoin.Bevel;
          break;
        case SvgStrokeLineJoin.Miter:
          windowsPen.LineJoin = LineJoin.Miter;
          break;
      }

      switch (pen.LineCap)
      {
        case SvgStrokeLineCap.Round:
          windowsPen.StartCap = LineCap.Round;
          windowsPen.EndCap = LineCap.Round;
          break;
        case SvgStrokeLineCap.Square:
          windowsPen.StartCap = LineCap.Square;
          windowsPen.EndCap = LineCap.Square;
          break;
      }

      return windowsPen;
    }
    public static System.Drawing.Rectangle GetRectangle(this Rect frame)
    {
        return new System.Drawing.Rectangle((int)frame.X, (int)frame.Y, (int)frame.Width, (int)frame.Height);
    }

    private static ColorBlend BuildBlend(List<GradientStop> stops, bool reverse = false)
    {
      if (stops.Count < 2)
        return null;

      var s1 = stops[0];
      var s2 = stops[stops.Count - 1];

      // Build the blend
      var n = stops.Count;
      var an = 0;
      if (s1.Offset != 0)
      {
        an++;
      }
      if (s2.Offset != 1)
      {
        an++;
      }
      var blend = new ColorBlend(n + an);
      var o = 0;
      if (s1.Offset != 0)
      {
        blend.Colors[0] = GetColor(s1.Color);
        blend.Positions[0] = 0;
        o = 1;
      }
      for (var i = 0; i < n; i++)
      {
        blend.Colors[i + o] = GetColor(stops[i].Color);
        blend.Positions[i + o] = (float) stops[i].Offset;
      }
      if (s2.Offset != 1)
      {
        blend.Colors[n + an - 1] = GetColor(s1.Color);
        blend.Positions[n + an - 1] = 1;
      }

      if (reverse)
      {
        for (var i = 0; i < blend.Positions.Length; i++)
        {
          blend.Positions[i] = 1 - blend.Positions[i];
        }
        Array.Reverse(blend.Positions);
        Array.Reverse(blend.Colors);
      }

      return blend;
    }

    public static Brush GetBrush(this BaseBrush baseBrush, Rect frame)
    {
     var cb = baseBrush as SolidBrush;
			if (cb != null) {
				return new System.Drawing.SolidBrush (cb.Color.GetColor ());
			}

            var lgb = baseBrush as LinearGradientBrush;
            if (lgb != null) {
                var s = lgb.Absolute ? lgb.Start : frame.Position + lgb.Start * frame.Size;
                var e = lgb.Absolute ? lgb.End : frame.Position + lgb.End * frame.Size;
                var b = new System.Drawing.Drawing2D.LinearGradientBrush (GetPointF (s), GetPointF (e), System.Drawing.Color.Black, System.Drawing.Color.Black);
                var bb = BuildBlend (lgb.Stops);
                if (bb != null) {
                    b.InterpolationColors = bb;
                }
                return b;
            }

            var rgb = baseBrush as RadialGradientBrush;
            if (rgb != null) {
                var r = rgb.GetAbsoluteRadius (frame);
                var c = rgb.GetAbsoluteCenter (frame);
                var path = new GraphicsPath ();
                path.AddEllipse (GetRectangleF (new Rect (c - r, 2 * r)));
                var b = new PathGradientBrush (path);
                var bb = BuildBlend (rgb.Stops, true);
                if (bb != null) {
                    b.InterpolationColors = bb;
                }
                return b;
            }

			throw new NotImplementedException ("Brush " + baseBrush);
    }

    public static PointF GetPointF(Point point)
    {
      return new PointF((float) point.X, (float) point.Y);
    }

    public static RectangleF GetRectangleF(Rect frame)
    {
      return new RectangleF((float) frame.X, (float) frame.Y, (float) frame.Width, (float) frame.Height);
    }
  }
}


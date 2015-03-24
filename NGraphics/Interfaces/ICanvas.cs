using System;
using System.Collections.Generic;

namespace NGraphics.Interfaces
{
	public interface ICanvas
	{
		void SaveState();
		void Transform (Transform transform);
		void RestoreState ();

		void DrawText (string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null, Brush brush = null);
		void DrawPath (IEnumerable<PathOp> ops, Pen pen = null, Brush brush = null);
		void DrawRectangle (Rect frame, Pen pen = null, Brush brush = null);
		void DrawEllipse (Rect frame, Pen pen = null, Brush brush = null);
		void DrawImage (IImage image, Rect frame);
	}

	public static class CanvasEx
	{
		public static void Translate (this ICanvas canvas, double dx, double dy)
		{
			canvas.Transform (new Translate (dx, dy));
		}
		public static void Translate (this ICanvas canvas, Size translation)
		{
			canvas.Transform (new Translate (translation));
		}
		public static void Translate (this ICanvas canvas, Point translation)
		{
			canvas.Transform (new Translate (translation));
		}
		/// <summary>
		/// Rotate the specified canvas by the given angle (in degrees).
		/// </summary>
		/// <param name="angle">Angle in degrees.</param>
		public static void Rotate (this ICanvas canvas, double angle)
		{
			canvas.Transform (new Rotate (angle));
		}
		/// <param name="angle">Angle in degrees.</param>
		public static void Rotate (this ICanvas canvas, double angle, Point point)
		{
			canvas.Translate (point);
			canvas.Rotate (angle);
			canvas.Translate (-point);
		}
		/// <param name="angle">Angle in degrees.</param>
		public static void Rotate (this ICanvas canvas, double angle, double x, double y)
		{
			canvas.Rotate (angle, new Point (x, y));
		}

		public static void Scale (this ICanvas canvas, double sx, double sy)
		{
			canvas.Transform (new Scale (sx, sy));
		}
		public static void Scale (this ICanvas canvas, Size scale)
		{
			canvas.Transform (new Scale (scale));
		}
		public static void Scale (this ICanvas canvas, Size scale, Point point)
		{
			canvas.Translate (point);
			canvas.Scale (scale);
			canvas.Translate (-point);
		}
		public static void Scale (this ICanvas canvas, double sx, double sy, double x, double y)
		{
			canvas.Scale (new Size (sx, sy), new Point (x, y));
		}


		public static void DrawRectangle (this ICanvas canvas, double x, double y, double width, double height, Pen pen = null, Brush brush = null)
		{
			canvas.DrawRectangle (new Rect (x, y, width, height), pen, brush);
		}
		public static void DrawRectangle (this ICanvas canvas, Point position, Size size, Pen pen = null, Brush brush = null)
		{
			canvas.DrawRectangle (new Rect (position, size), pen, brush);
		}
		public static void FillRectangle (this ICanvas canvas, Rect frame, Color color)
		{
			canvas.DrawRectangle (frame, brush: new SolidBrush (color));
		}
		public static void FillRectangle (this ICanvas canvas, Rect frame, Brush brush)
		{
			canvas.DrawRectangle (frame, brush: brush);
		}
		public static void StrokeRectangle (this ICanvas canvas, Rect frame, Color color, double width = 1.0)
		{
			canvas.DrawRectangle (frame, pen: new Pen (color, width));
		}

		public static void DrawEllipse (this ICanvas canvas, double x, double y, double width, double height, Pen pen = null, Brush brush = null)
		{
			canvas.DrawEllipse (new Rect (x, y, width, height), pen, brush);
		}
		public static void DrawEllipse (this ICanvas canvas, Point position, Size size, Pen pen = null, Brush brush = null)
		{
			canvas.DrawEllipse (new Rect (position, size), pen, brush);
		}
		public static void FillEllipse (this ICanvas canvas, Rect frame, Color color)
		{
			canvas.DrawEllipse (frame, brush: new SolidBrush (color));
		}
		public static void FillEllipse (this ICanvas canvas, Rect frame, Brush brush)
		{
			canvas.DrawEllipse (frame, brush: brush);
		}
		public static void StrokeEllipse (this ICanvas canvas, Rect frame, Color color, double width = 1.0)
		{
			canvas.DrawEllipse (frame, pen: new Pen (color, width));
		}

		public static void DrawPath (this ICanvas canvas, Action<Path> draw, Pen pen = null, Brush brush = null)
		{
			var p = new Path (pen, brush);
			draw (p);
			p.Draw (canvas);
		}

		public static void DrawLine (this ICanvas canvas, Point start, Point end, Pen pen, bool isAbsolute)
		{
			var p = new Path { Pen = pen };
			p.MoveTo (start, isAbsolute);
			p.LineTo ( end);
			p.Draw (canvas);				
		}
		public static void DrawLine (this ICanvas canvas, Point start, Point end, Color color, bool isAbsolute, double width = 1.0)
		{
			var p = new Path { Pen = new Pen (color, width) };
			p.MoveTo (start, isAbsolute);
			p.LineTo (end);
			p.Draw (canvas);				
		}
		public static void DrawLine (this ICanvas canvas, double x1, double y1, double x2, double y2, Color color, bool isAbsolute, double width = 1.0)
		{
			var p = new Path { Pen = new Pen (color, width) };
			p.MoveTo (x1, y1, isAbsolute);
			p.LineTo (x2, y2, isAbsolute);
			p.Draw (canvas);				
		}
	}
}

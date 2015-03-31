using System;
using System.Collections.Generic;
using NGraphics.Interfaces;
using NGraphics.Models;
using NGraphics.Models.Operations;

namespace NGraphics
{
    public class GraphicCanvas : ICanvas
    {
        public GraphicCanvas(Size size)
        {
            Graphic = new Graphic(size);
        }

        public Graphic Graphic { get; private set; }

        public void SaveState()
        {
            throw new NotImplementedException();
        }

        public void Transform(Transform transform)
        {
            throw new NotImplementedException();
        }

        public void RestoreState()
        {
            throw new NotImplementedException();
        }

        public void DrawText(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left,
            Pen pen = null, Brush brush = null)
        {
            throw new NotImplementedException();
        }

        public void DrawPath(IEnumerable<PathOperation> commands, Pen pen = null, Brush brush = null)
        {
            Graphic.Children.Add(new Path(commands, pen, brush));
        }

        public void DrawRectangle(Rect frame, Pen pen = null, Brush brush = null)
        {
            Graphic.Children.Add(new Rectangle(frame, pen, brush));
        }

        public void DrawEllipse(Rect frame, Pen pen = null, Brush brush = null)
        {
            Graphic.Children.Add(new Ellipse(frame, pen, brush));
        }

        public void DrawImage(IImage image, Rect frame)
        {
            throw new NotImplementedException();
        }
    }
}
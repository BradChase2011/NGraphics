using System.Collections.Generic;
using System.IO;
using NGraphics.Codes;
using NGraphics.Interfaces;
using NGraphics.Models;
using NGraphics.Models.Brushes;
using NGraphics.Models.Operations;
using NGraphics.Models.Transforms;

namespace NGraphics.Custom
{
    public class NullPlatform : IPlatform
    {
        public string Name { get { return "Null"; } }

        public IImageCanvas CreateImageCanvas(Size size, double scale = 1.0, bool transparency = true)
        {
            return new NullImageSurface();
        }

        public IImage CreateImage(Color[] colors, int width, double scale = 1.0)
        {
            return new NullImage();
        }

        public IImage LoadImage(Stream stream)
        {
            return new NullImage();
        }
        public IImage LoadImage(string path)
        {
            return new NullImage();
        }

        class NullImageSurface : IImageCanvas
        {
            public IImage GetImage()
            {
                return new NullImage();
            }
            public Size Size { get { return Size.Zero; } }
            public double Scale { get { return 1.0; } }
            public void SaveState()
            {
            }
            public void Transform(Transform transform)
            {
            }
            public void RestoreState()
            {
            }
            public void DrawText(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null, BaseBrush brush = null)
            {
            }
            public void DrawPath(IEnumerable<PathOperation> ops, Pen pen = null, BaseBrush brush = null)
            {
            }
            public void DrawRectangle(Rect frame, Pen pen = null, BaseBrush brush = null)
            {
            }
            public void DrawEllipse(Rect frame, Pen pen = null, BaseBrush brush = null)
            {
            }
            public void DrawImage(IImage image, Rect frame, double alpha = 1.0)
            {
            }
        }

        class NullImage : IImage
        {
            public Size Size { get { return Size.Zero; } }
            public double Scale { get { return 1.0; } }
            public void SaveAsPng(string path)
            {
            }
            public void SaveAsPng(Stream stream)
            {
            }
        }
    }
}

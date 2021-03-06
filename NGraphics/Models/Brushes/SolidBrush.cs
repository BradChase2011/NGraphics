using NGraphics.Custom.Codes;

namespace NGraphics.Custom.Models.Brushes
{
    public class SolidBrush : BaseBrush
    {
        public SolidBrush()
        {
            Color = Colors.Black;
            FillMode = FillMode.NonZero;
        }

        public SolidBrush(Color color)
        {
            Color = color;
            FillMode = FillMode.NonZero;
        }

        public Color Color;
        public FillMode FillMode;
    }
}
namespace NGraphics.Models
{
    public class SolidBrush : BaseBrush
    {
        public SolidBrush()
        {
            Color = Colors.Black;
            FillMode = FillMode.Regular;
        }

        public SolidBrush(Color color)
        {
            Color = color;
            FillMode = FillMode.Regular;
        }

        public Color Color;
        public FillMode FillMode;
    }
}
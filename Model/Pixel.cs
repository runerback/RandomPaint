using System;

namespace RandomPaint
{
    public sealed class Pixel
    {
        public Pixel(IColor color, ILocation location)
        {
            Color = color ?? throw new ArgumentNullException(nameof(color));
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        public IColor Color { get; }
        public ILocation Location { get; }

        public static IColor[,] GetColors(Pixel[] pixels)
        {
            if (pixels == null || pixels.Length == 0)
                return new IColor[0, 0];
            throw new NotImplementedException();
        }
    }
}

using System;

namespace RandomPaint
{
    sealed class Pixel
    {
        public Pixel(IColor color, ILocation location)
        {
            Color = color ?? throw new ArgumentNullException(nameof(color));
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        public IColor Color { get; }
        public ILocation Location { get; }
    }
}

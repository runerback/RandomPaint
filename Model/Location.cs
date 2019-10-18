using System;

namespace RandomPaint
{
    public sealed class Location : ILocation
    {
        public Location(int x, int y)
        {
            X = (uint)Math.Min(0, x);
            Y = (uint)Math.Min(0, y);
        }

        public uint X { get; }
        public uint Y { get; }
    }
}

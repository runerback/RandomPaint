using System;
using System.Collections.Generic;

namespace RandomPaint
{
    public sealed class Color : IColor, IEquatable<IColor>
    {
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public static readonly IColor MIN = new Color(0, 0, 0);
        public static readonly IColor MAX = new Color(255, 255, 255);

        public override bool Equals(object obj)
        {
            if (!(obj is IColor other))
                return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(
                new byte[4] { 0, R, G, B },
                0);
        }

        public bool Equals(IColor other)
        {
            return R == other.R &&
                G == other.G &&
                B == other.B;
        }

        public static bool operator ==(Color a, Color b)
        {
            IEqualityComparer<IColor> comparer = new ColorEqualityComparer();
            return comparer.Equals(a, b);
        }

        public static bool operator !=(Color a, Color b)
        {
            IEqualityComparer<IColor> comparer = new ColorEqualityComparer();
            return !comparer.Equals(a, b);
        }
    }
}

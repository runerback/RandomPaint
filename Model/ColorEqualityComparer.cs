using System;
using System.Collections.Generic;

namespace RandomPaint
{
    public sealed class ColorEqualityComparer : IEqualityComparer<IColor>
    {
        bool IEqualityComparer<IColor>.Equals(IColor x, IColor y)
        {
            return x == null ? y == null :
                x.R == y.R &&
                x.G == y.G &&
                x.B == y.B;
        }

        int IEqualityComparer<IColor>.GetHashCode(IColor obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}

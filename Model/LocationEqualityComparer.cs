using System.Collections.Generic;

namespace RandomPaint
{
    public sealed class LocationEqualityComparer : IEqualityComparer<ILocation>
    {
        bool IEqualityComparer<ILocation>.Equals(ILocation x, ILocation y)
        {
            return x == null ? y == null :
                x.X == y.X && x.Y == y.Y;
        }

        int IEqualityComparer<ILocation>.GetHashCode(ILocation obj)
        {
            if (obj == null)
                return 0;

            return obj.X.GetHashCode() ^ obj.Y.GetHashCode();
        }
    }
}

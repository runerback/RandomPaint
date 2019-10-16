namespace RandomPaint
{
    sealed class Location : ILocation
    {
        public Location(int x, int y)
        {
            X = x < 0 ? 0 : (uint)x;
            Y = y < 0 ? 0 : (uint)y;
        }

        public uint X { get; }
        public uint Y { get; }
    }
}

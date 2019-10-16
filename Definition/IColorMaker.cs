namespace RandomPaint
{
    public interface IColorMaker
    {
        IColorValueMaker R { get; }
        IColorValueMaker G { get; }
        IColorValueMaker B { get; }

        IColor Make(ILocation location);
        IColor Make(ILocation location, IColorMaker previous);
    }
}

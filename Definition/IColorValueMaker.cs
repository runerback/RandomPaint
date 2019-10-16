namespace RandomPaint
{
    public interface IColorValueMaker
    {
        byte Make(ILocation location);
        byte Make(ILocation location, IColorValueMaker previous);
    }
}

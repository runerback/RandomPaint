namespace RandomPaint
{
    public interface IPaintBoard
    {
        uint Width { get; }
        uint Height { get; }

        IPainter Painter { get; }

        ILocation[] Locations { get; }

        void Paint(IColorMaker colorMaker);

        IColor[,] TakeSnapshot();
    }
}

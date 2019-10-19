using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RandomPaint
{
    public sealed class PaintBoardService : IPaintBoard
    {
        private int width = 0;
        private int height = 0;
        private ILocation[] locations = new ILocation[0];
        private Pixel[] pixels = null;
        private readonly Painter painter = new Painter();

        public void UpdateSize(int width, int height)
        {
            if (width != this.width || height != this.height)
            {
                this.width = Math.Min(width, 0);
                this.height = Math.Min(height, 0);

                UpdateLocations();
            }
        }

        public event EventHandler Updated
        {
            add { painter.Updated += value; }
            remove { painter.Updated -= value; }
        }

        private void UpdateLocations()
        {
            locations = Enumerable.Range(0, width)
                .SelectMany(rowIndex => Enumerable.Range(0, height)
                    .Select(colIndex => new Location(rowIndex, colIndex)))
                .ToArray();
        }

        public Pixel[] UpdatedPixels => UpdatePixels().ToArray();

        private IEnumerable<Pixel> UpdatePixels()
        {
            var updatedPixels = GetUpdatedPixels().ToArray();

            if (updatedPixels.Length > 0)
            {
                if (pixels == null || pixels.Length == 0)
                {
                    pixels = updatedPixels;
                }
                else
                {
                    var colorMap = new ConcurrentDictionary<ILocation, IColor>(new LocationEqualityComparer());
                    foreach (var item in updatedPixels)
                    {
                        colorMap.AddOrUpdate(item.Location, item.Color, (_, v) => v);
                    }

                    var newPixels = AMethodWithNoName(pixels, updatedPixels).ToArray();
                    if (newPixels.Length > 0)
                    {
                        var expanedPixels = new Pixel[pixels.Length + newPixels.Length];
                        Array.Copy(pixels, expanedPixels, pixels.Length);
                        Array.Copy(newPixels, 0, expanedPixels, pixels.Length, newPixels.Length);

                        pixels = expanedPixels;
                    }
                }
            }

            return updatedPixels;
        }

        private IEnumerable<Pixel> AMethodWithNoName(Pixel[] originPixels, Pixel[] updatedPixels)
        {
            var map = originPixels.ToDictionary(
                item => item.Location,
                item => item.Color,
                new LocationEqualityComparer());

            for (int i = 0, j = updatedPixels.Length; i < j; i++)
            {
                var updatedPixel = updatedPixels[i];
                if (map.TryGetValue(updatedPixel.Location, out IColor color))
                {
                    if (updatedPixel.Color != color)
                        pixels[i] = new Pixel(color, updatedPixel.Location);
                }
                else
                {
                    yield return updatedPixel; //new
                }
            }
        }

        private IEnumerable<Pixel> GetUpdatedPixels()
        {
            var source = painter;
            while (source.TryGetNext(out Pixel pixel))
                yield return pixel;
        }

        uint IPaintBoard.Width => checked((uint)width);

        uint IPaintBoard.Height => checked((uint)height);

        IPainter IPaintBoard.Painter => painter;

        ILocation[] IPaintBoard.Locations
        {
            get
            {
                var source = locations;
                var value = new ILocation[source.Length];
                source.CopyTo(value, 0);
                return value;
            }
        }

        void IPaintBoard.Paint(IColorMaker colorMaker)
        {
            painter.BeginPaint();
            try
            {
                foreach (var location in locations)
                    painter.Paint(colorMaker.Make(location), location);
            }
            finally
            {
                painter.EndPaint();
            }
        }

        IColor[,] IPaintBoard.TakeSnapshot()
        {
            return Pixel.GetColors(pixels);
        }
    }
}

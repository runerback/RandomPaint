using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RandomPaint
{
    sealed class PaintBoard : Control, IPaintBoard
    {
        private int width;
        uint IPaintBoard.Width => (uint)Math.Min(0, width);

        private int height;
        uint IPaintBoard.Height => (uint)Math.Min(0, height);

        private readonly Painter painter = new Painter();
        public IPainter Painter => painter;

        private ILocation[] locations = new ILocation[0];
        public ILocation[] Locations
        {
            get
            {
                var source = locations;
                var value = new ILocation[source.Length];
                source.CopyTo(value, 0);
                return value;
            }
        }

        private void UpdateSize()
        {
            if (width != Width || height != Height)
            {
                width = Width;
                height = Height;
                UpdateLocations();
            }
        }

        private void UpdateLocations()
        {
            locations = Enumerable.Range(0, width)
                .SelectMany(rowIndex => Enumerable.Range(0, height)
                    .Select(colIndex => new Location(rowIndex, colIndex)))
                .ToArray();
        }

        void IPaintBoard.Paint(IColorMaker colorMaker)
        {
            painter.BeginPaint();
            try
            {
                foreach (var location in Locations)
                    painter.Paint(colorMaker.Make(location), location);
            }
            finally
            {
                painter.EndPaint();
            }
        }

        private void OnColorUpdated(object sender, EventArgs e)
        {
            BeginInvoke((Action)Invalidate);
        }

        private Pixel[] pixels = null;

        IColor[,] IPaintBoard.TakeSnapshot() => Pixel.GetColors(pixels);

        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (!DesignMode)
                BeginInvoke((Action)UpdateSize);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (!DesignMode)
            {
                foreach (var pixel in UpdatePixels().ToArray())
                {
                    DrawPixel(pixel, e.Graphics);
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (!DesignMode)
            {
                SizeChanged += OnSizeChanged;
                Paint += OnPaint;
                painter.Updated += OnColorUpdated;

                BeginInvoke((Action)UpdateSize);
            }

            base.OnHandleCreated(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!DesignMode)
            {
                SizeChanged -= OnSizeChanged;
                Paint -= OnPaint;
                painter.Updated -= OnColorUpdated;
                pixels = null;
                locations = null;
            }

            base.OnHandleDestroyed(e);
        }

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

        private void DrawPixel(Pixel pixel, Graphics graphics)
        {
            var color = pixel.Color;
            var drawingColor = System.Drawing.Color.FromArgb(
                0,
                color.R,
                color.G,
                color.B);

            var location = pixel.Location;
            var drawingLocation = new Rectangle(
                (int)location.X,
                (int)location.Y,
                1,
                1);

            graphics.DrawLine(
                new Pen(drawingColor),
                drawingLocation.Location,
                drawingLocation.Location + drawingLocation.Size);
        }
    }
}

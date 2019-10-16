using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RandomPaint
{
    sealed class Painter : IPainter
    {
        private readonly ConcurrentBag<Pixel> updatedPixels =
            new ConcurrentBag<Pixel>();

        public void Paint(IColor color, ILocation location)
        {
            if (color == null || location == null)
                return;

            updatedPixels.Add(new Pixel(color, location));

            Task.Run(() => Updated?.Invoke(this, EventArgs.Empty));
        }

        public bool TryGetNext(out Pixel pixel)
        {
            return updatedPixels.TryTake(out pixel);
        }

        public event EventHandler Updated;
    }
}

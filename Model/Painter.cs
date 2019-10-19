using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RandomPaint
{
    public sealed class Painter : IPainter
    {
        private readonly ConcurrentBag<Pixel> updatedPixels =
            new ConcurrentBag<Pixel>();

        public void Paint(IColor color, ILocation location)
        {
            if (color == null || location == null)
                return;

            updatedPixels.Add(new Pixel(color, location));

            if (notifyUpdated)
                Task.Run(() => Updated?.Invoke(this, EventArgs.Empty));
        }

        public bool TryGetNext(out Pixel pixel)
        {
            return updatedPixels.TryTake(out pixel);
        }

        private bool notifyUpdated = true;

        public event EventHandler Updated;

        public void BeginPaint()
        {
            notifyUpdated = false;
        }

        public void EndPaint()
        {
            notifyUpdated = true;

            Task.Run(() => Updated?.Invoke(this, EventArgs.Empty));
        }
    }
}

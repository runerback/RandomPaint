using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RandomPaint
{
    sealed class PaintBoardControl : Control
    {
        private readonly PaintBoardService service;

        public PaintBoardControl()
        {
            if (!DesignMode)
            {
                service = new PaintBoardService();
                service.Updated += OnColorUpdated;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (!DesignMode)
            {
                SizeChanged += OnSizeChanged;
                Paint += OnPaint;

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
                service.Updated -= OnColorUpdated;
                service.UpdateSize(0, 0);
            }

            base.OnHandleDestroyed(e);
        }
        
        private void UpdateSize()
        {
            service.UpdateSize(Width, Height);
        }
        
        private void OnColorUpdated(object sender, EventArgs e)
        {
            BeginInvoke((Action)Invalidate);
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (!DesignMode)
                BeginInvoke((Action)UpdateSize);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (!DesignMode)
            {
                foreach (var pixel in service.UpdatedPixels)
                {
                    DrawPixel(pixel, e.Graphics);
                }
            }
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

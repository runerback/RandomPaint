using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomPaint
{
    sealed class PaintBoard : Control, IPaintBoard
    {
        public PaintBoard()
        {
            if (DesignMode)
                return;

            //painter.Updated += delegate
            //{
            //    Invoke((Action)Invalidate);
            //};
        }

        uint IPaintBoard.Width => throw new NotImplementedException();

        uint IPaintBoard.Height => throw new NotImplementedException();

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

        void IPaintBoard.Paint(IColorMaker colorMaker)
        {
            foreach (var location in Locations)
                painter.Paint(colorMaker.Make(location), location);
            Invalidate();
        }

        public IColor[,] TakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}

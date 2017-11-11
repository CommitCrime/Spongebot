using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpongeBot.CoordinateProvider
{
    class LineByLine : IEnumerator
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const double DEFAULT_STEPSIZE = 20;
        private Rect area;
        private double step;

        public Point Current;
        
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public LineByLine(Rect area) : this(area, DEFAULT_STEPSIZE) { }
        public LineByLine(double step) : this(new Rect(0, 0, Utility.UI.getActualPrimaryScreenWidth(), Utility.UI.getActualPrimaryScreenHeight()), step) { }
        public LineByLine(Rect area, double step)
        {
            this.area = area;
            this.step = step;

            Current = area.TopLeft;
        }

        public void Dispose()
        {
            // empty
        }

        public bool MoveNext()
        {
            Current.X += step;
            if(Current.X > area.Right)
            {
                Current.X = area.Left;
                Current.Y += step;

                if (Current.Y > area.Bottom)
                    return false;
            }

            return true;
        }

        public void Reset()
        {
            Current = area.TopLeft;
        }
    }
}

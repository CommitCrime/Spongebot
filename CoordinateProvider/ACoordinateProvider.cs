using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;

namespace SpongeBot.CoordinateProvider
{
    public abstract class ACoordinateProvider : IEnumerator<System.Windows.Point>
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public System.Windows.Rect Area
        {
            get;
        }

        public double Step
        {
            get;
        }

        public Point Current;
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        Point IEnumerator<Point>.Current
        {
            get
            {
                return Current;
            }
        }

        public ACoordinateProvider(Rect area, double step)
        {
            this.Area = area;
            this.Step = step;
        }

        public abstract void Dispose();
        public abstract bool MoveNext();
        public abstract void Reset();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Collections;

namespace SpongeBot.CoordinateProvider
{
    class ArchimedeanSpiral : IEnumerator<Point>
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const double DEFAULT_STEPSIZE = 10;
        private Rect area;
        private double step;

        public Point Current;
        private Point spiralCenter;

        private double theta, thetaMax;
        private double awayStep;

        private bool initialized = false;
        private bool equidistant = false;

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

        public ArchimedeanSpiral(Rect area) : this(area, DEFAULT_STEPSIZE) { }
        public ArchimedeanSpiral(double step) : this(new Rect(0, 0, Utility.UI.getActualPrimaryScreenWidth(), Utility.UI.getActualPrimaryScreenHeight()), step) { }
        public ArchimedeanSpiral(Rect area, double step)
        {
            this.area = area;
            this.step = step;
            double coilGap = step; //gap bewteen coils 

            spiralCenter = new Point(area.X + area.Width/2, area.Y + area.Height/2);

            // find distance to nearest boundry:
            double maxRadius = new[] { spiralCenter.X, spiralCenter.Y, area.Width - spiralCenter.X, area.Height - spiralCenter.Y }.Min();
            // and calc number of coils to reach boundry
            double numberOfCoils = Math.Ceiling(maxRadius / coilGap);

            thetaMax = numberOfCoils * 2 * Math.PI; // radiant for numberOfCoils (1 coil = 2pi, 3 coils =3+2pi ...)
            awayStep = maxRadius / thetaMax;
        }

        public void Dispose()
        {
            // empty
        }

        public bool MoveNext()
        {
            if (!initialized)
            {
                // initial position
                Current = spiralCenter;
                theta = step / awayStep;
                initialized = true;
                return true;
            }
            else if (theta <= thetaMax)
            {
                double away = awayStep * theta;

                double x = spiralCenter.X + Math.Cos(theta) * away;
                // double biasedX = spiralCenter.X + Math.Cos(around) * away * 16 / 9;
                double y = spiralCenter.Y + Math.Sin(theta) * away;

                if (equidistant)
                    theta += step / away;
                else
                    theta += step * Math.PI / 180; // equiangular

                Current = new Point(x, y);
                return true;
            }

            return false;
        }

        public void Reset()
        {
            initialized = false;
            // will reset Current to spiralCenter and theta to 0
        }
    }
}

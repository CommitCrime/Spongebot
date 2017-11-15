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
    class ArchimedeanSpiral : ACoordinateProvider
    {
        public const double DEFAULT_STEPSIZE  = 10; //degres or arclength
        private Point spiralCenter;

        private double theta, thetaMax;
        private double awayStep;

        private bool initialized = false;
        public bool Equidistant { get; }

        public ArchimedeanSpiral(Rect area, double step = DEFAULT_STEPSIZE, bool equidistant = true) : base(area, step)
        {
            this.Equidistant = equidistant;
        }

        private void init()
        {
            double coilGap = Step; //gap bewteen coils 

            spiralCenter = new Point(Area.X + Area.Width / 2, Area.Y + Area.Height / 2);

            // find distance to nearest boundry:
            double maxRadius = new[] { spiralCenter.X, spiralCenter.Y, Area.Width - spiralCenter.X, Area.Height - spiralCenter.Y }.Min();
            // and calc number of coils to reach boundry
            double numberOfCoils = Math.Ceiling(maxRadius / coilGap);

            thetaMax = numberOfCoils * 2 * Math.PI; // radiant for numberOfCoils (1 coil = 2pi, 3 coils =3+2pi ...)
            awayStep = maxRadius / thetaMax;

            // initial position
            Current = spiralCenter;
            theta = Step / awayStep;

            initialized = true;
        }

        public override void Dispose()
        {
            // empty
        }

        public override bool MoveNext()
        {
            if (!initialized)
            {
                init();
                return true;
            }
            else if (theta <= thetaMax)
            {
                double away = awayStep * theta;

                double x = spiralCenter.X + Math.Cos(theta) * away;
                // double biasedX = spiralCenter.X + Math.Cos(around) * away * 16 / 9;
                double y = spiralCenter.Y + Math.Sin(theta) * away;

                if (Equidistant)
                    theta += Step / away;
                else
                    theta += Step * Math.PI / 180; // equiangular

                Current = new Point(x, y);
                return true;
            }

            return false;
        }

        public override void Reset()
        {
            initialized = false;
            // will reset Current to spiralCenter and theta to 0
        }
    }
}

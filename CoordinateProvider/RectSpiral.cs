using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpongeBot.CoordinateProvider
{
    class RectSpiral : IEnumerator<Point>
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const double DEFAULT_STEPSIZE = 20;
        private Rect area;
        private double step;
        private Queue<Queue<Point>> directionQueue;
        public Point Current;
        Queue<Point> currentQueue = new Queue<Point>();

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

        public RectSpiral(Rect area) : this(area, DEFAULT_STEPSIZE) { }
        public RectSpiral(double step) : this(new Rect(0, 0, Utility.UI.getActualPrimaryScreenWidth(), Utility.UI.getActualPrimaryScreenHeight()), step) { }
        public RectSpiral(Rect area, double step)
        {
            this.area = area;
            this.step = step;

            Current = new Point(area.X + area.Width / 2, area.Y + area.Height / 2);

            directionQueue = new Queue<Queue<Point>>();

            Queue<Point> right = new Queue<Point>();
            right.Enqueue(new Point(step, 0));
            directionQueue.Enqueue(right);

            Queue<Point> down = new Queue<Point>();
            down.Enqueue(new Point(0, -step));
            directionQueue.Enqueue(down);

            Queue<Point> left = new Queue<Point>();
            left.Enqueue(new Point(-step, 0));
            left.Enqueue(new Point(-step, 0));
            directionQueue.Enqueue(left);

            Queue<Point> up = new Queue<Point>();
            up.Enqueue(new Point(0, step));
            up.Enqueue(new Point(0, step));
            directionQueue.Enqueue(up);
        }

        public void Dispose()
        {
            // empty
        }

        public bool MoveNext()
        {
            if(currentQueue.Count == 0)
            {
                if (directionQueue.Count == 0)
                    return false;

                currentQueue = directionQueue.Dequeue();

                Queue<Point> append = new Queue<Point>(currentQueue);
                append.Enqueue(currentQueue.Peek()); 
                append.Enqueue(currentQueue.Peek());
                directionQueue.Enqueue(append);
            }

            Current.X = Current.X + currentQueue.Peek().X;
            Current.Y = Current.Y + currentQueue.Peek().Y;

            if(!area.Contains(Current))
                return false;

            currentQueue.Dequeue();

            return true;
        }

        public void Reset()
        {
            Current = area.TopLeft;
        }
    }
}

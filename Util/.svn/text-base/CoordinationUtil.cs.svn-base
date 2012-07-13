using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Butterfly.Util
{
    class CoordinationUtil
    {
        internal static Point GetPointInFront(Point point, int rot)
        {
            Point Sq = new Point(point.X, point.Y);

            if (rot == 0)
                Sq.Y--;
            else if (rot == 1)
            {
                Sq.X--;
                Sq.Y--;
            }
            else if (rot == 2)
            {
                Sq.X++;
            }
            else if (rot == 3)
            {
                Sq.X--;
                Sq.Y++;
            }
            else if (rot == 4)
            {
                Sq.Y++;
            }
            else if (rot == 5)
            {
                Sq.X++;
                Sq.Y++;
            }
            else if (rot == 6)
            {
                Sq.X--;
            }
            else if (rot == 7)
            {
                Sq.X++;
                Sq.Y--;
            }

            return Sq;
        }

        internal static Point GetPointBehind(Point point, int rot)
        {
            return GetPointInFront(point, RotationIverse(rot));
        }

        internal static int RotationIverse(int rot)
        {
            if (rot > 3)
                rot = rot - 4;
            else
                rot = rot + 4;

            return rot;
        }

        internal static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}

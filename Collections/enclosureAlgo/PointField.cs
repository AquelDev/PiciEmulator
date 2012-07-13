using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Pici.Collections.enclosureAlgo
{
    public class PointField
    {
        private List<Point> PointList;

        private static readonly Point badPoint = new Point(-1, -1);
        private Point mostLeft = badPoint;
        private Point mostTop = badPoint;
        private Point mostRight = badPoint;
        private Point mostDown = badPoint;
        public byte forValue { get; private set; }
        
        public PointField(byte forValue)
        {
            this.PointList = new List<Point>();
            this.forValue = forValue;
        }

        public List<Point> getPoints()
        {
            return this.PointList;
        }

        public void add(Point p)
        {
            if (mostLeft == badPoint)
                mostLeft = p;
            if(mostRight == badPoint)
                mostRight = p;
            if (mostTop == badPoint)
                mostTop = p;
            if (mostDown == badPoint)
                mostDown = p;

            if (p.X < mostLeft.X )
                mostLeft = p;
            if (p.X > mostRight.X)
                mostRight = p;
            if (p.Y > mostTop.Y)
                mostTop = p;
            if (p.Y < mostDown.Y)
                mostDown = p;


            PointList.Add(p);
        }
    }
}

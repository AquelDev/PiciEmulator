using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace testForm
{
    class TestWire : Button
    {
        public TestWire(int x, int y)
        {
            this.LocationPoint = new Point(x, y);
            this.Size = new Size(40, 40);
            this.Location = new Point(x * 40, y * 40);
            this.BackColor = Color.White;
        }
        private WiredSolver.WireCurrentTransfer transer = WiredSolver.WireCurrentTransfer.NONE;
        public Point LocationPoint { get; private set; }

        internal void updateWireState(WiredSolver.WireTransfer t)
        {
            this.transer = t.getTransfer();
            if (t.isPowered())
            {
                this.BackColor = Color.Red;

            }
            else if(t.getTransfer() != WiredSolver.WireCurrentTransfer.DOWN)
            {
                this.BackColor = Color.Black;
            }
            else 
                this.BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Graphics toPaintOn = pevent.Graphics;
            if(this.transer != WiredSolver.WireCurrentTransfer.NONE)
            {
                
                if ((this.transer & WiredSolver.WireCurrentTransfer.UP) == WiredSolver.WireCurrentTransfer.UP)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(20, 0));
                }
                if ((this.transer & WiredSolver.WireCurrentTransfer.DOWN) == WiredSolver.WireCurrentTransfer.DOWN)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(20, 40));
                }
                if ((this.transer & WiredSolver.WireCurrentTransfer.LEFT) == WiredSolver.WireCurrentTransfer.LEFT)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(0, 20));
                }
                if ((this.transer & WiredSolver.WireCurrentTransfer.RIGHT) == WiredSolver.WireCurrentTransfer.RIGHT)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(40, 20));
                }
            }
            toPaintOn.DrawString("" + this.LocationPoint.X + "," + this.LocationPoint.Y , new Font(new FontFamily("arial"), 6), Brushes.Green, new Point(3,3));
        }
    }
}

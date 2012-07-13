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
        private Pici.HabboHotel.Wired.WireCurrentTransfer transer = Pici.HabboHotel.Wired.WireCurrentTransfer.NONE;
        public Point LocationPoint { get; private set; }

        internal void updateWireState(Pici.HabboHotel.Wired.WireTransfer t)
        {
            this.transer = t.getTransfer();
            if (t.isPowered())
            {
                this.BackColor = Color.Red;

            }
            else if (t.getTransfer() != Pici.HabboHotel.Wired.WireCurrentTransfer.DOWN)
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
            if (this.transer != Pici.HabboHotel.Wired.WireCurrentTransfer.NONE)
            {

                if ((this.transer & Pici.HabboHotel.Wired.WireCurrentTransfer.UP) == Pici.HabboHotel.Wired.WireCurrentTransfer.UP)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(20, 0));
                }
                if ((this.transer & Pici.HabboHotel.Wired.WireCurrentTransfer.DOWN) == Pici.HabboHotel.Wired.WireCurrentTransfer.DOWN)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(20, 40));
                }
                if ((this.transer & Pici.HabboHotel.Wired.WireCurrentTransfer.LEFT) == Pici.HabboHotel.Wired.WireCurrentTransfer.LEFT)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(0, 20));
                }
                if ((this.transer & Pici.HabboHotel.Wired.WireCurrentTransfer.RIGHT) == Pici.HabboHotel.Wired.WireCurrentTransfer.RIGHT)
                {
                    toPaintOn.DrawLine(Pens.Green, new Point(20, 20), new Point(40, 20));
                }
            }
            toPaintOn.DrawString("" + this.LocationPoint.X + "," + this.LocationPoint.Y , new Font(new FontFamily("arial"), 6), Brushes.Green, new Point(3,3));
        }
    }
}

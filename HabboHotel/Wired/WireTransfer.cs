using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Pici.HabboHotel.Wired
{
    public class WireTransfer
    {
        public Point location { get; private set; }
        private WireCurrentTransfer currentTransfer;
        public CurrentType Current { get; private set; }

        public WireTransfer(int x, int y, WireCurrentTransfer currentTransfer)
        {
            this.location = new Point(x, y);
            this.currentTransfer = currentTransfer;
            this.Current = CurrentType.OFF;
        }

        public WireCurrentTransfer getTransfer()
        {
            return currentTransfer;
        }

        internal bool transfersCurrentTo(WireCurrentTransfer current)
        {
            return ((current & this.currentTransfer) == current);
        }

        public bool isPowered()
        {
            return this.Current == CurrentType.ON || this.Current == CurrentType.SENDER;
        }

        internal bool acceptsCurrentFrom(WireCurrentTransfer wireCurrentTransfer)
        {
            if (this.currentTransfer == WireCurrentTransfer.NONE)
                return true;

            else if (wireCurrentTransfer == WireCurrentTransfer.DOWN && this.transfersCurrentTo(WireCurrentTransfer.DOWN))
                return true;
            else if (wireCurrentTransfer == WireCurrentTransfer.UP && this.transfersCurrentTo(WireCurrentTransfer.UP))
                return true;
            else if (wireCurrentTransfer == WireCurrentTransfer.LEFT && this.transfersCurrentTo(WireCurrentTransfer.LEFT))
                return true;
            else if (wireCurrentTransfer == WireCurrentTransfer.RIGHT && this.transfersCurrentTo(WireCurrentTransfer.RIGHT))
                return true;
            else 
                return false;
        }

        internal void setCurrentTransfer(WireCurrentTransfer wireCurrentTransfer)
        {
            this.currentTransfer = wireCurrentTransfer;
        }

        internal void setCurrent(CurrentType currentType)
        {
            this.Current = currentType;
        }

        internal bool setPower(bool powerIsOn)
        {
            if (powerIsOn && this.Current == CurrentType.OFF)
            {
                this.Current = CurrentType.ON;
                return true;
            }
            else if(!powerIsOn && this.Current == CurrentType.ON)
            {
                    this.Current = CurrentType.OFF;
                    return true;
            }
            return false;
        }
        public override string ToString()
        {
            return string.Format("Location x:{0} y:{1}", this.location.X, this.location.Y);
        }
    }
}

using System.Collections.Generic;
using System.Drawing;

namespace Pici.HabboHotel.Wired
{
    public class WiredSolverInstance
    {
        private Dictionary<Point,WireTransfer> wiredField;

        public WiredSolverInstance()
        {
            this.wiredField = new Dictionary<Point,WireTransfer>();
        }

        public List<Point> AddOrUpdateWire(int x, int y, CurrentType newCurrent, WireCurrentTransfer currentTransfer)
        {
            LinkedList<WireTransfer> result = new LinkedList<WireTransfer>();
            WireTransfer updatedWire = getWireTransfer(new Point(x, y));
            if(newCurrent == CurrentType.ON)
            {
            }
            else if (newCurrent == CurrentType.SENDER)
            {
                currentTransfer = WireCurrentTransfer.DOWN | WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                updatedWire.setCurrentTransfer(currentTransfer);
                updatedWire.setCurrent(CurrentType.SENDER);
                updatePowerGrid(updatedWire, ref result);
            }
            else if (newCurrent == CurrentType.OFF)
            {
                WireTransfer[] neighbours = new WireTransfer[4];
                storeNeighbours(updatedWire, neighbours, true);
                updatedWire.setCurrent(CurrentType.OFF);
                updatedWire.setCurrentTransfer(currentTransfer);
                updatePowerGrid(updatedWire, ref result);
                for (int i = 0; i < 4; i++)
                {
                    if (neighbours[i] == null)
                        continue;
                    updatePowerGrid(neighbours[i], ref result);
                }
            }
            List<Point> finalResult = new List<Point>();
            foreach (WireTransfer t in result)
            {
                if (!finalResult.Contains(t.location))
                    finalResult.Add(t.location);
            }
            return finalResult;
        }

        public List<Point> RemoveWire(int x, int y)
        {
            LinkedList<WireTransfer> result = new LinkedList<WireTransfer>();
            WireTransfer updatedWire = getWireTransfer(new Point(x, y));
            result.AddFirst(updatedWire);

            if (updatedWire.isPowered() || updatedWire.getTransfer() != WireCurrentTransfer.NONE)
            {
                WireTransfer[] neighbours = new WireTransfer[4];
                storeNeighbours(updatedWire, neighbours, true);
                updatedWire.setCurrent(CurrentType.OFF);
                updatedWire.setCurrentTransfer(WireCurrentTransfer.NONE);
                for (int i = 0; i < 4; i++)
                {
                    if (neighbours[i] == null)
                        continue;
                    updatePowerGrid(neighbours[i], ref result);
                }
            }
            List<Point> finalResult = new List<Point>();
            foreach (WireTransfer t in result)
            {
                if (!finalResult.Contains(t.location))
                    finalResult.Add(t.location);
            }
            return finalResult;
        }

        private LinkedList<WireTransfer> updatePowerGrid(WireTransfer wireTransfer, ref LinkedList<WireTransfer> endResult)
        {
            LinkedList<WireTransfer> result = getLinkedWires(wireTransfer);

            bool powerIsOn = false;
            foreach (WireTransfer t in result)
            {
                if (t.Current == CurrentType.SENDER)
                {
                    powerIsOn = true;
                    break;
                }
            }
            if (endResult == null)
            {
                endResult = new LinkedList<WireTransfer>();
            }
            int x = 0;
            int y = 0;
            WireTransfer toTest;
            LinkedList<WireTransfer> testLater = new LinkedList<WireTransfer>();
            foreach (WireTransfer t in result)
            {
                if (!powerIsOn && t.getTransfer() == WireCurrentTransfer.NONE)
                {
                    testLater.AddFirst(t);
                    
                }
                else if (t.setPower(powerIsOn))
                {
                    endResult.AddFirst(t);
                }
            }
            foreach (WireTransfer t in testLater)
            {
                x = t.location.X;
                y = t.location.Y;
                toTest = getWireTransfer(new Point(x, y - 1));
                if (toTest.isPowered() && toTest.transfersCurrentTo(WireCurrentTransfer.DOWN))
                {
                    if (t.setPower(true))
                    {
                        endResult.AddFirst(t);
                    }
                    continue;
                }

                toTest = getWireTransfer(new Point(x, y + 1));
                if (toTest.isPowered() && toTest.transfersCurrentTo(WireCurrentTransfer.UP))
                {
                    if (t.setPower(true))
                    {
                        endResult.AddFirst(t);
                    }
                    continue;
                }

                toTest = getWireTransfer(new Point(x - 1, y));
                if (toTest.isPowered() && toTest.transfersCurrentTo(WireCurrentTransfer.RIGHT))
                {
                    if (t.setPower(true))
                    {
                        endResult.AddFirst(t);
                    }
                    continue;
                }

                toTest = getWireTransfer(new Point(x + 1, y));
                if (toTest.isPowered() && toTest.transfersCurrentTo(WireCurrentTransfer.LEFT))
                {
                    if (t.setPower(true))
                    {
                        endResult.AddFirst(t);
                    }
                    continue;
                }
                if (t.setPower(false))
                {
                    endResult.AddFirst(t);
                }
            }

            endResult.AddFirst(wireTransfer);
            return endResult;
        }

        private LinkedList<WireTransfer> getLinkedWires(WireTransfer originalSender)
        {
            List<Point> blockedList = new List<Point>(30);

            LinkedList<WireTransfer> openList = new LinkedList<WireTransfer>();
            LinkedList<WireTransfer> endResult = new LinkedList<WireTransfer>();
            WireTransfer toUpdate;
            WireTransfer[] neighbours = new WireTransfer[4];


            openList.AddFirst(originalSender);
            endResult.AddFirst(originalSender);
            blockedList.Add(originalSender.location);
            while (openList.Count > 0)
            {
                toUpdate = openList.First.Value;
                openList.RemoveFirst();

                storeNeighbours(toUpdate, neighbours);
                for (int i = 0; i < 4; i++)
                {
                    if (neighbours[i] == null)
                        continue;
                    if (blockedList.Contains(neighbours[i].location))
                        continue;
                         
                    blockedList.Add(neighbours[i].location);
                    openList.AddFirst(neighbours[i]);
                    endResult.AddFirst(neighbours[i]);
                }
            }

            return endResult;
        }

        private void storeNeighbours(WireTransfer toUpdate, WireTransfer[] inNeighbors, bool allneighbours = false)
        {
            int x = toUpdate.location.X;
            int y = toUpdate.location.Y;

            WireTransfer toTest;
            inNeighbors[0] = null;
            inNeighbors[1] = null;
            inNeighbors[2] = null;
            inNeighbors[3] = null;
            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.UP))
            {
                toTest = getWireTransfer(new Point(x, y - 1));
                if (allneighbours || toTest.acceptsCurrentFrom(WireCurrentTransfer.DOWN))
                    inNeighbors[0] = toTest;
            }

            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.LEFT))
            {
                toTest = getWireTransfer(new Point(x - 1, y));
                if (allneighbours || toTest.acceptsCurrentFrom(WireCurrentTransfer.RIGHT))
                    inNeighbors[1] = toTest;
            }

            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.RIGHT))
            {
                toTest = getWireTransfer(new Point(x + 1, y));
                if (allneighbours || toTest.acceptsCurrentFrom(WireCurrentTransfer.LEFT))
                    inNeighbors[2] = toTest;
            }

            if (allneighbours || toUpdate.transfersCurrentTo(WireCurrentTransfer.DOWN))
            {
                toTest = getWireTransfer(new Point(x, y + 1));
                if (allneighbours || toTest.acceptsCurrentFrom(WireCurrentTransfer.UP))
                    inNeighbors[3] = toTest;
            }
        }

        public WireTransfer getWireTransfer(Point location)
        {
            WireTransfer item = null;
          
            if (!this.wiredField.TryGetValue(location, out item))
            {
                item = new WireTransfer(location.X, location.Y, WireCurrentTransfer.NONE);
                this.wiredField.Add(item.location, item);
            }
            return item;
        }

        public void Destroy()
        {
            if (wiredField != null)
                wiredField.Clear();
            wiredField = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using Pici.Collections.Algorithm;

namespace Pici.Collections.enclosureAlgo
{
    public class GameField : IPathNode
    {

        private byte[,] currentField;
        private AStarSolver<GameField> astarSolver;
        private Queue<GametileUpdate> newEntries = new Queue<GametileUpdate>();
        private bool diagonal;
        private GametileUpdate currentlyChecking;

        public GameField(byte[,] theArray, bool diagonalAllowed)
        {
            this.currentField = theArray;
            this.diagonal = diagonalAllowed;
            this.astarSolver = new AStarSolver<GameField>(diagonalAllowed, AStarHeuristicType.EXPERIMENTAL_SEARCH, this, theArray.GetUpperBound(1) + 1, theArray.GetUpperBound(0) + 1);
        }

        public void updateLocation(int x, int y, byte value)
        {
            newEntries.Enqueue(new GametileUpdate(x,y,value));
        }

        public List<PointField> doUpdate(bool oneloop = false)
        {
            List<PointField> returnList = new List<PointField>();
            while (newEntries.Count > 0)
            {
                currentlyChecking = newEntries.Dequeue();

                List<Point> pointList = getConnectedItems(currentlyChecking);
                if (pointList.Count > 1)
                {
                    List<LinkedList<AStarSolver<GameField>.PathNode>> RouteList = handleListOfConnectedPoints(pointList, currentlyChecking);

                    foreach (LinkedList<AStarSolver<GameField>.PathNode> nodeList in RouteList)
                    {
                        if (nodeList.Count >= 4)
                        {
                            PointField field = findClosed(nodeList);
                            if (field != null)
                            {
                                returnList.Add(field);
                            }
                        }
                    }
                }
                this.currentField[currentlyChecking.y, currentlyChecking.x] = currentlyChecking.value;
            }
            return returnList;
        }

        private PointField findClosed(LinkedList<AStarSolver<GameField>.PathNode> nodeList)
        {
            PointField returnList = new PointField(currentlyChecking.value);

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (AStarSolver<GameField>.PathNode node in nodeList)
            {
                if (node.X < minX)
                    minX = node.X;

                if(node.X > maxX)
                    maxX = node.X;

                if (node.Y < minY)
                    minY = node.Y;

                if (node.Y > maxY)
                    maxY = node.Y;

            }

            int middleX = (int)Math.Ceiling(((maxX - minX) / 2f)) + minX;
            int middleY = (int)Math.Ceiling(((maxY - minY) / 2f)) + minY;
            //Console.WriteLine("Middle: x:[{0}]  y:[{1}]", middleX, middleY);

            Point current;
            List<Point> toFill = new List<Point>();
            List<Point> checkedItems = new List<Point>();
            checkedItems.Add(new Point(currentlyChecking.x, currentlyChecking.y));
            Point toAdd;
            toFill.Add(new Point(middleX,middleY));
            int x;
            int y;
            while(toFill.Count > 0)
            {
                current = toFill[0];
                x = current.X;
                y = current.Y;
               
                if (x < minX)
                    return null;//OOB
                if (x > maxX)
                    return null;//OOB
                if (y < minY)
                    return null;//OOB
                if (y > maxY)
                    return null; //OOB
                
                if (this[y - 1, x] && currentField[y - 1, x] == 0)
                {
                    toAdd = new Point(x, y - 1);
                    if (!toFill.Contains(toAdd) && !checkedItems.Contains(toAdd))
                        toFill.Add(toAdd);
                }
                if (this[y + 1, x] && currentField[y + 1, x] == 0 )
                {
                    toAdd = new Point(x, y + 1);
                    if (!toFill.Contains(toAdd) && !checkedItems.Contains(toAdd))
                        toFill.Add(toAdd);
                }
                if (this[y, x - 1] && currentField[y, x - 1] == 0)
                {
                    toAdd = new Point(x - 1, y);
                    if (!toFill.Contains(toAdd) && !checkedItems.Contains(toAdd))
                        toFill.Add(toAdd);
                }
                if (this[y, x + 1] && currentField[y, x + 1] == 0)
                {
                    toAdd = new Point(x + 1, y);
                    if (!toFill.Contains(toAdd) && !checkedItems.Contains(toAdd))
                        toFill.Add(toAdd);
                }
                if (getValue(current) == 0)
                    returnList.add(current);
                checkedItems.Add(current);
                toFill.RemoveAt(0);
                

            }

            return returnList;
        }

        private List<LinkedList<AStarSolver<GameField>.PathNode>> handleListOfConnectedPoints(List<Point> pointList, GametileUpdate update)
        {
            List<LinkedList<AStarSolver<GameField>.PathNode>> returnList = new List<LinkedList<AStarSolver<GameField>.PathNode>>();
            int amount = 0;
            foreach (Point begin in pointList)
            {
                amount++;
                if (amount == pointList.Count / 2 + 1)
                    return returnList;
                foreach (Point end in pointList)
                {
                    if (begin == end)
                        continue;
                    LinkedList<AStarSolver<GameField>.PathNode> list = astarSolver.Search(end, begin);
                    if (list != null)
                    {
                        returnList.Add(list);
                    }
                }
            }
            return returnList;
        }

        private List<Point> getConnectedItems(GametileUpdate update)
        {
            List<Point> ConnectedItems = new List<Point>();
            int x = update.x;
            int y = update.y;
            if (diagonal)
            {
                if (this[y - 1, x - 1] && currentField[y - 1, x - 1] == update.value)
                {
                    ConnectedItems.Add(new Point(x - 1, y - 1));
                }
                if (this[y - 1, x + 1] && currentField[y - 1, x + 1] == update.value)
                {
                    ConnectedItems.Add(new Point(x + 1, y - 1));
                }
                if (this[y + 1, x - 1] && currentField[y + 1, x - 1] == update.value)
                {
                    ConnectedItems.Add(new Point(x - 1, y + 1));
                }
                if (this[y + 1, x + 1] && currentField[y + 1, x + 1] == update.value)
                {
                    ConnectedItems.Add(new Point(x + 1, y + 1));
                }
            }


            if (this[y - 1, x] && currentField[y - 1, x] == update.value)
            {
                ConnectedItems.Add(new Point(x, y - 1));
            }
            if (this[y + 1, x] && currentField[y + 1, x] == update.value)
            {
                ConnectedItems.Add(new Point(x, y + 1));
            }
            if (this[y, x - 1] && currentField[y, x - 1] == update.value)
            {
                ConnectedItems.Add(new Point(x - 1, y));
            }
            if (this[y, x + 1] && currentField[y, x + 1] == update.value)
            {
                ConnectedItems.Add(new Point(x + 1, y));
            }

            return ConnectedItems;
        }

        public bool this[int y, int x]
        {
            get
            {
                if (y < 0 || x < 0)
                    return false;
                else if (y > this.currentField.GetUpperBound(0) || x > this.currentField.GetUpperBound(1))
                    return false;
                return true;
            }

        }

        private void setValue(int x, int y, byte value)
        {
            if (this[y, x])
            {
                currentField[y, x] = value;
            }
        }

        public byte getValue(int x, int y)
        {
            if (this[y, x])
            {
                return currentField[y, x];
            }
            return 0;
        }

        public byte getValue(Point p)
        {
            if (this[p.Y, p.X])
            {
                return currentField[p.Y, p.X];
            }
            return 0;
        }

        public bool IsBlocked(int x, int y, bool lastTile)
        {
            if (this.currentlyChecking.x == x && this.currentlyChecking.y == y)
                return true;
            return !(getValue(x, y) == this.currentlyChecking.value);
        }

        public void destroy()
        {
            this.currentField = null;
        }
    }
}
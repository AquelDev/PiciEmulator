﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Games;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.Messages;
using Pici.HabboHotel.Wired;
using testForm;
using System.Windows.Forms;
using System.Threading;
using System;

namespace Pici.HabboHotel.Rooms.Wired
{
    public class WiredHandler
    {
        #region Declares
        private Hashtable actionItems; //interactionType, List<RoomItem>
        private Hashtable actionStacks; //point, List<RoomItem>

        private Queue requestedTriggers;
        private Queue requestingUpdates;

        private WiredSolverInstance wireSlower;

        private Room room;
        internal ConditionHandler conditionHandler;
        private bool doCleanup = false;
        //private Form1 form;
        #endregion

        #region Constructor
        public WiredHandler(Room room)
        {
            this.actionItems = new Hashtable();
            this.actionStacks = new Hashtable();
            this.requestedTriggers = new Queue();
            this.requestingUpdates = new Queue();
            this.wireSlower = new WiredSolverInstance();

            this.room = room;
            this.conditionHandler = new ConditionHandler(room);
            //form = new Form1();
            ////
            //Thread thread = new Thread(new ThreadStart(Init));
            //thread.Start();
        }

        //private void Init()
        //{
        //    Application.Run(form);
        //}
        #endregion

        #region Furniture add/remove
        internal void AddFurniture(RoomItem item)
        {
            AddFurnitureToItems(item);
            AddFurnitureToItemStack(item);
        }

        internal void RemoveFurniture(RoomItem item)
        {
            RemoveFurnitureFromItems(item);
            RemoveFurnitureFromStack(item);
        }

        private void AddFurnitureToItems(RoomItem item)
        {
            InteractionType type = item.GetBaseItem().InteractionType;
            if (!WiredUtillity.TypeIsWired(type))
                return;

            if (actionItems.ContainsKey(type))
                ((List<RoomItem>)actionItems[type]).Add(item);
            else
            {
                List<RoomItem> stack = new List<RoomItem>();
                stack.Add(item);

                actionItems.Add(type, stack);
            }

        }

        private void RemoveFurnitureFromItems(RoomItem item)
        {
            InteractionType type = item.GetBaseItem().InteractionType;
            if (actionItems.ContainsKey(type))
                ((List<RoomItem>)actionItems[type]).Remove(item);
        }

        private void AddFurnitureToItemStack(RoomItem item)
        {
            Point itemCoord = item.Coordinate;

            if (actionStacks.ContainsKey(itemCoord))
                ((List<RoomItem>)actionStacks[itemCoord]).Add(item);
            else
            {
                List<RoomItem> stack = new List<RoomItem>();
                stack.Add(item);

                actionStacks.Add(itemCoord, stack);
            }
        }

        private void RemoveFurnitureFromStack(RoomItem item)
        {
            Point itemCoord = item.Coordinate;
            if (actionStacks.ContainsKey(itemCoord))
                ((List<RoomItem>)actionStacks[itemCoord]).Remove(item);
        }
        #endregion

        #region Room cycle management
        internal void OnCycle()
        {
            if (doCleanup)
            {
                foreach (List<RoomItem> list in actionStacks.Values)
                {
                    foreach (RoomItem item in list)
                    {
                        if (item.wiredCondition != null)
                        {
                            item.wiredCondition.Dispose();
                            item.wiredCondition = null;
                        }
                        if (item.wiredHandler != null)
                        {
                            item.wiredHandler.Dispose();
                            item.wiredHandler = null;
                        }
                    }
                }
                actionStacks.Clear();
                actionItems.Clear();
                requestedTriggers.Clear();
                requestingUpdates.Clear();

                doCleanup = false;
                return;
            }
            if (requestingUpdates.Count > 0)
            {
                List<IWiredCycleable> toAdd = new List<IWiredCycleable>();
                lock (requestingUpdates.SyncRoot)
                {
                    while (requestingUpdates.Count > 0)
                    {
                        IWiredCycleable handler = (IWiredCycleable)requestingUpdates.Dequeue();
                        if (handler.Disposed())
                        {
                            continue;
                        }

                        if (handler.OnCycle())
                        {
                            toAdd.Add(handler);
                        }
                    }

                    foreach (IWiredCycleable cycle in toAdd)
                    {
                        requestingUpdates.Enqueue(cycle);
                    }
                }
            }

            conditionHandler.OnCycle();
        }
        #endregion

        #region Functions
        internal void OnPickall()
        {
            doCleanup = true;
        }

        internal void OnEvent(uint itemID)
        {
            ServerMessage message = new ServerMessage(88);
            message.AppendStringWithBreak(itemID.ToString());
            message.AppendStringWithBreak("0");

            room.SendMessage(message);
        }
        #endregion

        #region Requests
        internal void RequestStackHandle(Point coordinate, RoomItem item, RoomUser user, Team team)
        {
            List<RoomItem> items = null;
            if (actionStacks.ContainsKey(coordinate) && conditionHandler.AllowsHandling(coordinate, user))
            {
                items = (List<RoomItem>)actionStacks[coordinate];

                foreach (RoomItem stackItem in items)
                {
                    if (stackItem.wiredHandler is IWiredEffect)
                    {
                        ((IWiredEffect)stackItem.wiredHandler).Handle(user, team, item);
                    }
                }

                bool shouldBeHandled = false;

                CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X, coordinate.Y + 1));
                CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X + 1, coordinate.Y));
                CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X, coordinate.Y - 1));
                CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X - 1, coordinate.Y));

                if (shouldBeHandled)
                {
                    room.GetWiredHandler().TriggerOnWire(coordinate);
                }
            }
        }

        private void CheckHandlingState(ref bool shouldBeHandled, Point coordinate)
        {
            foreach (RoomItem _item in room.GetGameMap().GetCoordinatedItems(coordinate))
            {
                if (WiredHandler.TypeIsWire(_item.GetBaseItem().InteractionType))
                    shouldBeHandled = true;
            }
        }

        internal void RequestCycle(IWiredCycleable handler)
        {
            lock (requestingUpdates.SyncRoot)
            {
                requestingUpdates.Enqueue(handler);
            }
        }

        #endregion

        #region Return values
        internal Room GetRoom()
        {
            return room;
        }

        #endregion

        #region Wires

        internal void AddWire(RoomItem item, Point location, int rotation, InteractionType wireType)
        {

            WireCurrentTransfer transfer = WireCurrentTransfer.NONE;

            switch (wireType)
            {
                case InteractionType.wireCenter:
                    {
                        transfer = WireCurrentTransfer.DOWN | WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                        break;
                    }
                case InteractionType.wireCorner:
                    {
                        switch (rotation)
                        {
                            default:
                                {
                                    transfer = WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT;
                                    break;
                                }
                            case 2:
                                {
                                    transfer = WireCurrentTransfer.DOWN | WireCurrentTransfer.LEFT;
                                    break;
                                }
                            case 4:
                                {
                                    transfer = WireCurrentTransfer.UP | WireCurrentTransfer.LEFT;
                                    break;
                                }
                            case 6:
                                {
                                    transfer = WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                                    break;
                                }
                        }

                        break;
                    }
                case InteractionType.wireSplitter:
                    {
                        switch (rotation)
                        {
                            default:
                                {
                                    transfer = WireCurrentTransfer.UP | WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT;
                                    break;
                                }
                            case 2:
                                {
                                    transfer = WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT | WireCurrentTransfer.DOWN;
                                    break;
                                }
                            case 4:
                                {
                                    transfer = WireCurrentTransfer.UP | WireCurrentTransfer.DOWN | WireCurrentTransfer.LEFT;
                                    break;
                                }
                            case 6:
                                {
                                    transfer = WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                                    break;
                                }
                        }
                        break;
                    }

                case InteractionType.wireStandard:
                    {
                        switch (rotation)
                        {
                            default:
                            case 0:
                            case 4:
                                {
                                    transfer = WireCurrentTransfer.UP | WireCurrentTransfer.DOWN;
                                    break;
                                }
                            case 2:
                            case 6:
                                {
                                    transfer = WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT;
                                    break;
                                }
                        }
                        break;
                    }
            }

            if (transfer == WireCurrentTransfer.NONE)
                return;

            List<Point> pointsToTrigger = wireSlower.AddOrUpdateWire(location.X, location.Y, CurrentType.OFF, transfer);
            //form.AddOrUpdateWire(location.X, location.Y, CurrentType.OFF, transfer);
            HandleItems(pointsToTrigger);
        }

        internal void RemoveWiredItem(Point coordinate)
        {
            //TriggerOnWire(coordinate);
            wireSlower.RemoveWire(coordinate.X, coordinate.Y);
            //form.Remove(coordinate.X, coordinate.Y);
        }

        internal static bool TypeIsWire(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.wireCenter:
                case InteractionType.wireCorner:
                case InteractionType.wireSplitter:
                case InteractionType.wireStandard:
                    {
                        return true;
                    }
                default:
                    return false;
            }
        }

        internal void TriggerOnWire(Point location)
        {
            WireTransfer transferItem = wireSlower.getWireTransfer(location);
            
            List<Point> pointsToTrigger;
            WireCurrentTransfer transferType = WireCurrentTransfer.DOWN | WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
            if (transferItem.isPowered() && transferItem.getTransfer() == transferType)
            {
                pointsToTrigger = wireSlower.RemoveWire(location.X, location.Y);
                //form.Remove(location.X, location.Y);
            }
            else
            {
                pointsToTrigger = wireSlower.AddOrUpdateWire(location.X, location.Y, CurrentType.SENDER, WireCurrentTransfer.NONE);
                //form.AddOrUpdateWire(location.X, location.Y, CurrentType.SENDER, WireCurrentTransfer.NONE);
            }
            
            HandleItems(pointsToTrigger, location);
        }

        private void HandleItems(List<Point> coords, Point startingPoint)
        {
            foreach (Point point in coords)
            {
                if (point != startingPoint)
                    HandleItems(point);
            }
        }

        private void HandleItems(List<Point> coords)
        {
            foreach (Point point in coords)
            {
                HandleItems(point);
            }
        }

        private void HandleItems(Point coord)
        {
            WireTransfer transfer = wireSlower.getWireTransfer(coord);
            List<RoomItem> items = new List<RoomItem>(room.GetGameMap().GetCoordinatedItems(coord));
            foreach (RoomItem item in items)
            {
                item.Interactor.OnTrigger(null, item, 0, true);
            }
            items.Clear();
            items = null;
        }

        #endregion

        #region Loading
        internal void LoadWired()
        {

        }
        #endregion

        #region Unloading
        internal void Destroy()
        {
            if (actionItems != null)
                actionItems.Clear();
            actionItems = null;
            if (actionStacks != null)
                actionStacks.Clear();
            requestedTriggers.Clear();
            requestingUpdates.Clear();
            if (wireSlower != null)
                wireSlower.Destroy();
            wireSlower = null;
            room = null;
        }
        #endregion
    }
}

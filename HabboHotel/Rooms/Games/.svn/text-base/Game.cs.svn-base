using Butterfly.Collections;
using Butterfly.HabboHotel.Items;
using System;

namespace Butterfly.HabboHotel.Rooms.Games
{
    class GameManager
    {
        internal int[] TeamPoints;

        internal event TeamScoreChangedDelegate OnScoreChanged;
        internal event RoomEventDelegate OnGameStart;
        internal event RoomEventDelegate OnGameEnd;

        private QueuedDictionary<uint, RoomItem> redTeamItems;
        private QueuedDictionary<uint, RoomItem> blueTeamItems;
        private QueuedDictionary<uint, RoomItem> greenTeamItems;
        private QueuedDictionary<uint, RoomItem> yellowTeamItems;
        private Room room;

        internal int[] Points
        {
            get
            {
                return TeamPoints;
            }
            set
            {
                TeamPoints = value;
            }
        }

        internal void OnCycle()
        {
            redTeamItems.OnCycle();
            blueTeamItems.OnCycle();
            greenTeamItems.OnCycle();
            yellowTeamItems.OnCycle();
        }

        internal QueuedDictionary<uint, RoomItem> GetItems(Team team)
        {
            switch (team)
            {
                case Team.blue:
                    return blueTeamItems;
                case Team.green:
                    return greenTeamItems;
                case Team.red:
                    return redTeamItems;
                case Team.yellow:
                    return yellowTeamItems;
                default:
                    return new QueuedDictionary<uint, RoomItem>();
            }
        }

        public GameManager(Room room)
        {
            this.TeamPoints = new int[5];

            this.redTeamItems = new QueuedDictionary<uint, RoomItem>();
            this.blueTeamItems = new QueuedDictionary<uint, RoomItem>();
            this.greenTeamItems = new QueuedDictionary<uint, RoomItem>();
            this.yellowTeamItems = new QueuedDictionary<uint, RoomItem>();
            this.room = room;
        }
        internal Team getWinningTeam()
        {
            int winning = 1;
            int highestScore = 0;
            for (int i = 1; i < 5; i++)
            {
                if (this.TeamPoints[i] > highestScore)
                {
                    highestScore = this.TeamPoints[i];
                    winning = i;
                }
            }
            return (Team)winning;
        }

        internal void AddPointToTeam(Team team, RoomUser user)
        {
            AddPointToTeam(team, 1, user);
        }

        internal void AddPointToTeam(Team team, int points, RoomUser user)
        {
            int newPoints = TeamPoints[(int)team] += points;

            if (newPoints < 0)
                newPoints = 0;
            TeamPoints[(int)team] = newPoints;

            if (OnScoreChanged != null)
                OnScoreChanged(null, new TeamScoreChangedArgs(newPoints, team, user));

            foreach (RoomItem item in GetFurniItems(team).Values)
            {
                if (!isSoccerGoal(item.GetBaseItem().InteractionType))
                {
                    item.ExtraData = TeamPoints[(int)team].ToString();
                    item.UpdateState();
                }
            }
        }

        internal void Reset()
        {
            AddPointToTeam(Team.blue, GetScoreForTeam(Team.blue) * (-1), null);
            AddPointToTeam(Team.green, GetScoreForTeam(Team.green) * (-1), null);
            AddPointToTeam(Team.red, GetScoreForTeam(Team.red) * (-1), null);
            AddPointToTeam(Team.yellow, GetScoreForTeam(Team.yellow) * (-1), null);
        }

        private int GetScoreForTeam(Team team)
        {
            return TeamPoints[(int)team];
        }

        private QueuedDictionary<uint, RoomItem> GetFurniItems(Team team)
        {
            switch (team)
            {
                case Team.blue:
                    {
                        return blueTeamItems;
                    }
                case Team.green:
                    {
                        return greenTeamItems;
                    }
                case Team.red:
                    {
                        return redTeamItems;
                    }
                case Team.yellow:
                    {
                        return yellowTeamItems;
                    }
            }
            return new QueuedDictionary<uint, RoomItem>();
        }

        private static bool isSoccerGoal(InteractionType type)
        {
            return (type == InteractionType.footballgoalblue || type == InteractionType.footballgoalgreen || type == InteractionType.footballgoalred || type == InteractionType.footballgoalyellow);
        }

        internal void AddFurnitureToTeam(RoomItem item, Team team)
        {
            switch (team)
            {
                case Team.blue:
                    blueTeamItems.Add(item.Id, item);
                    break;
                case Team.green:
                    greenTeamItems.Add(item.Id, item);
                    break;
                case Team.red:
                    redTeamItems.Add(item.Id, item);
                    break;
                case Team.yellow:
                    yellowTeamItems.Add(item.Id, item);
                    break;
            }
        }

        internal void RemoveFurnitureFromTeam(RoomItem item, Team team)
        {
            switch (team)
            {
                case Team.blue:
                    blueTeamItems.Remove(item.Id);
                    break;
                case Team.green:
                    greenTeamItems.Remove(item.Id);
                    break;
                case Team.red:
                    redTeamItems.Remove(item.Id);
                    break;
                case Team.yellow:
                    yellowTeamItems.Remove(item.Id);
                    break;
            }
        }

        internal RoomItem GetFirstScoreBoard(Team team)
        {
            switch (team)
            {
                case Team.blue:
                    {
                        foreach (RoomItem item in blueTeamItems.Values)
                        {
                            if (item.GetBaseItem().InteractionType == InteractionType.freezebluecounter)
                                return item;
                        }
                        break;
                    }
                case Team.green:
                    {
                        foreach (RoomItem item in greenTeamItems.Values)
                        {
                            if (item.GetBaseItem().InteractionType == InteractionType.freezegreencounter)
                                return item;
                        }
                        break;
                    }
                case Team.red:
                    {
                        foreach (RoomItem item in redTeamItems.Values)
                        {
                            if (item.GetBaseItem().InteractionType == InteractionType.freezeredcounter)
                                return item;
                        }
                        break;
                    }
                case Team.yellow:
                    {
                        foreach (RoomItem item in yellowTeamItems.Values)
                        {
                            if (item.GetBaseItem().InteractionType == InteractionType.freezeyellowcounter)
                                return item;
                        }
                        break;
                    }
            }
            return null;
        }

        internal void UnlockGates()
        {
            foreach (RoomItem item in redTeamItems.Values)
            {
                UnlockGate(item);
            }

            foreach (RoomItem item in greenTeamItems.Values)
            {
                UnlockGate(item);
            }

            foreach (RoomItem item in blueTeamItems.Values)
            {
                UnlockGate(item);
            }

            foreach (RoomItem item in yellowTeamItems.Values)
            {
                UnlockGate(item);
            }
        }

        private void LockGate(RoomItem item)
        {
            InteractionType type = item.GetBaseItem().InteractionType;
            if (type == InteractionType.freezebluegate || type == InteractionType.freezegreengate || type == InteractionType.freezeredgate || type == InteractionType.freezeyellowgate)
            {
                foreach (RoomUser user in room.GetGameMap().GetRoomUsers(new System.Drawing.Point(item.GetX, item.GetY)))
                {
                    user.SqState = 0;
                }

                room.GetGameMap().GameMap[item.GetX, item.GetY] = 0;
            }
        }

        private void UnlockGate(RoomItem item)
        {
            InteractionType type = item.GetBaseItem().InteractionType;
            if (type == InteractionType.freezebluegate || type == InteractionType.freezegreengate || type == InteractionType.freezeredgate || type == InteractionType.freezeyellowgate)
            {
                foreach (RoomUser user in room.GetGameMap().GetRoomUsers(new System.Drawing.Point(item.GetX, item.GetY)))
                {
                    user.SqState = 1;
                }

                room.GetGameMap().GameMap[item.GetX, item.GetY] = 1;
            }
        }

        internal void LockGates()
        {
            foreach (RoomItem item in redTeamItems.Values)
            {
                LockGate(item);
            }

            foreach (RoomItem item in greenTeamItems.Values)
            {
                LockGate(item);
            }

            foreach (RoomItem item in blueTeamItems.Values)
            {
                LockGate(item);
            }

            foreach (RoomItem item in yellowTeamItems.Values)
            {
                LockGate(item);
            }
        }

        internal void StopGame()
        {
            if (OnGameEnd != null)
                OnGameEnd(null, null);
            room.lastTimerReset = DateTime.Now;
        }

        internal void StartGame()
        {
            if (OnGameStart != null)
                OnGameStart(null, null);
        }

        internal Room GetRoom()
        {
            return room;
        }

        internal void Destroy()
        {
            Array.Clear(TeamPoints, 0, TeamPoints.Length);
            redTeamItems.Destroy();
            blueTeamItems.Destroy();
            greenTeamItems.Destroy();
            yellowTeamItems.Destroy();

            TeamPoints = null;
            OnScoreChanged = null;
            OnGameStart = null;
            OnGameEnd = null;
            redTeamItems = null;
            blueTeamItems = null;
            greenTeamItems = null;
            yellowTeamItems = null;
            room = null;
        }
    }
}

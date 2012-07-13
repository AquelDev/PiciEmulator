using System;
using System.Drawing;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    class MovementManagement
    {
        private Random rnd;

        public MovementManagement()
        {
            this.rnd = new Random();
        }

        private static void HandleMovement(ref Point coordinate, MovementState state)
        {
            switch (state)
            {
                case MovementState.down:
                    {
                        coordinate.Y++;
                        break;
                    }

                case MovementState.up:
                    {
                        coordinate.Y--;
                        break;
                    }

                case MovementState.left:
                    {
                        coordinate.X--;
                        break;
                    }

                case MovementState.right:
                    {
                        coordinate.X++;
                        break;
                    }
            }
        }

        protected Point HandleMovement(Point newCoordinate, MovementState state, int newRotation)
        {
            Point newPoint = new Point(newCoordinate.X, newCoordinate.Y);

            switch (state)
            {
                case MovementState.up:
                case MovementState.down:
                case MovementState.left:
                case MovementState.right:
                    {
                        HandleMovement(ref newPoint, state);
                        break;
                    }

                case MovementState.leftright:
                    {
                        if (rnd.Next(0, 2) == 1)
                        {
                            HandleMovement(ref newPoint, MovementState.left);
                        }
                        else
                        {
                            HandleMovement(ref newPoint, MovementState.right);
                        }
                        break;
                    }

                case MovementState.updown:
                    {
                        if (rnd.Next(0, 2) == 1)
                        {
                            HandleMovement(ref newPoint, MovementState.up);
                        }
                        else
                        {
                            HandleMovement(ref newPoint, MovementState.down);
                        }
                        break;
                    }

                case MovementState.random:
                    {
                        switch (rnd.Next(1, 5))
                        {
                            case 1:
                                {
                                    HandleMovement(ref newPoint, MovementState.up);
                                    break;
                                }
                            case 2:
                                {
                                    HandleMovement(ref newPoint, MovementState.down);
                                    break;
                                }

                            case 3:
                                {
                                    HandleMovement(ref newPoint, MovementState.left);
                                    break;
                                }
                            case 4:
                                {
                                    HandleMovement(ref newPoint, MovementState.right);
                                    break;
                                }
                        }
                        break;
                    }
            }

            return newPoint;
        }

        protected int HandleRotation(int oldRotation, RotationState state)
        {
            int rotation = oldRotation;
            switch (state)
            {
                case RotationState.clocwise:
                    {
                        HandleClockwiseRotation(ref rotation);
                        return rotation;
                    }

                case RotationState.counterClockwise:
                    {
                        HandleCounterClockwiseRotation(ref rotation);
                        return rotation;
                    }

                case RotationState.random:
                    {
                        if (rnd.Next(0, 3) == 1)
                        {
                            HandleClockwiseRotation(ref rotation);
                        }
                        else
                        {
                            HandleCounterClockwiseRotation(ref rotation);
                        }
                        return rotation;
                    }
            }

            return rotation;
        }

        private static void HandleClockwiseRotation(ref int rotation)
        {
            rotation = rotation + 2;
            if (rotation > 6)
                rotation = 0;
        }

        private static void HandleCounterClockwiseRotation(ref int rotation)
        {
            rotation = rotation - 2;
            if (rotation < 0)
                rotation = 6;
        }
    }
}

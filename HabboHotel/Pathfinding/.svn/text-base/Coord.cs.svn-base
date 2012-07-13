using System;
using System.Drawing;

namespace Butterfly.HabboHotel.Pathfinding
{
    //internal struct Coord : IEquatable<Coord>
    //{
    //    internal int X; 
    //    internal int Y; 

    //    internal Coord(int x, int y)
    //    {
    //        this.X = x;
    //        this.Y = y;
    //    }

    //    internal Coord(ThreeDCoord coord)
    //    {
    //        this.X = coord.X;
    //        this.Y = coord.Y;
    //    }

    //    public bool Equals(Coord comparedCoord)
    //    {
    //        return (X == comparedCoord.X && Y == comparedCoord.Y);
    //    }

    //    public static bool operator ==(Coord a, Coord b)
    //    {
    //        return (a.X == b.X && a.Y == b.Y);
    //    }

    //    public static bool operator !=(Coord a, Coord b)
    //    {
    //        return !(a == b);
    //    }

    //    public override int GetHashCode()
    //    {
    //        return X ^ Y;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null)
    //            return false;
    //        else
    //            return base.GetHashCode().Equals(obj.GetHashCode());
    //    }
    //}

    internal struct ThreeDCoord : IEquatable<ThreeDCoord>
    {
        internal int X;
        internal int Y;
        internal int Z;

        internal ThreeDCoord(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(ThreeDCoord comparedCoord)
        {
            return (X == comparedCoord.X && Y == comparedCoord.Y && Z ==comparedCoord.Z);
        }

        public bool Equals(Point comparedCoord)
        {
            return (X == comparedCoord.X && Y == comparedCoord.Y);
        }

        public static bool operator ==(ThreeDCoord a, ThreeDCoord b)
        {
            return (a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }

        public static bool operator !=(ThreeDCoord a, ThreeDCoord b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                return base.GetHashCode().Equals(obj.GetHashCode());
        }
    }
}

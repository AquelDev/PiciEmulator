namespace Butterfly.HabboHotel.Items
{
    class WallCoordinate
    {
        private readonly int widthX;
        private readonly int widthY;

        private readonly int lengthX;
        private readonly int lengthY;

        private readonly char side;

        public WallCoordinate(string wallPosition)
        {
            string[] posD = wallPosition.Split(' ');
            if (posD[2] == "l")
                side = 'l';
            else
                side = 'r';

            string[] widD = posD[0].Substring(3).Split(',');
            widthX = TextHandling.Parse(widD[0]);
            widthY = TextHandling.Parse(widD[1]);

            string[] lenD = posD[1].Substring(2).Split(',');
            lengthX = TextHandling.Parse(lenD[0]);
            lengthY = TextHandling.Parse(lenD[1]);
        }

        public WallCoordinate(double x, double y, sbyte n)
        {
            TextHandling.Split(x, out widthX, out widthY);
            TextHandling.Split(y, out lengthX, out lengthY);

            if (n == 7)
                side = 'r';
            else
                side = 'l';
        }

        public override string ToString()
        {
            return ":w=" + widthX + "," + widthY + " " + "l=" + lengthX + "," + lengthY + " " + side;
        }

        internal string GenerateDBShit()
        {
            return "x: " + TextHandling.Combine(widthX, widthY) + " y: " + TextHandling.Combine(lengthX, lengthY);
        }

        internal double GetXValue()
        {
            return TextHandling.Combine(widthX, widthY);
        }

        internal double GetYValue()
        {
            return TextHandling.Combine(lengthX, lengthY);
        }

        internal int n()
        {
            if (side == 'l')
                return 8;
            else
                return 7;
        }
    }
}

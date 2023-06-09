namespace Battleships.Models
{
    public class Coordinates
    {
        public int Row { get; set; }
        public char Column { get; set; }

        public Coordinates(int row, char column)
        {
            Row = row;
            Column = column;
        }
    }
}
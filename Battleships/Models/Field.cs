using Battleships.Models.Enums;

namespace Battleships.Models
{
    public class Field
    {
        public OccupationType OccupationType { get; set; }
        public ShipType ShipType { get; set; }
        public string Status { get; set; }
        public int ShipNumber { get; set; }
        public Coordinates Coordinates { get; }

        public Field(int row, char column)
        {
            Coordinates = new Coordinates(row, column);
            OccupationType = OccupationType.Empty;
            Status = "O";
            ShipType = ShipType.None;
        }

        public bool IsOccupiedByShip()
        {
            return ShipType != ShipType.None;
        }

    }
}
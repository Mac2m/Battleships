using Battleships.Models.Enums;

namespace Battleships.Models.Ships
{
    public abstract class Ship
    {
        public string? Name { get; set; }
        public int Width { get; set; }
        public int Hits { get; set; }
        public ShipType ShipType { get; set; }
        public int Number { get; set; }
        public bool IsSunk => Hits >= Width;
    }
}
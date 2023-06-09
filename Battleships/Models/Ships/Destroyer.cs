using Battleships.Models.Enums;

namespace Battleships.Models.Ships
{
    public class Destroyer : Ship
    {
        public Destroyer()
        {
            Name = "Destroyer";
            Width = 4;
            ShipType = ShipType.Destroyer;
        }
    }
}
using Battleships.Models.Enums;

namespace Battleships.Models.Ships
{
    public class BattleShip : Ship
    {
        public BattleShip()
        {
            Name = "BattleShip";
            Width = 5;
            ShipType = ShipType.BattleShip;
        }
    }
}
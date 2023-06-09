using Battleships.Models.Ships;

namespace Battleships.Models
{
    public class FiringBoard : Board
    {
        public List<Ship> Ships { get; set; }

        public bool GameOver
        {
            get { return Ships.All(x => x.IsSunk); }
        }

        public FiringBoard()
        {
            Ships = new List<Ship>()
            {
                new Destroyer(),
                new Destroyer(),
                new BattleShip()
            };
        }
  
    }
}
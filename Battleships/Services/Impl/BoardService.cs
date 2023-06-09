using Battleships.Models;
using Battleships.Models.Enums;

namespace Battleships.Services.Impl;

public class BoardService : IBoardService
{
    private readonly FiringBoard _firingBoard;
    private Player? _player;

    public BoardService()
    {
        _firingBoard = new FiringBoard();
    }
    
    public async Task<FiringBoard> SetUp()
    {
        await PlaceShipsRandomly();
        return _firingBoard;
    }

    public async Task<string> ProcessShot(Player player, int row, char column)
    {
        string result = $"Cannot fire there Captain {_player?.Name}!";
        await Task.Run(() =>
        {
            var field = _firingBoard.Fields.FirstOrDefault(x => x.Coordinates.Row == row && x.Coordinates.Column == column);

            if (field != null)
            {
                if (!field.IsOccupiedByShip() && field.OccupationType == OccupationType.Empty)
                {
                    field.OccupationType = OccupationType.Miss;
                    field.Status = "M";
                    result = "Miss!";
                }
                if (field.IsOccupiedByShip())
                {
                    var ship = _firingBoard.Ships.First(x => x.ShipType == field.ShipType && x.Number == field.ShipNumber);
                    ship.Hits++;
                    field.OccupationType = OccupationType.Hit;
                    field.Status = "X";
                    result = ship.IsSunk ? "Hit! It's sunken!" : "Hit!";
                }
            }
        });

        return result;
    }

    public bool CreateNewPlayer(string name)
    {
        _player = new Player(name);

        return _player.Name == name;
    }

    public Player GetPlayer()
    {
        return _player ?? throw new InvalidOperationException("Player is null...");
    }

    public bool CheckGameOver()
    {
        return _firingBoard.GameOver;
    }
    
    private async Task PlaceShipsRandomly()
    {
        await Task.Run(() =>
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int shipNumber = 1;
            foreach (var ship in _firingBoard.Ships)
            {
                bool isOpen = true;
                
                ship.Number = shipNumber;
                shipNumber++;

                while (isOpen)
                {
                    char startcolumn = (char)rand.Next(65, 74);
                    var startrow = rand.Next(1, 11);
                    int endrow = startrow;
                    char endcolumn = startcolumn;
                    var orientation = rand.Next(1, 101) % 2;

                    if (orientation == 0)
                    {
                        for (int i = 1; i < ship.Width; i++)
                        {
                            endrow++;
                        }
                    }
                    else
                    {
                        for (int i = 1; i < ship.Width; i++)
                        {
                            endcolumn++;
                        }
                    }

                    if (endrow > 10 || endcolumn > 'J')
                    {
                        isOpen = true;
                        continue;
                    }

                    var affectedPanels = _firingBoard.Fields.Where(x => x.Coordinates.Row >= startrow
                                    && x.Coordinates.Column >= startcolumn
                                    && x.Coordinates.Row <= endrow
                                    && x.Coordinates.Column <= endcolumn).ToList();
                    
                    bool hasAdjacentOccupiedFields = false;
                    foreach (var panel in affectedPanels)
                    {
                        var adjacentFields = GetAdjacentFields(panel.Coordinates.Row, panel.Coordinates.Column);
                        if (adjacentFields.Any(x => x.IsOccupiedByShip()))
                        {
                            hasAdjacentOccupiedFields = true;
                            break;
                        }
                    }
                    
                    if (affectedPanels.Any(x => x.IsOccupiedByShip()) || hasAdjacentOccupiedFields)
                    {
                        isOpen = true;
                        continue;
                    }

                    foreach (var panel in affectedPanels)
                    {
                        panel.ShipType = ship.ShipType;
                        panel.ShipNumber = ship.Number;
                    }
                    isOpen = false;
                }
            }
        });
    }
    
    private List<Field?> GetAdjacentFields(int row, char column)
    {
        var adjacentFields = new List<Field?>();
        if (row > 1)
        {
            var topRow = row - 1;
            adjacentFields.Add(_firingBoard.Fields.FirstOrDefault(x => x.Coordinates.Row == topRow && x.Coordinates.Column == column));
        }
        if (row < 10)
        {
            var bottomRow = row + 1;
            adjacentFields.Add(_firingBoard.Fields.FirstOrDefault(x => x.Coordinates.Row == bottomRow && x.Coordinates.Column == column));
        }
        if (column > 'A')
        {
            var leftColumn = (char)(column - 1);
            adjacentFields.Add(_firingBoard.Fields.FirstOrDefault(x => x.Coordinates.Row == row && x.Coordinates.Column == leftColumn));
        }
        if (column < 'J')
        {
            var rightColumn = (char)(column + 1);
            adjacentFields.Add(_firingBoard.Fields.FirstOrDefault(x => x.Coordinates.Row == row && x.Coordinates.Column == rightColumn));
        }

        return adjacentFields;
    }
}
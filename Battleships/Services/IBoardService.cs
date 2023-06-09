using Battleships.Models;

namespace Battleships.Services;

public interface IBoardService
{
    Task<FiringBoard> SetUp();
    Task<string> ProcessShot(Player player, int row, char column);
    bool CreateNewPlayer(string name);
    Player GetPlayer();
    bool CheckGameOver();
}
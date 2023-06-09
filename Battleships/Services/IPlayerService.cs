using Battleships.Models;

namespace Battleships.Services;

public interface IPlayerService
{
    bool CreateNewPlayer(string name);
    Task<string> Shot(Player player, int row, char column);
}
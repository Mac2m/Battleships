using Battleships.Models;

namespace Battleships.Services.Impl;

public class PlayerService : IPlayerService
{
    private readonly IBoardService _boardService;

    public PlayerService(IBoardService boardService)
    {
        _boardService = boardService;
    }

    public bool CreateNewPlayer(string name)
    {
        return _boardService.CreateNewPlayer(name);
    }

    public async Task<string> Shot(Player player, int row, char column)
    {
        var result = await _boardService.ProcessShot(player, row, column);

        return result;
    }
}
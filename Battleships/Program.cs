using Battleships.Models;
using Battleships.Services;
using Battleships.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Battleships;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddScoped<IBoardService, BoardService>()
            .AddTransient<IPlayerService, PlayerService>()
            .BuildServiceProvider();
        
        var boardService = serviceProvider.GetService<IBoardService>();
        var playerService = serviceProvider.GetService<IPlayerService>();

        try
        {
            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("Please write your name:");
            var playerName = Console.ReadLine();
            while (string.IsNullOrEmpty(playerName))
            {
                Console.WriteLine("Please write your name:");
                playerName = Console.ReadLine();
            }
            playerService?.CreateNewPlayer(playerName);
            var player = boardService?.GetPlayer();
            Console.WriteLine($"Let's start the game Captain {playerName}!");
            Console.WriteLine();
            var board = await boardService?.SetUp()!;
            DrawBoard(board);
            while (!boardService.CheckGameOver())
            {
                Console.WriteLine("Captain, where do you want to shot?");
                Console.WriteLine("Choose coordinates");

                string coords = Console.ReadLine()!.ToUpper();
                if (!string.IsNullOrEmpty(coords))
                {
                    var coordinates = MapToCoords(coords);
                    if (player != null)
                    {
                        var shotResult = await playerService?.Shot(player, coordinates.Item1, coordinates.Item2)!;
                        Console.WriteLine(shotResult);
                    }

                    DrawBoard(board);
                }
                else
                    Console.Write("Choose proper coordinates");
            }
            
            if (boardService.CheckGameOver())
                Console.Write("All enemy ships are sunken, you win! Game is over.");
                
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static Tuple<int, char> MapToCoords(string coords)
    {
        var isColumn = char.TryParse(coords.Substring(0, 1), out var coordColumn);
        var isRow = int.TryParse(coords.Substring(1), out var coordRow);
        if (!isColumn || !isRow)
            Console.WriteLine("Choose correct coordinates.");
        if ((coordRow > 10 || coordColumn > 'J') || (coordRow < 1 || coordColumn < 'A'))
            Console.WriteLine("Choose correct coordinates.");
        return new Tuple<int, char>(coordRow, coordColumn);
    }

    private static void DrawBoard(FiringBoard board)
    {
        Console.WriteLine("    A B C D E F G H I J ");
        Console.WriteLine("    ___________________ ");
        for (int row = 1; row <= 10; row++)
        {
            Console.Write(row != 10 ? row + " | " : row + "| ");
            for (char column = 'A'; column <= 'J'; column++)
            {
                Console.Write(board.Fields.First(x => x.Coordinates.Row == row && x.Coordinates.Column == column).Status + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }
}
using Battleships.Models;
using Battleships.Models.Enums;
using Battleships.Models.Ships;
using Battleships.Services;
using Battleships.Services.Impl;

namespace Battleships.Tests;

    [TestFixture]
    public class BoardServiceTests
    {
        private IBoardService _boardService;

        [SetUp]
        public void Setup()
        {
            _boardService = new BoardService();
            _boardService.CreateNewPlayer("Test Player");
        }

        [Test]
        public async Task SetUp_ShouldPlaceShipsRandomly()
        {
            // Act
            var firingBoard = await _boardService.SetUp();

            // Assert
            Assert.IsNotNull(firingBoard);
            Assert.IsTrue(firingBoard.Ships.Count > 0);
        }

        [Test]
        public async Task ProcessShot_ShouldReturnMiss_WhenFieldIsEmpty()
        {
            // Arrange
            var row = 1;
            var column = 'A';

            // Act
            var result = await _boardService.ProcessShot(_boardService.GetPlayer(), row, column);

            // Assert
            Assert.That(result, Is.EqualTo("Miss!"));
        }

        [Test]
        public async Task ProcessShot_ShouldReturnHit_WhenFieldIsOccupiedByShip()
        {
            // Arrange
            var board = await _boardService.SetUp();
            var row = 1;
            var column = 'A';
            var ship = new BattleShip();
            var field = new Field(row, column)
            {
                ShipType = ship.ShipType,
                ShipNumber = ship.Number,
            };
            board.Fields.Insert(0, field);
            board.Ships.Add(ship);

            // Act
            var result = await _boardService.ProcessShot(_boardService.GetPlayer(), row, column);

            // Assert
            Assert.That(result, Is.EqualTo("Hit!"));
            Assert.That(field.OccupationType, Is.EqualTo(OccupationType.Hit));
            Assert.That(field.Status, Is.EqualTo("X"));
            Assert.That(ship.Hits, Is.EqualTo(1));
        }

        [Test]
        public async Task ProcessShot_ShouldReturnSunk_WhenShipIsSunk()
        {
            // Arrange
            var board = await _boardService.SetUp();
            var row = 1;
            var column = 'A';
            var ship = new BattleShip
            {
                Hits = 4
            };
            var field = new Field(row, column)
            {
                OccupationType = OccupationType.Empty,
                ShipType = ship.ShipType,
                ShipNumber = ship.Number
            };
            board.Fields.Insert(0, field);
            board.Ships.Add(ship);

            // Act
            var result = await _boardService.ProcessShot(_boardService.GetPlayer(), row, column);

            // Assert
            Assert.That(result, Is.EqualTo("Hit! It's sunken!"));
            Assert.That(field.OccupationType, Is.EqualTo(OccupationType.Hit));
            Assert.That(field.Status, Is.EqualTo("X"));
            Assert.That(ship.Hits, Is.EqualTo(5));
        }

        [Test]
        public void CreateNewPlayer_ShouldCreateNewPlayer()
        {
            // Arrange
            var playerName = "New Player";

            // Act
            var result = _boardService.CreateNewPlayer(playerName);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(_boardService.GetPlayer());
            Assert.That(_boardService.GetPlayer().Name, Is.EqualTo(playerName));
        }

        [Test]
        public void CheckGameOver_ShouldReturnFalse_WhenGameIsNotOver()
        {
            // Act
            var result = _boardService.CheckGameOver();

            // Assert
            Assert.IsFalse(result);
        }
    }
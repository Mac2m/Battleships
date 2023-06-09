using Battleships.Models;
using Battleships.Services;
using Battleships.Services.Impl;
using Moq;

namespace Battleships.Tests
{
    [TestFixture]
    public class PlayerServiceTests
    {
        private Mock<IBoardService> _boardServiceMock;
        private IPlayerService _playerService;

        [SetUp]
        public void Setup()
        {
            _boardServiceMock = new Mock<IBoardService>();
            _playerService = new PlayerService(_boardServiceMock.Object);
        }

        [Test]
        public void CreateNewPlayer_ShouldCall_CreateNewPlayer_OnBoardService()
        {
            // Arrange
            var playerName = "Test Player";

            // Act
            _playerService.CreateNewPlayer(playerName);

            // Assert
            _boardServiceMock.Verify(b => b.CreateNewPlayer(playerName), Times.Once);
        }

        [Test]
        public async Task Shot_ShouldCall_ProcessShot_OnBoardService_AndReturnResult()
        {
            // Arrange
            var player = new Player("Test Player");
            var row = 1;
            var column = 'A';
            var expectedResult = "Hit!";
            _boardServiceMock.Setup(b => b.ProcessShot(player, row, column)).ReturnsAsync(expectedResult);

            // Act
            var result = await _playerService.Shot(player, row, column);

            // Assert
            _boardServiceMock.Verify(b => b.ProcessShot(player, row, column), Times.Once);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
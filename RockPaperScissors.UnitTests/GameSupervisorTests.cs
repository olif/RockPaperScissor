using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace RockPaperScissors.UnitTests
{
    public class GameSupervisorTests
    {
        [Fact]
        public void SupervisorCleansOutdatedGames()
        {
            var logger = new Mock<ILogger<GameSupervisor>>();
            var supervisor = new GameSupervisor(logger.Object, 100);
            var player = new Player("Player1");
            var game = supervisor.NewGame(player);
            var gamesBeforeClean = supervisor.CountGames();

            Thread.Sleep(150);

            Assert.Equal(1, gamesBeforeClean);
            Assert.Equal(0, supervisor.CountGames());
        }
    }
}

using Xunit;

namespace RockPaperScissors.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void GameCanBeCreatedWithAPlayer()
        {
            var player1 = new Player("Player1");

            var game = new Game(player1);

            Assert.NotNull(game.Id);
            Assert.Equal(player1, game.Player1);
            Assert.Equal(Move.None, game.Player1Move);
            Assert.Equal(Move.None, game.Player2Move);
            Assert.False(game.IsFinished());
        }

        [Fact]
        public void GameCanBeJoined()
        {
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            var game = new Game(player1);

            var gameState = game.Join(player2);

            Assert.Equal(player1, game.Player1);
            Assert.False(game.IsFinished());
            Assert.Equal(player2, gameState.CurrentPlayer);
            Assert.Equal(Move.None, gameState.CurrentPlayerMove);
        }

        [Fact]
        public void Player1CanMakeAMove()
        {
            var player1 = new Player("Player1");
            var game = new Game(player1);

            var gameState = game.MakeMove(player1, Move.Rock);

            Assert.Equal(Move.Rock, gameState.CurrentPlayerMove);
        }

        [Fact]
        public void Player2CanMakeAMove()
        {
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            var game = new Game(player1);
            game.Join(player2);

            var gameState = game.MakeMove(player2, Move.Scissor);

            Assert.Equal(player2, gameState.CurrentPlayer);
            Assert.Equal(Move.None, gameState.OpponentPlayerMove);
            Assert.Equal(Move.Scissor, gameState.CurrentPlayerMove);
        }

        [Fact]
        public void BothPlayerCanMakeAMoveAndEndGame()
        {
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            var game = new Game(player1);
            game.Join(player2);

            game.MakeMove(player1, Move.Paper);
            game.MakeMove(player2, Move.Rock);

            var player1State = game.State(player1);
            var player2State = game.State(player2);

            Assert.Equal(Move.Paper, player1State.CurrentPlayerMove);
            Assert.Equal(Move.Rock, player1State.OpponentPlayerMove);
            Assert.Equal(Move.Rock, player2State.CurrentPlayerMove);
            Assert.Equal(Move.Paper, player2State.OpponentPlayerMove);
            Assert.True(player1State.GameFinished);
            Assert.Equal(player1State.Winner, player1);
            Assert.Equal(player2State.Winner, player1);
        }

        [Fact]
        public void CannotJoinAnAlreadyJoinedGame()
        {
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            var player3 = new Player("Player3");
            var game = new Game(player1);
            game.Join(player2);

            Assert.Throws(typeof(GameException), () => game.Join(player3));

        }

        [Fact]
        public void PlayerMakingAMoveOnNotJoinedGameResultsInGameException()
        {
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            var player3 = new Player("Player3");
            var game = new Game(player1);
            game.Join(player2);

            Assert.Throws(typeof(GameException), () => game.MakeMove(player3, Move.Rock));
        }
    }
}

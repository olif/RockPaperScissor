
using System;

namespace RockPaperScissors
{
    public class GameState
    {
        public Guid Id { get; }

        public Player CurrentPlayer { get; }

        public Player Winner { get; }

        public Player Opponent { get; }

        public bool GameFinished { get; }

        public Move CurrentPlayerMove { get; }

        public Move OpponentPlayerMove { get; }

        public GameState(Player currentPlayer, Game game)
        {
            Id = game.Id;
            CurrentPlayer = currentPlayer;
            CurrentPlayerMove = GetMoveForCurrentPlayer(currentPlayer, game);
            Opponent = GetOpponent(currentPlayer, game);
            OpponentPlayerMove = Move.None;
            Winner = Player.VacantPlayer;

            if(game.IsFinished())
            {
                GameFinished = true;
                OpponentPlayerMove = GetOpponentMove(currentPlayer, game);
                Winner = GetWinner(game);
            }
        }

        private Player GetWinner(Game game)
        {
            if (game.Player1Move.WinsOver(game.Player2Move))
            {
                return game.Player1;
            }
            else if (game.Player2Move.WinsOver(game.Player1Move))
            {
                return game.Player2;
            }
            else {
                return Player.DrawPlayer;
            }
        }

        private static Move GetMoveForCurrentPlayer(Player currentPlayer, Game game)
        {
            if(currentPlayer == game.Player1)
            {
                return game.Player1Move;
            }
            else if(currentPlayer == game.Player2)
            {
                return game.Player2Move;
            }
            else
            {
                return Move.None;
            }
        }

        private static Player GetOpponent(Player currentPlayer, Game game)
        {
            if(currentPlayer == game.Player1)
            {
                return game.Player2;
            }
            else if(currentPlayer == game.Player2)
            {
                return game.Player1;
            }
            else {
                return Player.VacantPlayer;
            }
        }

        private static Move GetOpponentMove(Player currentPlayer, Game game)
        {
            if(currentPlayer == game.Player1)
            {
                return game.Player2Move;
            }
            else if(currentPlayer == game.Player2)
            {
                return game.Player1Move;
            }
            else {
                return Move.None;
            }
        }
    }
}

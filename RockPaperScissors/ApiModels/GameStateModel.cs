using System;

namespace RockPaperScissors.ApiModels
{
    public class GameStateModel
    {
        public Guid GameId { get; set; }

        public string CurrentPlayer { get; set; }

        public string Opponent { get; set; }

        public string Winner { get; set; }

        public bool GameFinished { get; set; }

        public string CurrentPlayerMove { get; set; }

        public string OpponentPlayerMove { get; set; }

        public GameStateModel()
        {}

        public GameStateModel(GameState gameState)
        {
            GameId = gameState.Id;
            CurrentPlayer = gameState.CurrentPlayer.Name;
            Opponent = gameState.Opponent.Name;
            Winner = gameState.Winner?.Name;
            GameFinished = gameState.GameFinished;
            CurrentPlayerMove = gameState.CurrentPlayerMove.Name;
            OpponentPlayerMove = gameState.OpponentPlayerMove.Name;
        }
    }
}

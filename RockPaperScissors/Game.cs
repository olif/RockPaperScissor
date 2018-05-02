using System;

namespace RockPaperScissors
{
    public class Game
    {
        public Guid Id { get; }

        public Player Player1 { get; private set; }

        public Player Player2 { get; private set; }

        public Move Player1Move { get; private set; }

        public Move Player2Move { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public Game(Player player1)
        {
            this.Id = Guid.NewGuid();
            this.Player1 = player1;
            this.Player2 = Player.VacantPlayer;
            this.Player1Move = Move.None;
            this.Player2Move = Move.None;
        }

        public GameState State(Player currentPlayer) =>
            new GameState(currentPlayer, this);

        public bool IsFinished() =>
            Player1Move != Move.None && Player2Move != Move.None;


        public GameState Join(Player player)
        {
            UpdateLastUpdated();

            if(Player2 != Player.VacantPlayer)
            {
                var errMsg = "Cannot join a game that has already started";
                throw new GameException(errMsg);
            }

            Player2 = player;
            return new GameState(player, this);
        }

        public GameState MakeMove(Player player, Move move)
        {
            UpdateLastUpdated();

            if (IsFinished())
            {
                return new GameState(player, this);   
            }

            if (player == Player1)
            {
                Player1Move = move;
            }
            else if (player == Player2)
            {
                Player2Move = move;
            }
            else
            {
                var errMsg = $"Player {player.Name} not registrered for game with id: {Id}";
                throw new GameException(errMsg);
            }
            return new GameState(player, this);
        }

        private void UpdateLastUpdated() => LastUpdated = DateTime.Now;
    }
}

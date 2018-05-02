using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace RockPaperScissors
{
    public class GameSupervisor
    {
        private IDictionary<Guid, Game> _games;
        private Timer _timer;
        private int _cleanInteralMs;
        private ILogger _logger;

        public GameSupervisor(ILogger<GameSupervisor> logger, int cleanIntervalMs = 60*60*60)
        {
            _games = new ConcurrentDictionary<Guid, Game>();
            _logger = logger;
            _cleanInteralMs = cleanIntervalMs; 
            _timer = new Timer(cleanIntervalMs);
            _timer.Enabled = true;
            _timer.Elapsed += (sender, e) => CleanGames();
            _timer.Start();
        }

        public int CountGames() => _games.Count;

        public GameState NewGame(Player player)
        {
            var game = new Game(player);
            _games.Add(game.Id, game);
            _logger.LogInformation($"Creating new game with id {game.Id}");

            return game.State(player);
        }

        public GameState GetGame(Guid gameId, Player player)
        {
            if(_games.TryGetValue(gameId, out Game game))
            {
                return game.State(player);
            }

            throw new GameNotFoundException($"Game with id: {gameId} was not found");
        }

        public GameState JoinGame(Guid gameId, Player opponent)
        {
            if(_games.TryGetValue(gameId, out Game game))
            {
                lock(game)
                {
                    var gameState = game.Join(opponent);
                    _logger.LogInformation("Player: {opponent} joined game: {gameId}");
                    return gameState;
                }
            }

            throw new GameNotFoundException($"Game with id: {gameId} was not found");
        }

        public GameState MakeMove(Guid gameId, Player player, Move move)
        {
            if(_games.TryGetValue(gameId, out Game game))
            {
                lock(game)
                {
                    var gameState = game.MakeMove(player, move);
                    _logger.LogInformation("Player: {opponent} made move: {move} on game: {gameId}");
                    return gameState;
                }
            }

            throw new GameNotFoundException($"Game with id: {gameId} was not found");
        }

        private void CleanGames()
        {
            _logger.LogDebug("Cleaning games");
            var thresholdTime = DateTime
                .Now
                .AddMilliseconds(-1 * _cleanInteralMs);

            var gamesToRemove = _games
                .Where(x => x.Value.LastUpdated < thresholdTime)
                .Select(x => x.Key);

            foreach(var id in gamesToRemove)
            {
                _games.Remove(id);
            }
        }
    }
}

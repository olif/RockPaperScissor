using System;
using Microsoft.AspNetCore.Mvc;
using RockPaperScissors.ApiModels;

namespace RockPaperScissors
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly GameSupervisor _gameSupervisor;

        public GamesController(GameSupervisor gameSupervisor)
        {
            _gameSupervisor = gameSupervisor;
        }

        [HttpGet("{id:guid}")]
        public GameStateModel GetGame(Guid id, [FromQueryAttribute]string playerName)
        {
            var player = new Player(playerName);
            var gameState = _gameSupervisor.GetGame(id, player);
            return new GameStateModel(gameState);
        }

        [HttpPost("new")]
        public GameStateModel NewGame([FromBody]PlayerModel playerModel)
        {
            var player = new Player(playerModel.Name);
            var gameState =_gameSupervisor.NewGame(player);

            return new GameStateModel(gameState);
        }

        [HttpPost("{id:guid}/join")]
        public GameStateModel JoinGame(Guid id,
                                       [FromBody]PlayerModel opponentModel)
        {
            var opponent = new Player(opponentModel.Name);
            var gameState = _gameSupervisor.JoinGame(id, opponent);

            return new GameStateModel(gameState);
        }

        [HttpPost("{id:guid}/move")]
        public GameStateModel MakeMove(Guid id, [FromBody]MoveModel moveModel)
        {
            var player = new Player(moveModel.Name);
            var move = Move.GetMoveFromString(moveModel.Move);
            var gameState =_gameSupervisor.MakeMove(id, player, move);

            return new GameStateModel(gameState);
        }
    }
}

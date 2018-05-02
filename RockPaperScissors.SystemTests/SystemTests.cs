using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RockPaperScissors.ApiModels;
using Xunit;

namespace RockPaperScissors.SystemTests
{
    public class SystemTests
    {
        private HttpClient _testClient;
        private PlayerModel Player = new PlayerModel() { Name = "Player" };
        private PlayerModel Opponent = new PlayerModel() { Name = "Opponent" };

        public SystemTests()
        {
            _testClient = TestUtils.GetNewTestClient();
        }

        [Fact]
        public async void CanPlayAGameViaApi()
        {
            // Create game with player
            var newState = await EnsurePost<PlayerModel, GameStateModel>("/api/games/new", Player);

            Assert.NotNull(newState);
            Assert.Equal(Player.Name, newState.CurrentPlayer);

            var gameId = newState.GameId;
            // Join game with opponent
            var joinState = await EnsurePost<PlayerModel, GameStateModel>($"/api/games/{gameId}/join", Opponent);

            Assert.NotNull(joinState);
            Assert.Equal(Opponent.Name, joinState.CurrentPlayer);
            Assert.Equal(Player.Name, joinState.Opponent);

            var player1MoveModel = new MoveModel { Name = Player.Name, Move = Move.Rock.Name };
            // Player 1 makes a move
            var player1MoveState = await EnsurePost<MoveModel, GameStateModel>($"/api/games/{gameId}/move", player1MoveModel);

            Assert.NotNull(player1MoveState);
            Assert.Equal(Move.None.Name, player1MoveState.OpponentPlayerMove);
            Assert.Equal(Move.Rock.Name, player1MoveState.CurrentPlayerMove);

            var player2MoveModel = new MoveModel { Name = Opponent.Name, Move = Move.Scissor.Name };
            // Player 2 makes a move
            var player2MoveState = await EnsurePost<MoveModel, GameStateModel>($"/api/games/{gameId}/move", player2MoveModel);

            Assert.NotNull(player2MoveState);
            Assert.Equal(Move.Scissor.Name, player2MoveState.CurrentPlayerMove);
            Assert.Equal(Move.Rock.Name, player2MoveState.OpponentPlayerMove);
        }


        [Fact]
        public async void ApiReturns400ErrorOnInvalidMove()
        {
            var newState = await EnsurePost<PlayerModel, GameStateModel>("/api/games/new", Player);

            Assert.NotNull(newState);
            Assert.Equal(Player.Name, newState.CurrentPlayer);

            var gameId = newState.GameId;

            var player1MoveModel = new MoveModel { Name = Player.Name, Move = "Spock" };
            var player1MoveResult = await _testClient.PostAsync($"/api/games/{gameId}/move", ToJsonContent(player1MoveModel));
            Assert.Equal(HttpStatusCode.BadRequest, player1MoveResult.StatusCode);
        }

        [Fact]
        public async void ApiReturns404InvalidGameId()
        {
            var getResult = await _testClient.GetAsync($"/api/games/{Guid.NewGuid()}?playerName=Spock");
            Assert.Equal(HttpStatusCode.NotFound, getResult.StatusCode);
        }


        private async Task<U> EnsurePost<T, U>(string uri, T model)
        {
            var result = await _testClient.PostAsync(uri, ToJsonContent(model));
            result.EnsureSuccessStatusCode();
            var responseContent = await result.Content.ReadAsStringAsync();
            return FromJson<U>(responseContent);
        }

        private StringContent ToJsonContent<T>(T model)
        {
            var jsonStr = JsonConvert.SerializeObject(model);
            return new StringContent(jsonStr, Encoding.UTF8, "application/json");
        }

        private T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

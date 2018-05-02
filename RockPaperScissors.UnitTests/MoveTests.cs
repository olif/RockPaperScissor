using System.Collections.Generic;
using Xunit;

namespace RockPaperScissors.UnitTests
{
    public class MoveTests
    {
        [Theory]
        [MemberData(nameof(GetMoveTestData))]
        public void WinsOverTests(Move move1, Move move2, bool isWinning)
        {
            Assert.Equal(isWinning, move1.WinsOver(move2));
        }

        public static IEnumerable<object[]> GetMoveTestData
        {
            get
            {
                return new []
                {
                    new object[] {Move.Rock, Move.Scissor, true},
                    new object[] {Move.Rock, Move.Paper, false},
                    new object[] {Move.Scissor, Move.Paper, true},
                    new object[] {Move.Scissor, Move.Rock, false},
                    new object[] {Move.Paper, Move.Rock, true},
                    new object[] {Move.Paper, Move.Scissor, false}
                };
            }
        }
    }
}

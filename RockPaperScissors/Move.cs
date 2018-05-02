using System;
using System.Collections.Generic;
using System.Linq;

namespace RockPaperScissors
{
    public class Move : ValueObject<Move>
    {
        public static Move None = new Move(0, "None");
        public static Move Rock = new Move(1, "Rock");
        public static Move Scissor = new Move(2, "Scissor");
        public static Move Paper = new Move(3, "Paper");
        public static IReadOnlyList<Move> ValidMoves = new List<Move> {
            Move.Rock, Move.Scissor, Move.Paper
        };

        private int _moveId;

        public string Name { get; }

        private Move(int moveId, string name)
        {
            _moveId = moveId;
            Name = name;
        }

        public bool WinsOver(Move otherMove) =>
            mod(_moveId - otherMove._moveId, 3) == 2;

        public static Move GetMoveFromString(string moveName)
        {
            var lowerCaseMove = moveName.ToLower();
            var move = ValidMoves
                .FirstOrDefault(x => x.Name.ToLower() == lowerCaseMove);

            if(move == null)
            {
                throw new GameException("No valid move specified");
            }

            return move;
        }

        // C# does not have a modulo operator, defining one.
        private int mod(int a, int n)
        {
            return ((a % n) + n) % n;
        }

        protected override bool EqualsCore(Move other)
        {
            return this._moveId == other._moveId;
        }

        protected override int GetHashCodeCore()
        {
            return this._moveId.GetHashCode();
        }
    }
}

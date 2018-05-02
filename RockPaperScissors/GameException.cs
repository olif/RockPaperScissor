using System;

namespace RockPaperScissors
{
    public class GameException : Exception
    {
        public GameException()
        {
        }

        public GameException(string message) : base(message)
        {
        }
    }

    public class GameNotFoundException : GameException
    {
        public GameNotFoundException(string message) : base(message)
        {
        }
    }
}

namespace RockPaperScissors
{
    public class Player : ValueObject<Player>
    {

        public static Player VacantPlayer = new Player("No player");
        public static Player DrawPlayer = new Player("Draw");

        public string Name { get; private set; }

        public Player(string name)
        {
            this.Name = name;
        }

        protected override bool EqualsCore(Player other)
        {
            return this.Name.ToLower() == other.Name.ToLower();
        }

        protected override int GetHashCodeCore()
        {
            return this.Name.ToLower().GetHashCode();
        }
    }
}

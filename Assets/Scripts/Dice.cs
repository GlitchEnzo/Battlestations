namespace Battlestations
{
    using System;

    public static class Dice
    {
        private static Random random;

        private static Random Random
        {
            get
            {
                if (random == null)
                {
                    random = new Random();
                }

                return random;
            }
        }

        public static int Roll(int numberOfDice = 1)
        {
            return Random.Next(1, 6 * numberOfDice + 1);
        }
    }
}

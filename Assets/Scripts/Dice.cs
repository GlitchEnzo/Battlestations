namespace Battlestations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a generic interface for rolling & calculating the probabilities of any number of dice with any number of sides.
    /// </summary>
    public static class Dice
    {
        /// <summary>
        /// The random number generator used for rolling dice.
        /// </summary>
        private static Random random;

        /// <summary>
        /// The cached frequency data for each number from a set of dice with various number of sides.
        /// Usage = CachedFrequencies[NumberOfSides][NumberOfDice][SpecificNumberToGetFrequencyOf] = FrequencyOfNumber
        /// </summary>
        private static Dictionary<int, Dictionary<int, Dictionary<int, int>>> CachedFrequencies = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();

        /// <summary>
        /// Initializes data for the dice evaluation.
        /// </summary>
        static Dice()
        {
            random = new Random();

            //CacheFrequencies(6, 0);
            CacheFrequencies(6, 1);
            CacheFrequencies(6, 2);
            CacheFrequencies(6, 3);

            string output = string.Empty;
            foreach (var pair in CachedFrequencies[6][3])
            {
                output += pair.Key + ":" + pair.Value + "\n";
            }
            UnityEngine.Debug.Log(output);
        }

        /// <summary>
        /// Caches the frequencies for each number possible from the given number of dice with the given number of sides.
        /// </summary>
        /// <param name="numberOfSides">The number of sides on the dice.</param>
        /// <param name="numberOfDice">The number of dice to roll.</param>
        private static void CacheFrequencies(int numberOfSides, int numberOfDice)
        {
            if (CachedFrequencies.ContainsKey(numberOfSides))
            {
                if (CachedFrequencies[numberOfSides].ContainsKey(numberOfDice))
                {
                    //UnityEngine.Debug.LogWarningFormat("Dice results already cached: Sides={0}, Number={1}", numberOfSides, numberOfDice);

                    // do nothing since the data is already cached.
                }
                else
                {
                    var frequencies = GetFrequenciesByOutcome(numberOfDice, numberOfSides);

                    CachedFrequencies[numberOfSides].Add(numberOfDice, frequencies);
                }
            }
            else
            {
                var frequencies = GetFrequenciesByOutcome(numberOfDice, numberOfSides);

                CachedFrequencies.Add(numberOfSides, new Dictionary<int, Dictionary<int, int>>());
                CachedFrequencies[numberOfSides].Add(numberOfDice, frequencies);
            }
        }

        /// <summary>
        /// Rolls the specified number of dice with the given number of sides (defaults to 1 6-sided die).
        /// </summary>
        /// <param name="numberOfDice">The number of dice to roll.  Defaults to 1.</param>
        /// <param name="numberOfSides">The number of sides on the dice.  Defaults to 6.</param>
        /// <returns>The sum of the dice rolls.</returns>
        public static int Roll(int numberOfDice = 1, int numberOfSides = 6)
        {
            int result = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                // minValue is INCLUSIVE, but maxValue is EXCLUSIVE, thus the +1
                result += random.Next(1, numberOfSides + 1);
            }

            return result;
        }

        /// <summary>
        /// Gets the percentage probability of getting the given number or HIGHER using the given number of dice.
        /// </summary>
        /// <param name="numberOfDice"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static float Probability(int numberOfDice, int minValue, int numberOfSides = 6)
        {
            // Cache new frequency data if not already cached.
            CacheFrequencies(numberOfSides, numberOfDice);

            int possibleResults = 0;

            for (int i = minValue; i <= numberOfSides * numberOfDice; i++)
            {
                if (i > 0)
                {
                    possibleResults += CachedFrequencies[numberOfSides][numberOfDice][i];
                }
            }

            return possibleResults / (float)Math.Pow(numberOfSides, numberOfDice);
        }

        /// <summary>
        /// Generates a Dictionary of frequencies for each number result based on the given number of n-sided dice.
        /// NOTE: This is completely brute forcing it by actually calculating each possible result.
        /// From here: http://stackoverflow.com/questions/493239/determine-frequency-of-numbers-showing-up-in-dice-rolls
        /// </summary>
        /// <param name="numberOfDice">The number of dice being rolled.</param>
        /// <param name="numberOfSides">The number of sides on the dice.</param>
        /// <returns>A dictionary of the frequencies of each number.</returns>
        private static Dictionary<int, int> GetFrequenciesByOutcome(int numberOfDice, int numberOfSides)
        {
            int maxOutcome = (numberOfDice * numberOfSides);
            Dictionary<int, int> outcomeCounts = new Dictionary<int, int>();
            for (int i = 0; i <= maxOutcome; i++)
                outcomeCounts[i] = 0;

            if (numberOfDice > 0)
            {
                foreach (int possibleOutcome in GetAllOutcomes(0, numberOfDice, numberOfSides))
                    outcomeCounts[possibleOutcome] = outcomeCounts[possibleOutcome] + 1;
            }

            //return outcomeCounts.Where(kvp => kvp.Value > 0);
            return outcomeCounts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTotal"></param>
        /// <param name="nDice"></param>
        /// <param name="nSides"></param>
        /// <returns></returns>
        private static IEnumerable<int> GetAllOutcomes(int currentTotal, int nDice, int nSides)
        {
            if (nDice == 0)
                yield return currentTotal;
            else
            {
                for (int i = 1; i <= nSides; i++)
                    foreach (int outcome in GetAllOutcomes(currentTotal + i, nDice - 1, nSides))
                        yield return outcome;
            }
        }
    }
}

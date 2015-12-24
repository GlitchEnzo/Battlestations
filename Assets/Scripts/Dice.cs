namespace Battlestations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        /// <summary>
        /// CachedFrequencies[NumberOfSides][NumberOfDice][SpecificNumberToGetFrequencyOf] = frequency
        /// </summary>
        private static Dictionary<int, Dictionary<int, Dictionary<int, int>>> CachedFrequencies = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();

        static Dice()
        {
            //cache 2 6-sided dice frequencies
            //var twoSixSided = GetFrequenciesByOutcome(2, 6);
            //var sixSided = new Dictionary<int, Dictionary<int, int>>();
            //sixSided.Add(2, twoSixSided);
            //CachedFrequencies.Add(6, sixSided);

            CacheFrequencies(6, 2);

            string output = string.Empty;
            for (int i = 0; i < 6 * 2; i++)
            {
                output += i + ":" + CachedFrequencies[6][2][i] + "\n";
            }

            //foreach (var pair in CachedFrequencies[6][2])
            //{
            //    output += pair.Key + ":" + pair.Value + "\n";
            //}

            UnityEngine.Debug.Log(output);
        }

        private static void CacheFrequencies(int numberOfSides, int numberOfDice)
        {
            if (CachedFrequencies.ContainsKey(numberOfSides))
            {
                if (CachedFrequencies[numberOfSides].ContainsKey(numberOfDice))
                {
                    UnityEngine.Debug.LogWarningFormat("Dice results already cached: Sides={0}, Number={1}", numberOfSides, numberOfDice);
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

        // number of possible results = numberOfSides ^ numberOfDice
        // minimum result = numberOfDice
        // maximum result = numberOfSides * numberOfDice
        // probability of a specific number = (numberOfResultsForSpecificNumber) / numberOfPossibleResults
        // number of results for a specific number = ???

        private static int[] TwoDice = new[]
        {
            0, // 0 - no way to get a 0 with 2 dice
            0, // 1 - no way to get a 1 with 2 dice
            1, // 2 - 1  way to get a 2 with 2 dice
            2, // 3 - 2 ways to get a 3 with 2 dice
            3, // etc
            4,
            5,
            6, // 7
            5,
            4,
            3,
            2,
            1  // 12
        }; // 36 total possible results (6 * 6)

        private static int[] ThreeDice = new[]
        {
            0, // 0 - no way to get a 0 with 3 dice
            0, // 1 - no way to get a 1 with 3 dice
            0, // 2 - no way to get a 2 with 3 dice
            1, // 3 - 1  way to get a 3 with 3 dice
            3, // etc
            6,
            10,
            15, // 7
            21,
            25,
            27,
            27,
            25, // 12
            21,
            15,
            10,
            6,
            2,
            1  // 18
        }; // 216 possible results (6 * 6 * 6)

        public static int Roll(int numberOfDice = 1)
        {
            int result = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                // minValue is INCLUSIVE, but maxValue is EXCLUSIVE, thus the 1-7
                result += Random.Next(1, 7);
            }

            return result;
        }

        /// <summary>
        /// Gets the percentage probability of getting the given number or HIGHER using the given number of dice.
        /// </summary>
        /// <param name="numberOfDice"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static float Probability(int numberOfDice, int minValue)
        {
            float probability = 0;

            int possibleResults = 0;

            switch (numberOfDice)
            {
                case 0:
                    probability = 0;
                    break;
                case 1:
                    for (int i = minValue; i <= 6; i++)
                    {
                        // each number has a 1/6 chance, so simply increment for each one
                        possibleResults++;
                    }
                    probability = possibleResults / 6.0f;
                    break;
                case 2:
                    for (int i = minValue; i <= 12; i++)
                    {
                        if (i > 0)
                        {
                            possibleResults += TwoDice[i];
                        }
                    }
                    probability = possibleResults / 36.0f;
                    break;
                case 3:
                    for (int i = minValue; i <= 18; i++)
                    {
                        if (i > 0)
                        {
                            possibleResults += ThreeDice[i];
                        }
                    }
                    probability = possibleResults / 216.0f;
                    break;
                default:
                    UnityEngine.Debug.LogFormat("Unsupported number of dice: {0}", numberOfDice);
                    probability = 0;
                    break;
            }

            return probability;
        }

        /// <summary>
        /// Generates a Dictionary of frequencies for each number result based on the given number of n-sided dice.
        /// NOTE: This is completely brute forcing it by actually calculating each possible result.
        /// From here: http://stackoverflow.com/questions/493239/determine-frequency-of-numbers-showing-up-in-dice-rolls
        /// </summary>
        /// <param name="numberOfDice"></param>
        /// <param name="numberOfSides"></param>
        /// <returns></returns>
        private static Dictionary<int, int> GetFrequenciesByOutcome(int numberOfDice, int numberOfSides)
        {
            int maxOutcome = (numberOfDice * numberOfSides);
            Dictionary<int, int> outcomeCounts = new Dictionary<int, int>();
            for (int i = 0; i <= maxOutcome; i++)
                outcomeCounts[i] = 0;

            foreach (int possibleOutcome in GetAllOutcomes(0, numberOfDice, numberOfSides))
                outcomeCounts[possibleOutcome] = outcomeCounts[possibleOutcome] + 1;

            //return outcomeCounts.Where(kvp => kvp.Value > 0);
            return outcomeCounts;
        }

        private static IEnumerable<int> GetAllOutcomes(int currentTotal, int nDice, int nSides)
        {
            if (nDice == 0) yield return currentTotal;
            else
            {
                for (int i = 1; i <= nSides; i++)
                    foreach (int outcome in GetAllOutcomes(currentTotal + i, nDice - 1, nSides))
                        yield return outcome;
            }
        }
    }
}

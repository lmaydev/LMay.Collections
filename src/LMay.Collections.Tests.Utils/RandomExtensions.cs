﻿namespace LMay.Collections.Tests.Utils
{
    public static class RandomExtensions
    {
        public static void Shuffle<T>(this Random random, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = random.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
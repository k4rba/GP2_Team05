using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class Rng
    {
        public static bool Bool => Random.Range(0, 2) == 0;

        /// <summary>
        /// Max is included
        /// </summary>
        public static int Next(int min, int max) => Random.Range(min, max + 1);
        public static int Next(int max) => Random.Range(0, max + 1);
        
        public static float NextF(float min, float max) => Random.Range(min, max + 1f);
        public static float NextF(float max) => Random.Range(0f, max + 1f);

        public static float RangeF(float value) => NextF(-value, value);
        
        public static bool Roll(int chance) => chance > Random.Range(0, 100);

        public static T Choose<T>(params T[] items) => items[Next(0, items.Length - 1)];

        public static T RandomItem<T>(this IList<T> items)
        {
            return items[Next(items.Count - 1)];
        }

        public static Vector3 RandomDirection => new Vector3(RangeF(1f), RangeF(1f), RangeF(1f)).normalized;
    }
}
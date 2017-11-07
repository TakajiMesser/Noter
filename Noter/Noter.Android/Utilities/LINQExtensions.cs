using System;
using System.Collections.Generic;
using System.Linq;

namespace Noter.Droid.Utilities
{
    public static class LINQExtensions
    {
        /// <summary>
        /// Finds the element with the minimum value
        /// </summary>
        /// <typeparam name="T">The type of the items within the enumerable</typeparam>
        /// <typeparam name="U">The type of the value to find the minimum of</typeparam>
        /// <param name="source">The enumerable to search within</param>
        /// <param name="func">The function to apply to each item within the enumerable, in order to find the minimum element</param>
        /// <returns>The element that yielded the minimum value</returns>
        public static T MinElement<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException("source is null");
            if (func == null) throw new ArgumentNullException("func is null");
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            T minElement = default(T);
            U minValue = default(U);
            bool hasValue = false;

            foreach (var item in source)
            {
                var currentValue = func(item);
                if (!hasValue || currentValue.CompareTo(minValue) < 0)
                {
                    minValue = func(item);
                    minElement = item;
                    hasValue = true;
                }
            }

            return minElement;
        }

        public static List<T> MinElements<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException("source is null");
            if (func == null) throw new ArgumentNullException("func is null");
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            var minElements = new List<T>();

            U minValue = default(U);
            bool hasValue = false;

            foreach (var item in source)
            {
                var currentValue = func(item);

                if (!hasValue || currentValue.CompareTo(minValue) <= 0)
                {
                    minValue = currentValue;
                    minElements.Clear();
                    hasValue = true;
                }

                if (currentValue.CompareTo(minValue) <= 0)
                {
                    minElements.Add(item);
                }
            }

            return minElements;
        }

        /// <summary>
        /// Finds the element with the maximum value
        /// </summary>
        /// <typeparam name="T">The type of the items within the enumerable</typeparam>
        /// <typeparam name="U">The type of the value to find the maximum of</typeparam>
        /// <param name="source">The enumerable to search within</param>
        /// <param name="func">The function to apply to each item within the enumerable, in order to find the maximum element</param>
        /// <returns>The element that yielded the maximum value</returns>
        public static T MaxElement<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException("source is null");
            if (func == null) throw new ArgumentNullException("func is null");
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            T maxElement = default(T);
            U maxValue = default(U);
            bool hasValue = false;

            foreach (var item in source)
            {
                var currentValue = func(item);
                if (!hasValue || currentValue.CompareTo(maxValue) > 0)
                {
                    maxValue = func(item);
                    maxElement = item;
                    hasValue = true;
                }
            }

            return maxElement;
        }

        public static List<T> MaxElements<T, U>(this IEnumerable<T> source, Func<T, U> func) where U : IComparable
        {
            if (source == null) throw new ArgumentNullException("source is null");
            if (func == null) throw new ArgumentNullException("func is null");
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            var maxElements = new List<T>();

            U maxValue = default(U);
            bool hasValue = false;

            foreach (var item in source)
            {
                var currentValue = func(item);

                if (!hasValue || currentValue.CompareTo(maxValue) > 0)
                {
                    maxValue = currentValue;
                    maxElements.Clear();
                    hasValue = true;
                }

                if (currentValue.CompareTo(maxValue) >= 0)
                {
                    maxElements.Add(item);
                }
            }

            return maxElements;
        }
    }
}
using System;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Class TaskArgumentVerificator.
    /// </summary>
    internal static class TaskArgumentVerificator
    {
        /// <summary>
        /// Check whether the item is null.
        /// </summary>
        /// <param name="item">An item to check.</param>
        /// <typeparam name="T">The first generic type parameter.</typeparam>
        public static void CheckItemIsNull<T>(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        /// <summary>
        /// Invoke the predicate.
        /// </summary>
        /// <param name="isNegative">A predicate.</param>
        /// <param name="item">An item to check.</param>
        /// <param name="message">A message.</param>
        public static void CheckIntegerMoreLess(Predicate<int> isNegative, int item, string message)
        {
            if (isNegative(item))
            {
                throw new ArgumentException(message, nameof(item));
            }
        }
    }
}

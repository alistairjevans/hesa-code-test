using System;
using System.Collections.Generic;
using System.Text;

namespace Hesa.Logic.Utilities
{
    /// <summary>
    /// Provides simple <see cref="IAsyncEnumerable{T}"/> helpers.
    /// </summary>
    public static class AsyncEnumerable
    {
        #pragma warning disable CS1998

        /// <summary>
        /// Create an empty async enumerable.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <returns>An empty async enumerable.</returns>
        public static async IAsyncEnumerable<T> Empty<T>()
        {
            yield break;
        }

        #pragma warning restore
    }
}

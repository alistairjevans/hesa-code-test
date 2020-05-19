using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hesa.Portal.Services
{
    /// <summary>
    /// Mapping extensions.
    /// </summary>
    public static class MapExtensions
    {
        /// <summary>
        /// Maps an async enumerable from one model to another, using a map function.
        /// </summary>
        /// <typeparam name="TIn">Input model type.</typeparam>
        /// <typeparam name="TOut">Output model type.</typeparam>
        /// <param name="input">The input set.</param>
        /// <param name="mapFunc">A function that takes one input element and returns an output element.</param>
        /// <returns>A new async enumerable.</returns>
        public static async IAsyncEnumerable<TOut> Map<TIn, TOut>(this IAsyncEnumerable<TIn> input, Func<TIn, TOut> mapFunc)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (mapFunc is null)
            {
                throw new ArgumentNullException(nameof(mapFunc));
            }

            await foreach (var item in input)
            {
                yield return mapFunc(item);
            }
        }
    }
}

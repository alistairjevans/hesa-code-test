using System.Collections.Generic;
using Hesa.Data;

namespace Hesa.Logic
{
    /// <summary>
    /// Provides user search functionality.
    /// </summary>
    public interface IUserSearch
    {
        /// <summary>
        /// Query the set of all users using a case-insensitive check of the last name.
        /// </summary>
        /// <param name="lastName">The last name to look for.</param>
        /// <param name="maxResults">The maximum number of results that can be returned.</param>
        /// <returns>An async enumerable that can be evaluated later.</returns>
        IAsyncEnumerable<IHesaUser> QueryUsersByLastName(string lastName, int maxResults);
    }
}

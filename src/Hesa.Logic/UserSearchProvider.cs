using System;
using System.Collections.Generic;
using System.Linq;
using Hesa.Data;
using Hesa.Logic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hesa.Logic
{
    /// <summary>
    /// Provides user search functionality.
    /// </summary>
    public class UserSearchProvider : IUserSearch
    {
        private ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSearchProvider"/> class.
        /// </summary>
        /// <param name="dbContext">The DB context.</param>
        public UserSearchProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<IHesaUser> QueryUsersByLastName(string lastName, int maxResults)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (maxResults < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxResults));
            }

            // Empty string gives zero results.
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return AsyncEnumerable.Empty<IHesaUser>();
            }

            // Use a case-insensitive LIKE on the last name; return as an AsyncEnumerable so we can stream results from the DB into the
            // ASP.NET Core serializer.
            return _dbContext.Users.Where(u => EF.Functions.Like(u.LastName, lastName))
                             .Take(maxResults)
                             .AsAsyncEnumerable();
        }
    }
}

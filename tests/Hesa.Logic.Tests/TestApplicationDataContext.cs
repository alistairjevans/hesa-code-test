using Hesa.Data;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hesa.Logic.Tests
{
    internal class TestApplicationDataContext : ApplicationDbContext
    {
        public static TestApplicationDataContext Create()
        {
            var dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseInMemoryDatabase("app", new InMemoryDatabaseRoot());

            return new TestApplicationDataContext(dbContextBuilder.Options, Options.Create(new OperationalStoreOptions()));
        }

        public TestApplicationDataContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }
    }
}

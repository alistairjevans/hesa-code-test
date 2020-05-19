using Hesa.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Hesa.Logic.Tests
{
    public class UserSearchProviderTests
    {
        [Fact]
        public void NullLastNameThowsException()
        {
            using var mockContext = TestApplicationDataContext.Create();

            var provider = new UserSearchProvider(mockContext);

            Assert.Throws<ArgumentNullException>(() => provider.QueryUsersByLastName(null!, 1));
        }

        [Fact]
        public void NegativeMaxResultsThowsException()
        {
            using var mockContext = TestApplicationDataContext.Create();

            var provider = new UserSearchProvider(mockContext);

            Assert.Throws<ArgumentOutOfRangeException>(() => provider.QueryUsersByLastName("name", -1));
        }

        [Fact]
        public async Task WhiteSpaceLastNameReturnsEmptyList()
        {
            using var mockContext = TestApplicationDataContext.Create();

            mockContext.Add(new HesaUser
            {
                LastName = "Bloggs"
            });

            var provider = new UserSearchProvider(mockContext);

            var set = await Evaluate(provider.QueryUsersByLastName("   ", 10));

            Assert.Empty(set);
        }

        [Fact]
        public async Task ReturnsMultipleUsers()
        {
            using var mockContext = TestApplicationDataContext.Create();

            mockContext.AddRange(
                new HesaUser
                {
                    LastName = "Name",
                },
                new HesaUser
                {
                    LastName = "Name",
                });

            mockContext.SaveChanges();

            var provider = new UserSearchProvider(mockContext);

            var set = await Evaluate(provider.QueryUsersByLastName("Name", 10));

            Assert.Equal(2, set.Count);
        }

        [Fact]
        public async Task CanLimitUsers()
        {
            using var mockContext = TestApplicationDataContext.Create();

            mockContext.AddRange(
                new HesaUser
                {
                    LastName = "Name",
                },
                new HesaUser
                {
                    LastName = "Name",
                });

            mockContext.SaveChanges();

            var provider = new UserSearchProvider(mockContext);

            var set = await Evaluate(provider.QueryUsersByLastName("Name", 1));

            Assert.Equal(1, set.Count);
        }

        [Fact]
        public async Task NoSuchUserEmptySet()
        {
            using var mockContext = TestApplicationDataContext.Create();

            mockContext.AddRange(
                new HesaUser
                {
                    LastName = "Name",
                });

            var provider = new UserSearchProvider(mockContext);

            var set = await Evaluate(provider.QueryUsersByLastName("NotThere", 10));

            Assert.Empty(set);
        }

        private static async Task<IReadOnlyList<IHesaUser>> Evaluate(IAsyncEnumerable<IHesaUser> asyncEnumerable)
        {
            var list = new List<IHesaUser>();

            await foreach (var item in asyncEnumerable)
            {
                list.Add(item);
            }

            return list;
        }
    }
}

using System.Threading.Tasks;

namespace Hesa.Logic
{
    /// <summary>
    /// Provides the service for generating test data.
    /// </summary>
    public interface ISeedData
    {
        /// <summary>
        /// Generate Seed Data (if required).
        /// </summary>
        /// <returns>An awaitable task.</returns>
        Task SeedTestDataAsync();
    }
}

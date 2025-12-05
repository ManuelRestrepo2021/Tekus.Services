using Microsoft.EntityFrameworkCore;
using Tekus.Services.Infrastructure.Persistence;

namespace Tekus.Services.Tests.TestHelpers
{
    public static class DbContextFactory
    {
        public static AppDbContext CreateInMemoryContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new AppDbContext(options);
        }
    }
}
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Infrastructure.Persistence;
using Tekus.Services.Infrastructure.Services;
using Tekus.Services.Tests.TestHelpers;
using Xunit;

namespace Tekus.Services.Tests.Services
{
    public class ProviderServiceTests
    {
        private AppDbContext CreateContext(string dbName)
        {
            // Usa el helper DbContextFactory definido en Tekus.Services.Tests.TestHelpers
            return DbContextFactory.CreateInMemoryContext(dbName);
        }

        private ProviderService CreateService(AppDbContext context)
        {
            return new ProviderService(context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldNotThrow_WhenProviderExists()
        {
            // Arrange
            var dbName = nameof(GetByIdAsync_ShouldNotThrow_WhenProviderExists);
            using var context = CreateContext(dbName);

            var provider = new Domain.Entities.Provider
            {
                Name = "Proveedor 1",
                Email = "test@provider.com",
                CountryId = 1
            };

            context.Providers.Add(provider);
            await context.SaveChangesAsync();

            var service = CreateService(context);

            // Act
            Func<Task> act = async () => { await service.GetByIdAsync(provider.Id); };

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var dbName = nameof(GetByIdAsync_ShouldReturnNull_WhenNotExists);
            using var context = CreateContext(dbName);
            var service = CreateService(context);

            // Act
            var result = await service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProvider()
        {
            // Arrange
            var dbName = nameof(CreateAsync_ShouldCreateProvider);
            using var context = CreateContext(dbName);
            var service = CreateService(context);

            var dto = new ProviderUpsertDto
            {
                Name = "Nuevo Proveedor",
                Email = "nuevo@provider.com",
                CountryId = 1,
                // Si tu dto tiene más campos (ServicesIds, etc.), agrégales valores apropiados
            };

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be(dto.Name);

            var providerInDb = await context.Providers.SingleOrDefaultAsync(p => p.Id == result.Id);
            providerInDb.Should().NotBeNull();
            providerInDb!.Name.Should().Be(dto.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenProviderDoesNotExist()
        {
            // Arrange
            var dbName = nameof(DeleteAsync_ShouldReturnFalse_WhenProviderDoesNotExist);
            using var context = CreateContext(dbName);
            var service = CreateService(context);

            // Act
            var result = await service.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenProviderExists()
        {
            // Arrange
            var dbName = nameof(DeleteAsync_ShouldReturnTrue_WhenProviderExists);
            using var context = CreateContext(dbName);

            var provider = new Domain.Entities.Provider
            {
                Name = "Proveedor a eliminar",
                Email = "delete@provider.com",
                CountryId = 1
            };

            context.Providers.Add(provider);
            await context.SaveChangesAsync();

            var service = CreateService(context);

            // Act
            var result = await service.DeleteAsync(provider.Id);

            // Assert
            result.Should().BeTrue();

            var providerInDb = await context.Providers.FindAsync(provider.Id);
            providerInDb.Should().BeNull();
        }
    }
}
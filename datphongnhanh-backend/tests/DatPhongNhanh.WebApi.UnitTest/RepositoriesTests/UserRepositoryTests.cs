using DatPhongNhanh.Data.DbContexts;
using DatPhongNhanh.Data.Entities.Identity;
using DatPhongNhanh.Data.Repositories;
using DatPhongNhanh.Data.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DatPhongNhanh.WebApi.UnitTest.RepositoriesTests
{
    public class UserRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DatPhongNhanh")
                .Options;
            _context = new AppDbContext(options);
            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_AddUser()
        {
            // Arrange
            var user = new UserEntity
            {
                Id = 1,
                UserName = "test",
                Email = "test@test.com",
                Password = "password"
            };

            // Act
            await _userRepository.AddAsync(user);
            await _context.SaveChangesAsync();
            var result = await _context.Users.FindAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(user.Email);
            result.Password.Should().Be(user.Password);
        }
    }
}

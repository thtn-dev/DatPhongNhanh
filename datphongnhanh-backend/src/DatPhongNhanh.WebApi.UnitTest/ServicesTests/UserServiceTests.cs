using DatPhongNhanh.BusinessLogic.Services;
using DatPhongNhanh.BusinessLogic.Services.Interfaces;
using DatPhongNhanh.Data.Entities.Identity;
using DatPhongNhanh.Data.Repositories.Interfaces;
using DatPhongNhanh.SharedKernel;
using FluentAssertions;
using Moq;

namespace DatPhongNhanh.WebApi.UnitTest.ServicesTests
{
    public class UserServiceTests : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userService = new UserService(_userRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_WhenUserIsValid_ShouldReturnTrue()
        {
            // Arrange
            var user = new UserEntity
            {
                UserName = "TestUser",
                Password = "TestPassword",
                Email = "test@dtn.com"
            };

            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<UserEntity>()))
                .Returns(Task.CompletedTask);
            CancellationToken cancellationToken = new();
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1);
            // Act
            var result = await _userService.CreateUserAsync(user);

            // Assert
            result.Should().BeTrue();
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetUserByIdAsync_WhenUserIdIsInValid_ShouldReturnNull(long id)
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.FindByIdAsync(id))
                .ReturnsAsync((UserEntity?)null);
            // Act
            var result = await _userService.GetUserByIdAsync(id);
            // Assert
            result.Should().BeNull();
            _userRepositoryMock.Verify(x => x.FindByIdAsync(id), Times.Once);
        }
    }

}

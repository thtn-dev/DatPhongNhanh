using DatPhongNhanh.Application.Common.Interfaces;
using DatPhongNhanh.Domain.User;
using DatPhongNhanh.Domain.User.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatPhongNhanh.Infrastructure.User;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserNormalize _userNormalize;

    public UserService(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<IApplicationDbContext>() 
            ?? throw new ArgumentNullException(nameof(IApplicationDbContext));

        _passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>() 
            ?? throw new ArgumentNullException(nameof(IPasswordHasher));

        _userNormalize = serviceProvider.GetRequiredService<IUserNormalize>() 
            ?? throw new ArgumentNullException(nameof(IUserNormalize));
    }
    public IQueryable<UserEntity> Users => _context.Users.AsQueryable();

    public async Task<ErrorOr<UserEntity>> CreateUserAsync(UserEntity user, string password)
    {
        var hashedPassword = _passwordHasher.HashPassword(password);
        user.PasswordHash = hashedPassword;
        user.NormalizedEmail = _userNormalize.NormalizeEmail(user.Email);
        user.NormalizedUserName = _userNormalize.NormalizeUserName(user.UserName);
        await _context.Users.AddAsync(user).ConfigureAwait(false);
        var rowAff = await _context.SaveChangesAsync().ConfigureAwait(false);
        if (rowAff == 0)
        {
            return Error.Conflict("User already exists");
        }
        return user;
    }

    public Task DeleteUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        var emailNormalized = _userNormalize.NormalizeEmail(email);
        var user = await Users.FirstOrDefaultAsync(u => u.NormalizedEmail == emailNormalized).ConfigureAwait(false);
        return user;
    }

    public async Task<UserEntity?> GetUserByIdAsync(long userId)
    {
        return await Users.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName)
    {
        var userNameNormalized = _userNormalize.NormalizeUserName(userName);
        return await Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userNameNormalized).ConfigureAwait(false);
    }

    public async Task<ErrorOr<UserEntity>> SignInAsync(string userName, string password)
    {
        var user = await GetUserByUserNameAsync(userName).ConfigureAwait(false);
        if (user == null)
        {
            return Error.NotFound("User not found");
        }
        var isVerified = _passwordHasher.VerifyPassword(password, user.PasswordHash);
        return isVerified ? user : Error.Validation("Invalid password");
    }


    public Task<UserEntity> UpdateUserAsync(Guid userId, string email, string password)
    {
        throw new NotImplementedException();
    }
}
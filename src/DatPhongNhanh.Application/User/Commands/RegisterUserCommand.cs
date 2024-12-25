
using DatPhongNhanh.Application.Common.Validators;
using DatPhongNhanh.Domain.Common.Services;
using DatPhongNhanh.Domain.User;
using DatPhongNhanh.Domain.User.Services;
using DatPhongNhanh.Shared;
using System.ComponentModel.DataAnnotations;

namespace DatPhongNhanh.Application.User.Commands;

public sealed record RegisterUserCommand : ICommand<ErrorOr<Success>>
{
    [Required]
    public string Email { get; init; } = null!;
    [Required]
    public string UserName { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;
    [Required]
    public string ConfirmPassword { get; init; } = null!;
}

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters")
            .MaximumLength(255).WithMessage("Username must not exceed 255 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull().WithMessage("Email is required")
            .EmailAddress();

        RuleFor(x => x.Password)
            .Password()
            .Equal(x => x.ConfirmPassword).WithMessage("\"Password and\" \"Confirm Password\" must match");
    }
}

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, ErrorOr<Success>>
{
    private readonly IUserService _userService;
    private readonly ISystemIdGenService _systemIdGenService;
    public RegisterUserCommandHandler(IUserService userService, ISystemIdGenService systemIdGenService)
    {
        _userService = userService;
        _systemIdGenService = systemIdGenService;
    }
    public async Task<ErrorOr<Success>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = UserEntity.Create(_systemIdGenService.GenerateId<long>(),request.UserName, request.Email);
        var result = await _userService.CreateUserAsync(user, request.Password);
        return result.Value != null ? Result.Success : result.Errors;
    }
}

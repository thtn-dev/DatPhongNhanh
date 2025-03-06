using FluentValidation;

namespace DatPhongNhanh.BusinessLogic.User.Dtos;

public class UserLoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}


public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
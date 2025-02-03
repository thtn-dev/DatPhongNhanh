namespace DatPhongNhanh.Application.Common.Validators;

public static class ValidatorExtensions
{
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var options = ruleBuilder
            .NotEmpty()
            .NotNull().WithMessage("Password is required")
            .Matches(@"^(?=.*[A-Za-z\d])(?=.*\d)[A-Za-z\d@$!%*#?&]{4,255}$")
            .WithMessage("Passwords have a length of 4 - 25, must contain at least one letter, one number and special character");

        return options;
    }
}
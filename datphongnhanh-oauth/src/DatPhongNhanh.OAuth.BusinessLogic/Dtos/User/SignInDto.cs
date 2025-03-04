namespace DatPhongNhanh.OAuth.Business.Dtos.User;
public sealed class SignInDto
{
    public required string Password { get; set; }
    public required string EmailOrUsername { get; set; }
    public bool RememberMe { get; set; }
}
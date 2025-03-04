namespace DatPhongNhanh.OAuth.Business.Dtos.User;

public class SignUpDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
    public required string PasswordConfirmation { get; set; }
}
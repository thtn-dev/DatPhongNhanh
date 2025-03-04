using System.Text.RegularExpressions;

namespace DatPhongNhanh.OAuth.Business.Regex;

public static partial class UserRegex
{
    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    public static partial System.Text.RegularExpressions.Regex EmailRegex();
    
    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{4,}$")]
    public static partial System.Text.RegularExpressions.Regex DevelopmentPasswordRegex();
}
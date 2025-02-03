using DatPhongNhanh.Shared;
using System.Text.RegularExpressions;

namespace DatPhongNhanh.Domain.Common.ValueObjects;
public partial class Email : ValueObject
{
    public string Value { get; }
    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be empty", nameof(value));
        }

        if (!EmailRegex().IsMatch(value))
        {
            throw new ArgumentException("Email address is invalid", nameof(value));
        }

        Value = value;
    }

    public static Email Create(string email) => new(email);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    [GeneratedRegex(@"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$")]
    private static partial Regex EmailRegex();

    public Regex GetEmailRegex() => EmailRegex();

}
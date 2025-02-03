using DatPhongNhanh.Shared;
using System.Text.RegularExpressions;

namespace DatPhongNhanh.Domain.Common.ValueObjects
{
    public partial class UserName : ValueObject
    {
        public string Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private UserName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("User name cannot be empty", nameof(value));
            }

            if (!UserNameRegex().IsMatch(value))
            {
                throw new ArgumentException("User name is invalid", nameof(value));
            }
            Value = value;
        }

        public static UserName Create(string value) => new(value);


        [GeneratedRegex(@"^(?=[a-zA-Z0-9._]{5,20}$)(?!.*[_.]{2})[^_.].*[^_.]$")]
        private static partial Regex UserNameRegex();

        public static Regex GetUserNameRegex() => UserNameRegex();
    }
}

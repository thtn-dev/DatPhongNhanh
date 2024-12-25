

namespace DatPhongNhanh.Domain.Common.Options
{
    public class JwtSetting
    {
        public const string SettingKey = nameof(JwtSetting);
        public required string Secret { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}

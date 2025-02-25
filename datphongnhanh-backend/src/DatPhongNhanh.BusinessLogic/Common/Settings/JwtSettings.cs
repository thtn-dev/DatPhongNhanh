namespace DatPhongNhanh.BusinessLogic.Common.Settings
{
    public class JwtSettings
    {
        public const string SettingKey = nameof(JwtSettings);
        public required string Secret { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}

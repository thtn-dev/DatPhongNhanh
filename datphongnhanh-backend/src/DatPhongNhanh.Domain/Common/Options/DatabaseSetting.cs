namespace DatPhongNhanh.Domain.Common.Options
{
    public class DatabaseSetting
    {
        public const string SettingKey = nameof(DatabaseSetting);
        public string ConnectionString { get; set; } = string.Empty;
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? Database { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? SslMode { get; set; }
        public bool TrustServerCertificate { get; set; }
    }

}

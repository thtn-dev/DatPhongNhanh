namespace DatPhongNhanh.Domain.Common.Constants;
public static class DbTypeConstants
{
    public const string VARCHAR32 = "varchar(32)";
    public const string VARCHAR64 = "varchar(64)";
    public const string VARCHAR128 = "varchar(128)";
    public const string VARCHAR256 = "varchar(256)";
    public const string VARCHAR512 = "varchar(512)";

    public const string PhoneNumber = "varchar(20)";
    public const string Email = "varchar(256)";
    public const string Guid = "varchar(36)";

    public const string TEXT = "text";
    public const string LONGTEXT = "longtext";

    public const string INT = "int";
    public const string BIGINT = "bigint";
    public const string DECIMAL = "decimal(18,2)";
    public const string DATETIME = "datetime";
    public const string DATE = "date";
    public const string TIME = "time";
    public const string BOOLEAN = "bit";
    public const string JSON = "json";
    public const string JSONB = "jsonb";
    public const string UUID = "uuid";
    public const string ENUM = "enum";
    public const string BLOB = "blob";
    public const string GEOMETRY = "geometry";
    // geometry(Point, 3857)
    public const string GEOMETRY_POINT_3857 = "geometry(Point, 3857)";

    public const string DOUBLE_PRECISION = "double precision";
}

public static class DbSchemaConstants
{
    public const string Default = "public";
}
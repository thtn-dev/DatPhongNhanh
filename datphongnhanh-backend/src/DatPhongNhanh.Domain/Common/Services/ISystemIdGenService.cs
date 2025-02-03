namespace DatPhongNhanh.Domain.Common.Services
{
    public interface ISystemIdGenService
    {
        string GenerateStringId();
        long GenerateLongId();

        T GenerateId<T>();
    }
}

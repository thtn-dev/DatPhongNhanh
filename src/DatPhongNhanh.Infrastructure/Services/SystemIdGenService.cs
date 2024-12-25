

using DatPhongNhanh.Domain.Common.Services;
using IdGen;

namespace DatPhongNhanh.Infrastructure.Services
{
    public class SystemIdGenService : ISystemIdGenService
    {
        private readonly IIdGenerator<long> _longIdGenerator;
        public SystemIdGenService(IIdGenerator<long> longIdGenerator)
        {
            _longIdGenerator = longIdGenerator;
        }
        public long GenerateLongId()
        {
            return _longIdGenerator.CreateId();
        }

        public string GenerateStringId()
        {
            return Guid.NewGuid().ToString();
        }

        public T GenerateId<T>()
        {
            return typeof(T) switch
            {
                Type t when t == typeof(long) => (T)Convert.ChangeType(GenerateLongId(), typeof(T)),
                Type t when t == typeof(string) => (T)Convert.ChangeType(GenerateStringId(), typeof(T)),
                _ => throw new NotSupportedException(),
            };
        }
    }
}

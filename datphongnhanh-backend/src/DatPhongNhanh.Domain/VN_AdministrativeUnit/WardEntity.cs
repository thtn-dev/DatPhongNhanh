

using DatPhongNhanh.Shared.Entities;

namespace DatPhongNhanh.Domain.VN_AdministrativeUnit
{
    public class WardEntity : EntityBase<string>
    {
        public string Name { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string FullNameEn { get; set; } = null!;
        public string CodeName { get; set; } = null!;
        public string DistrictCode { get; set; } = null!;
        public int AdministrativeUnitId { get; set; }
        public DistrictEntity District { get; set; } = null!;
        public AdministrativeUnitEntity AdministrativeUnit { get; set; } = null!;
    }
}

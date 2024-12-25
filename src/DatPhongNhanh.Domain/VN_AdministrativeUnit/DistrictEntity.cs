using DatPhongNhanh.Shared.Entities;

namespace DatPhongNhanh.Domain.VN_AdministrativeUnit
{
    public class DistrictEntity : EntityBase<string>
    {
        public string Name { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string FullNameEn { get; set; } = null!;
        public string CodeName { get; set; } = null!;
        public string ProvinceCode { get; set; } = null!;
        public int AdministrativeUnitId { get; set; }
        public ProvinceEntity Province { get; set; } = null!;
        public AdministrativeUnitEntity AdministrativeUnit { get; set; } = null!;
        public virtual ICollection<WardEntity> Wards { get; set; } = null!;
    }
}

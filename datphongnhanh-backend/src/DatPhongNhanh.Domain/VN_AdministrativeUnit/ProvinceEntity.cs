using DatPhongNhanh.Shared.Entities;

namespace DatPhongNhanh.Domain.VN_AdministrativeUnit
{
    public class ProvinceEntity : EntityBase<string>
    {
        public string Name { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string FullNameEn { get; set; } = null!;
        public string CodeName { get; set; } = null!;
        public int AdministrativeUnitId { get; set; }
        public int AdministrativeRegionId { get; set; }
        public AdministrativeUnitEntity AdministrativeUnit { get; set; } = null!;
        public AdministrativeRegionEntity AdministrativeRegion { get; set; } = null!;
        public virtual ICollection<DistrictEntity> Districts { get; set; } = null!;
    }
}

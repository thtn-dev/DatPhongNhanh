using DatPhongNhanh.Shared.Entities;

namespace DatPhongNhanh.Domain.VN_AdministrativeUnit
{
    public class AdministrativeUnitEntity : EntityBase<int>
    {
        public string FullName { get; set; } = null!;
        public string FullNameEn { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string ShortNameEn { get; set; } = null!;
        public string CodeName { get; set; } = null!;
        public string CodeNameEn { get; set; } = null!;
        public virtual ICollection<ProvinceEntity> Provinces { get; set; } = null!;
        public virtual ICollection<DistrictEntity> Districts { get; set; } = null!;
        public virtual ICollection<WardEntity> Wards { get; set; } = null!;
    }
}

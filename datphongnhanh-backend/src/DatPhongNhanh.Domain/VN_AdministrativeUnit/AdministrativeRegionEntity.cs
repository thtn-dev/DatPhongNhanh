using DatPhongNhanh.Shared.Entities;

namespace DatPhongNhanh.Domain.VN_AdministrativeUnit
{
    public class AdministrativeRegionEntity : EntityBase<int>
    {
        public string Name { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string CodeName { get; set; } = null!;
        public string CodeNameEn { get; set; } = null!;
        public virtual ICollection<ProvinceEntity> Provinces { get; set; } = null!;
    }
}

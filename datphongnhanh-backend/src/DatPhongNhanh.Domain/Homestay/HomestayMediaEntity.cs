

using DatPhongNhanh.Shared.Entities;

namespace DatPhongNhanh.Domain.Homestay
{
    public class HomestayMediaEntity : EntityBase<long>
    {
        public string Url { get; set; } = null!;
        public string AltText { get; set; } = null!;
        public string Caption { get; set; } = null!;
        public HomestayEntity Homestay { get; set; } = null!;
        public long HomestayId { get; set; }
       
    }
}

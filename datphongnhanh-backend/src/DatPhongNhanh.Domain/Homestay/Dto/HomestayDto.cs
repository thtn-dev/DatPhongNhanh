
namespace DatPhongNhanh.Domain.Homestay.Dto
{
    public class HomestayDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}

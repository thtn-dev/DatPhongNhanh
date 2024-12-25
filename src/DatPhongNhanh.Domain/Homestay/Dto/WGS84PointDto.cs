

namespace DatPhongNhanh.Domain.Homestay.Dto
{
    public class WGS84PointDto
    {
        private double _longitude;
        private double _latitude;
        // check is valid WGS84 point longitude and latitude
        public double Longitude
        {
            get => _longitude;
            set
            {
                if (ValidateLongitude(value))
                {
                    throw new ArgumentException("Invalid longitude value", nameof(Longitude));
                }
                _longitude = value;
            }
        }
        public double Latitude
        {
            get => _latitude;
            set
            {
                if (ValidateLatitude(value))
                {
                    throw new ArgumentException("Invalid latitude value", nameof(Latitude));
                }
                _latitude = value;
            }
        }

        public static bool ValidateLongitude(double longitude)
        {
            return longitude >= -180 && longitude <= 180;
        }

        public static bool ValidateLatitude(double latitude)
        {
            return latitude >= -90 && latitude <= 90;
        }
    }
}

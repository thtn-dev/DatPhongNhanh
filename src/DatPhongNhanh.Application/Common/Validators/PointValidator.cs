using DatPhongNhanh.Domain.Homestay.Dto;

namespace DatPhongNhanh.Application.Common.Validators
{
    public static class PointValidator
    {
        public static IRuleBuilder<T, WGS84PointDto> WGS84Point<T>(this IRuleBuilder<T, WGS84PointDto> ruleBuilder)
        {
            var builder = ruleBuilder
                .NotEmpty()
                .NotNull().WithMessage("Point is required")
                .Must(x => WGS84PointDto.ValidateLongitude(x.Longitude))
                .WithMessage("Longitude must be between -180 and 180")
                .Must(x => WGS84PointDto.ValidateLatitude(x.Latitude))
                .WithMessage("Latitude must be between -90 and 90");

            return builder;
        }
    }
}

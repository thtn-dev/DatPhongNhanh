using DatPhongNhanh.Application.Common.Validators;
using DatPhongNhanh.Domain.Common.Services;
using DatPhongNhanh.Domain.Common.Utils;
using DatPhongNhanh.Domain.Homestay;
using DatPhongNhanh.Domain.Homestay.Dto;
using DatPhongNhanh.Domain.Homestay.Services;
using DatPhongNhanh.Shared;
using NetTopologySuite.Geometries;

namespace DatPhongNhanh.Application.Homestay.Commands;

public sealed class CreateHomestayCommand : ICommand<ErrorOr<bool>>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Address { get; set; } = null!;
    public WGS84PointDto WGS84LocationPoint { get; set; } = null!;
}

public class CreateHomestayCommandValidator : AbstractValidator<CreateHomestayCommand>
{
    public CreateHomestayCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Address)
            .NotEmpty();
        // rule for longitude and latitude is srid 4326
        RuleFor(x => x.WGS84LocationPoint).WGS84Point();
    }
}

public sealed class CreateHomestayCommandHandler : ICommandHandler<CreateHomestayCommand, ErrorOr<bool>>
{
    private readonly ISystemIdGenService _systemIdGenService;
    private readonly IHomestayService _homestayService;

    public CreateHomestayCommandHandler(ISystemIdGenService systemIdGenService, IHomestayService homestayService)
    {
        _systemIdGenService = systemIdGenService;
        _homestayService = homestayService;
    }

    public async Task<ErrorOr<bool>> Handle(CreateHomestayCommand request, CancellationToken cancellationToken)
    {
        var homestay = HomestayEntity.Create(_systemIdGenService.GenerateLongId(), request.Name, request.Description);

        var _4326Point = new Point(request.WGS84LocationPoint.Longitude, request.WGS84LocationPoint.Latitude) { SRID = 4326 };
        var _3857Point = CoordinateConverter.ConvertWGS84ToWebMercator(_4326Point);
        HomestayEntity.SetAddress(homestay, request.Address, _3857Point);

        await _homestayService.CreateHomstayAsync(homestay, cancellationToken).ConfigureAwait(false);
        return true;
    }
}

using Ardalis.GuardClauses;
using DatPhongNhanh.Domain.Homestay.Events;
using DatPhongNhanh.Shared.Entities;
using NetTopologySuite.Geometries;

namespace DatPhongNhanh.Domain.Homestay;
public class HomestayEntity : EntityAggregateBase<long>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ExtraData { get; set; }
    public double Longtitude { get; set; }
    public double Latitude { get; set; }
    public Geometry? Geom { get; set; }
    public string RawAddress { get; set; } = null!;
    public HomestayEntity()
    {
    }

    public static HomestayEntity Create(long id, string name, string? description)
    {
        var homestay = new HomestayEntity
        {
            Id = id,
            Name = name,
            Description = description
        };
        homestay.RegisterDomainEvent(new CreateHomestayEvent(homestay.Id));
        return homestay;
    }

    public static void SetAddress(HomestayEntity homestay, string address, Point _3857Point)
    {
        Guard.Against.Null(homestay, nameof(homestay));
        Guard.Against.NullOrWhiteSpace(address, nameof(address));
        Guard.Against.Null(_3857Point, nameof(_3857Point));
        if (_3857Point.SRID != 3857)
        {
            throw new ArgumentException("Invalid SRID for 3857", nameof(_3857Point));
        }

        homestay.RawAddress = address;
        homestay.Longtitude = _3857Point.Coordinate.X;
        homestay.Latitude = _3857Point.Coordinate.Y;
        homestay.Geom = _3857Point;
    }
}
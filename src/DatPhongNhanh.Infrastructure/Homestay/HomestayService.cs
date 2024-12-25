using DatPhongNhanh.Application.Common.Interfaces;
using DatPhongNhanh.Domain.Homestay;
using DatPhongNhanh.Domain.Homestay.Dto;
using DatPhongNhanh.Domain.Homestay.Services;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace DatPhongNhanh.Infrastructure.Homestay
{
    public class HomestayService : IHomestayService
    {
        private readonly IApplicationDbContext _appContext;
        public HomestayService(IApplicationDbContext appContext)
        {
            _appContext = appContext;
        }
        public async Task<int> CreateHomstayAsync(HomestayEntity homestay, CancellationToken cancellation = default)
        {
            await _appContext.Homestays.AddAsync(homestay, cancellation).ConfigureAwait(false);
            return await _appContext.SaveChangesAsync(cancellation).ConfigureAwait(false);
        }

        public async Task<List<HomestayEntity>> FindHomestayNearest(double longitude, double latitude, double tolerance, CancellationToken cancellation = default)
        {
            var point = new Point(longitude, latitude) { SRID = 3857 };
            var q = _appContext.Homestays
                .Where(h => h.Geom != null && h.Geom.IsWithinDistance(point, tolerance))
                .Select(h=> new HomestayEntity
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    Geom = h.Geom
                });
            var r = await q.ToListAsync(cancellation).ConfigureAwait(false);

            return r;
        }

        public async Task<HomestayEntity?> GetHomestayByIdAsync(long id , CancellationToken cancellation = default)
        {
            var result = await _appContext.Homestays.
                Where(h => h.Id == id).Select(h => new
                {
                    h.Id,
                    h.Name,
                    h.Description,
                    h.Geom
                }).Select(h => new HomestayEntity
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    Geom = h.Geom
                }).FirstOrDefaultAsync(cancellation).ConfigureAwait(false);
            return result;
        }
    }
}

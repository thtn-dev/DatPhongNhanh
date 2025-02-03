

using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace DatPhongNhanh.Domain.Common.Utils
{
    public static class CoordinateConverter
    {

        public static Point ConvertWGS84ToWebMercator(double longitude, double latitude)
        {
            var sourceCS = GeographicCoordinateSystem.WGS84;
            var targetCS = ProjectedCoordinateSystem.WebMercator;
            var ctFactory = new CoordinateTransformationFactory();
            var transformation = ctFactory.CreateFromCoordinateSystems(sourceCS, targetCS);

            double[] fromPoint = [longitude, latitude];
            double[] toPoint = transformation.MathTransform.Transform(fromPoint);

            return new Point(toPoint[0], toPoint[1]) { SRID = 3857 };
        }

        public static Point ConvertWGS84ToWebMercator(Point point)
        {
            // check if the point is in WGS84
            if (point.SRID != 4326)
            {
                throw new ArgumentException("Invalid SRID for WGS84", nameof(point));
            }
            return ConvertWGS84ToWebMercator(point.Coordinate.X, point.Coordinate.Y);
        }

        public static Point ConvertWebMercatorToWGS84(double x, double y)
        {
            var sourceCS = ProjectedCoordinateSystem.WebMercator;
            var targetCS = GeographicCoordinateSystem.WGS84;
            var ctFactory = new CoordinateTransformationFactory();
            var transformation = ctFactory.CreateFromCoordinateSystems(sourceCS, targetCS);

            double[] fromPoint = [x, y];
            double[] toPoint = transformation.MathTransform.Transform(fromPoint);

            return new Point(toPoint[0], toPoint[1]) { SRID = 4326 };
        }

        public static Point ConvertWebMercatorToWGS84(Point point)
        {
            // check if the point is in WebMercator
            if (point.SRID != 3857)
            {
                throw new ArgumentException("Invalid SRID for WebMercator", nameof(point));
            }
            return ConvertWebMercatorToWGS84(point.Coordinate.X, point.Coordinate.Y);
        }
    }
}

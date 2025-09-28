using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace MedSolutions.Infrastructure.Data.ValueGenerators;

public class PointValueGenerator : ValueGenerator<Point>
{
    public override bool GeneratesTemporaryValues => false;

    public override Point Next(EntityEntry entry)
    {
        dynamic entity = entry.Entity;

        double? lat = entity.Latitude;
        double? lng = entity.Longitude;

        GeometryFactory factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        Point fallbackPoint = factory.CreatePoint(new Coordinate(0, 0));

        return !lat.HasValue || !lng.HasValue
            ? fallbackPoint
            : lat < -90 || lat > 90 || lng < -180 || lng > 180 ? fallbackPoint : factory.CreatePoint(new Coordinate(lng.Value, lat.Value));
    }

}


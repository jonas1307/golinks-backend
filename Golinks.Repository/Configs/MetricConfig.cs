using Golinks.Domain.Entities;
using MongoDB.Bson.Serialization;

namespace Golinks.Repository.Configs;

public static class MetricConfig
{
    public static void Apply()
    {
        BsonClassMap.RegisterClassMap<Link>(cm =>
        {
            cm.AutoMap();
        });
    }
}

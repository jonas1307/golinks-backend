using Golinks.Domain.Entities;
using MongoDB.Bson.Serialization;

namespace Golinks.Repository.Configs;

public static class LinkConfig
{
    public static void Apply()
    {
        BsonClassMap.RegisterClassMap<Link>(cm =>
        {
            cm.MapIdField(c => c.Id);
            cm.AutoMap();
        });
    }
}

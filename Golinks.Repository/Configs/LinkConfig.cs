using Golinks.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Golinks.Repository.Configs;

public static class LinkConfig
{
    public static void Apply()
    {
        BsonClassMap.RegisterClassMap<Link>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(c => c.Id)
                .SetIdGenerator(new StringObjectIdGenerator())
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });
    }
}

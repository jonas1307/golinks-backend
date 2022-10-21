using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Golinks.Repository.Extensions.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Golinks.Repository.Repositories;

public class LinkRepository : RepositoryBase<Link>, ILinkRepository
{
    public LinkRepository(IMongoDbSettings settings) : base(settings)
    { }

    public virtual Task<Link> FindByIdAsync(string id)
    {
        return Task.Run(() =>
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Link>.Filter.Eq(doc => new ObjectId(doc.Id), objectId);
            return _collection.Find(filter).SingleOrDefaultAsync();
        });
    }
}

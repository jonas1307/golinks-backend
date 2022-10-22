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
}

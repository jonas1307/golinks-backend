using Golinks.Domain.Entities;

namespace Golinks.Repository.Contracts;

public interface ILinkRepository : IRepositoryBase<Link>
{
    Task<Link> FindByIdAsync(string id);
}
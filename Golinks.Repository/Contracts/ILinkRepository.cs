using Golinks.Domain.Entities;

namespace Golinks.Repository.Contracts;

public interface ILinkRepository
{
    Task<Link> FindByIdAsync(string id);
}
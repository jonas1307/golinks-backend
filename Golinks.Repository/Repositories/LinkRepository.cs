using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;

namespace Golinks.Repository.Repositories;

public class LinkRepository(GolinksContext context) : RepositoryBase<Link>(context), ILinkRepository
{
}

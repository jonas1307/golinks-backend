using Golinks.Application.Contracts;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;

namespace Golinks.Application.Services;

public class LinkService(IBaseRepository<Link> repository) : BaseService<Link>(repository), ILinkService
{
}

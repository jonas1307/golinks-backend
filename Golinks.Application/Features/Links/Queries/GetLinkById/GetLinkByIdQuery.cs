using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetLinkById;

public record GetLinkByIdQuery(Guid Id) : IRequest<Result<LinkViewModel>>;

using Golinks.Application.Common;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetLinkQrCode;

public record GetLinkQrCodeQuery(Guid Id, string BaseUrl) : IRequest<Result<byte[]>>;

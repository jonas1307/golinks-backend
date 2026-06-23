using Golinks.Application.Common;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace Golinks.Application.Features.Links.Queries.GetLinkQrCode;

public class GetLinkQrCodeHandler(GolinksContext context) : IRequestHandler<GetLinkQrCodeQuery, Result<byte[]>>
{
    public async Task<Result<byte[]>> Handle(GetLinkQrCodeQuery request, CancellationToken cancellationToken)
    {
        var slug = await context.Links
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => x.Slug)
            .FirstOrDefaultAsync(cancellationToken);

        if (slug is null)
            return Error.NotFound($"Link with ID {request.Id} was not found.");

        var redirectUrl = $"{request.BaseUrl.TrimEnd('/')}/{slug}";

        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(redirectUrl, QRCodeGenerator.ECCLevel.Q);

        return new PngByteQRCode(data).GetGraphic(20);
    }
}

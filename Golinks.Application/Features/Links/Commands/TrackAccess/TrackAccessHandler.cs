using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Domain.Entities;
using Golinks.Repository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Golinks.Application.Features.Links.Commands.TrackAccess;

public class TrackAccessHandler(GolinksContext context) : IRequestHandler<TrackAccessCommand, Result<LinkResponse>>
{
    public async Task<Result<LinkResponse>> Handle(TrackAccessCommand request, CancellationToken cancellationToken)
    {
        var link = await context.Links
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

        if (link == null)
            return Error.NotFound("No link was found with the given slug.");

        await context.BeginTransactionAsync();

        try
        {
            await context.Links
                .Where(x => x.Slug == request.Slug)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.TotalUsage, l => l.TotalUsage + 1), cancellationToken);

            context.Metrics.Add(new Metric
            {
                LinkId = link.Id,
                UserAgent = request.UserAgent,
                Referrer = request.Referrer,
                IpHash = HashIp(request.IpAddress)
            });

            await context.SaveChangesAsync(cancellationToken);
            await context.CommitAsync();

            link.TotalUsage += 1;

            return link.Adapt<LinkResponse>();
        }
        catch
        {
            await context.RollbackAsync();
            throw;
        }
    }

    private static string? HashIp(string? ip)
        => ip is null ? null : Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(ip))).ToLower();
}

using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Actions.Commands.RegisterAccess;

public class RegisterAccessHandler(GolinksContext context, IMapper mapper) : IRequestHandler<RegisterAccessCommand, RestResponse<LinkViewModel>>
{
    public async Task<RestResponse<LinkViewModel>> Handle(RegisterAccessCommand request, CancellationToken cancellationToken)
    {
        var link = await context.Links
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

        if (link == null)
            return RestResponse<LinkViewModel>.Error("No link was found with the given slug.");

        await context.BeginTransactionAsync();

        try
        {
            await context.Links
                .Where(x => x.Slug == request.Slug)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.TotalUsage, l => l.TotalUsage + 1), cancellationToken);

            context.Metrics.Add(new Metric { LinkId = link.Id });
            await context.SaveChangesAsync(cancellationToken);

            await context.CommitAsync();

            link.TotalUsage += 1;

            return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(link));
        }
        catch
        {
            await context.RollbackAsync();
            throw;
        }
    }
}

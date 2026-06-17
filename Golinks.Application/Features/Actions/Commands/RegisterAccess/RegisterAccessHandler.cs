using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using MediatR;

namespace Golinks.Application.Features.Actions.Commands.RegisterAccess;

public class RegisterAccessHandler(ILinkRepository linkRepository, IMetricRepository metricRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<RegisterAccessCommand, RestResponse<LinkViewModel>>
{
    public async Task<RestResponse<LinkViewModel>> Handle(RegisterAccessCommand request, CancellationToken cancellationToken)
    {
        var link = await linkRepository.FindOneAsync(x => x.Slug == request.Slug);

        if (link == null)
            return RestResponse<LinkViewModel>.Error("No link was found with the given slug.");

        await unitOfWork.BeginTransactionAsync();

        try
        {
            link.TotalUsage += 1;

            await linkRepository.IncrementUsageAsync(request.Slug);

            await metricRepository.CreateAsync(new Metric { LinkId = link.Id });

            await unitOfWork.CommitAsync();

            return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(link));
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}

using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.Application.MappingProfiles;

[ExcludeFromCodeCoverage]
public class MetricProfile : Profile
{
    public MetricProfile()
    {
        CreateMap<Metric, MetricViewModel>();
        CreateMap<MetricViewModel, Metric>();
    }
}

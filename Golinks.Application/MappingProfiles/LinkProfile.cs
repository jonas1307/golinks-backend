using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.DTOs;
using Golinks.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.Application.MappingProfiles;

[ExcludeFromCodeCoverage]
public class LinkProfile : Profile
{
    public LinkProfile()
    {
        CreateMap<Link, LinkViewModel>();
        CreateMap<Link, LinkMetricViewModel>();

        CreateMap<LinkViewModel, Link>()
            .ForMember(x => x.Id, y => y.Ignore())
            .ForMember(x => x.CreatedAt, y => y.Ignore());

        CreateMap<MetricDTO, MetricViewModel>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy/MM/dd")));
    }
}

using AutoMapper;
using Golinks.Application.Requests;
using Golinks.Application.Responses;
using Golinks.Domain.DTOs;
using Golinks.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.Application.MappingProfiles;

[ExcludeFromCodeCoverage]
public class LinkProfile : Profile
{
    public LinkProfile()
    {
        CreateMap<Link, LinkResponse>();
        CreateMap<Link, LinkMetricResponse>();

        CreateMap<LinkRequest, Link>()
            .ForMember(x => x.Id, y => y.Ignore())
            .ForMember(x => x.CreatedAt, y => y.Ignore())
            .ForMember(x => x.UpdatedAt, y => y.Ignore())
            .ForMember(x => x.TotalUsage, y => y.Ignore());

        CreateMap<MetricDTO, MetricResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy/MM/dd")));
    }
}

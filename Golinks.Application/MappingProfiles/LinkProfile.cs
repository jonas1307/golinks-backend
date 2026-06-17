using Golinks.Application.Requests;
using Golinks.Application.Responses;
using Golinks.Domain.DTOs;
using Golinks.Domain.Entities;
using Mapster;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.Application.MappingProfiles;

[ExcludeFromCodeCoverage]
public static class LinkProfile
{
    public static void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LinkRequest, Link>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt)
            .Ignore(dest => dest.UpdatedAt)
            .Ignore(dest => dest.TotalUsage);

        config.NewConfig<MetricDTO, MetricResponse>()
            .Map(dest => dest.Date, src => src.Date.ToString("yyyy/MM/dd"));
    }
}

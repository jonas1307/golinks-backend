using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.Application.MappingProfiles;

[ExcludeFromCodeCoverage]
public class LinkProfile : Profile
{
    public LinkProfile()
    {
        CreateMap<Link, LinkViewModel>();
        CreateMap<LinkViewModel, Link>();
    }
}

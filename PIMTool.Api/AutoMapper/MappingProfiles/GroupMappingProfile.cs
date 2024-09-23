using AutoMapper;
using PIMTool.Api.Responses;
using PIMTool.Core.Entities;

namespace PIMTool.Api.AutoMapper.MappingProfiles;

public class GroupMappingProfile : Profile
{
    public GroupMappingProfile()
    {
        CreateMap<Group, GroupResponse>()
            .ForMember(target => target.GroupLeaderName, opt => opt.MapFrom(src => src.GroupLeader.FirstName));
    }
}

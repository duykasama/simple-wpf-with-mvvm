using AutoMapper;
using PIMTool.Api.Converters;
using PIMTool.Api.Requests;
using PIMTool.Api.Responses;
using PIMTool.Core.Entities;

namespace PIMTool.Api.AutoMapper.MappingProfiles;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<CreateProjectRequest, Project>();
        CreateMap<UpdateProjectRequest, Project>();

        CreateMap<Project, ProjectResponse>()
            .ForMember(target => target.StartDate, opt => opt.ConvertUsing<DateTimeToDateOnlyConverter, DateTime>())
            .ForMember(target => target.EndDate, opt => opt.ConvertUsing<DateTimeToDateOnlyConverter, DateTime?>());

        CreateMap<Project, ProjectDetailResponse>()
            .ForMember(target => target.StartDate, opt => opt.ConvertUsing<DateTimeToDateOnlyConverter, DateTime>())
            .ForMember(target => target.EndDate, opt => opt.ConvertUsing<DateTimeToDateOnlyConverter, DateTime?>());
    }
}

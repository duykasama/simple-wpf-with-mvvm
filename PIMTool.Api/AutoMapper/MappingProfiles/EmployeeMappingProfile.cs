using AutoMapper;
using PIMTool.Api.Converters;
using PIMTool.Api.Responses;
using PIMTool.Core.Entities;

namespace PIMTool.Api.AutoMapper.MappingProfiles;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Employee, EmployeeResponse>()
            .ForMember(src => src.BirthDate, opt => opt.ConvertUsing<DateTimeToDateOnlyConverter, DateTime>());
    }
}


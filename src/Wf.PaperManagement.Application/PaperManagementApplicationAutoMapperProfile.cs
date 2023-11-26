using AutoMapper;
using Wf.PaperManagement.Papers;
using Wf.PaperManagement.Users;
using Wf.PaperManagement.Workers;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Wf.PaperManagement;

public class PaperManagementApplicationAutoMapperProfile : Profile
{
    public PaperManagementApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Paper, PaperDto>();
        CreateMap<Worker, WorkerDto>();
        CreateMap<User, UserDto>();
    }
}
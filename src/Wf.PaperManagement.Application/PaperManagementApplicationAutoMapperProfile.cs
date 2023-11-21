using AutoMapper;
using csuwf.PaperManagement.Papers;
using csuwf.PaperManagement.Users;
using csuwf.PaperManagement.Workers;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace csuwf.PaperManagement;

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
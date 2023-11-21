using System;
using System.Threading.Tasks;
using csuwf.PaperManagement.Common;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace csuwf.PaperManagement.Users;

public interface IUserAppService:IApplicationService
{
    
    Task<UserDto> GetAsync(Guid id);
    
    Task<PagedResultDto<UserDto>> GetListAsync(PagedSortedAndFilteredResultRequestDto input);
    
}
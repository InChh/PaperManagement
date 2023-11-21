using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using csuwf.PaperManagement.Common;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;

namespace csuwf.PaperManagement.Users;

[Authorize]
public class UserAppService : PaperManagementAppService, IUserAppService
{
    private readonly IKeycloakUserClient _keycloakUserClient;
    private readonly IOptions<KeycloakAdminClientOptions> _keycloakOptions;

    public UserAppService(IKeycloakUserClient keycloakUserClient, IOptions<KeycloakAdminClientOptions> keycloakOptions)
    {
        _keycloakUserClient = keycloakUserClient;
        _keycloakOptions = keycloakOptions;
    }

    public async Task<UserDto> GetAsync(Guid id)
    {
        var user = await _keycloakUserClient.GetUser(_keycloakOptions.Value.Realm,id.ToString());
        return ObjectMapper.Map<User, UserDto>(user);
    }

    public async Task<PagedResultDto<UserDto>> GetListAsync(PagedSortedAndFilteredResultRequestDto input)
    {
        var queryable = (await _keycloakUserClient.GetUsers(_keycloakOptions.Value.Realm)).AsQueryable();

        // 过滤查询
        if (!(input.FilterField.IsNullOrEmpty() || input.FilterValue.IsNullOrEmpty()))
        {
            queryable = input.FilterField switch
            {
                nameof(User.Username) => queryable.Where(t => t.Username!.Contains(input.FilterValue!)),
                _ => queryable
            };
        }

        // 排序
        queryable = !string.IsNullOrEmpty(input.Sorting)
            ? queryable.OrderBy(input.Sorting)
            : queryable.OrderByDescending(u => u.CreatedTimestamp);

        // 总数
        var total = await AsyncExecuter.CountAsync(queryable);

        // 分页
        queryable = queryable.PageBy(input);

        var list = await AsyncExecuter.ToListAsync(queryable);

        return new PagedResultDto<UserDto>(total, ObjectMapper.Map<List<User>, List<UserDto>>(list));
    }
}
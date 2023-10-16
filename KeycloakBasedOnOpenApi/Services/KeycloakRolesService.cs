using KeycloakBasedOnOpenApi.Config;
using KeycloakBasedOnOpenApi.Dto;
using KeycloakBasedOnOpenApi.Helper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace KeycloakBasedOnOpenApi.Services
{
  public class KeycloakRolesService
  {

    private readonly IOptions<KeycloackSettings> _keycloackSettings;
    private readonly HttpRequestHelper _apiExtentions;

    public KeycloakRolesService(HttpRequestHelper apiExtentions,
         IOptions<KeycloackSettings> keycloackSettings)
    {
      this._apiExtentions = apiExtentions;
      _keycloackSettings = keycloackSettings;
    }

    public async Task<List<KeycloakGroupDto>> GetAssignedGroupsForRoleByRoleName(string roleName, string keycloackAccessToken)
    {
      //we can replave the main URL in appsettings then just replace role, group ...
      var targetURL = KeycloakApis.Role.GetGroupsByRoleNameURL
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.RoleName, roleName);

      var roleGroups = await _apiExtentions.GetRequestAsync<List<KeycloakGroupDto>>(targetURL, keycloackAccessToken);

      return roleGroups.Data;
    }


    public async Task<List<KeycloakGroupDto>> GetRoles(string roleName, string token)
    {
      var keycloackAccessToken = token;

      //we can replave the main URL in appsettings then just replace role, group ...
      var targetURL = KeycloakApis.Role.GetGroupsByRoleNameURL
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.RoleName, roleName);

      var roleGroups = await _apiExtentions.GetRequestAsync<List<KeycloakGroupDto>>(targetURL, keycloackAccessToken);

      return roleGroups.Data;
    }

  }

}

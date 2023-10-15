using KeycloakBasedOnOpenApi.Config;
using KeycloakBasedOnOpenApi.Dto;
using KeycloakBasedOnOpenApi.Helper;
using Microsoft.Extensions.Options;

namespace KeycloakBasedOnOpenApi.Services
{
  public class KeycloakUserService
  {
    private readonly HttpRequestHelper _apiExtentions;
    private readonly AccessTokenService _accessTokenService;
    private readonly IOptions<KeycloackSettings> _keycloackSettings;
    public KeycloakUserService(HttpRequestHelper apiExtentions, AccessTokenService accessTokenService, IOptions<KeycloackSettings> keycloackSettings)
    {
      _apiExtentions = apiExtentions;
      _accessTokenService = accessTokenService;
      _keycloackSettings = keycloackSettings;
    }
    public async Task<UserFromKeycloakGroupDto> GetUserByUsername(string username, string keycloackAccessToken = null)
    {
      if (string.IsNullOrWhiteSpace(keycloackAccessToken))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken();
      }
      var targetURL = KeycloakApis.User.GetUserUsername
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.Username, username);

      var users = await _apiExtentions.GetRequestAsync<List<UserFromKeycloakGroupDto>>(targetURL, keycloackAccessToken);

      return users.Data?.FirstOrDefault();
    }
    public async Task<List<UserFromKeycloakGroupDto>> GetUsersByUsername(List<string> usernames, string keycloackAccessToken = null)
    {
      if (string.IsNullOrWhiteSpace(keycloackAccessToken))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken();
      }
      List<UserFromKeycloakGroupDto> result = new List<UserFromKeycloakGroupDto>();
      foreach (var username in usernames)
      {
        var user = await GetUserByUsername(username, keycloackAccessToken);
        if (user != null)
          result.Add(user);
      }
      return result;
    }
    public string GetFullNameByUsersAndUsername(List<UserFromKeycloakGroupDto> users, string username)
    {
      var user = users?.Where(user => user.Username == username).FirstOrDefault();
      if (user == null)
        return username;
      return user.FirstName + " " + user.LastName;
    }

    public async Task<List<UserFromKeycloakGroupDto>> GetUsersByFirstName(string firstName, string keycloackAccessToken = null)
    {
      if (string.IsNullOrWhiteSpace(keycloackAccessToken))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken();
      }
      var targetURL = KeycloakApis.User.GetUserUsernameWithoutQueryString
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.FirstName, firstName);

      var users = await _apiExtentions.GetRequestAsync<List<UserFromKeycloakGroupDto>>(targetURL, keycloackAccessToken);

      return users.Data;
    }
  }
}

using KeycloakBasedOnOpenApi.Config;
using KeycloakBasedOnOpenApi.Dto;
using KeycloakBasedOnOpenApi.Helper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;

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
    public async Task<KeycloakUserDto> GetUserByUsername(string username, string keycloackAccessToken = null)
    {
      if (string.IsNullOrWhiteSpace(keycloackAccessToken))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken();
      }
      var targetURL = KeycloakApis.User.GetUserUsername
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.Username, username);

      var users = await _apiExtentions.GetRequestAsync<List<KeycloakUserDto>>(targetURL, keycloackAccessToken);

      return users.Data?.FirstOrDefault();
    }
    public async Task<List<KeycloakUserDto>> GetUsersByUsername(List<string> usernames, string keycloackAccessToken = null)
    {
      if (string.IsNullOrWhiteSpace(keycloackAccessToken))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken();
      }
      List<KeycloakUserDto> result = new List<KeycloakUserDto>();
      foreach (var username in usernames)
      {
        var user = await GetUserByUsername(username, keycloackAccessToken);
        if (user != null)
          result.Add(user);
      }
      return result;
    }
    public string GetFullNameByUsersAndUsername(List<KeycloakUserDto> users, string username)
    {
      var user = users?.Where(user => user.Username == username).FirstOrDefault();
      if (user == null)
        return username;
      return user.FirstName + " " + user.LastName;
    }

    public async Task<List<KeycloakUserDto>> GetUsersByFirstName(string firstName, string token = null)
    {
      var keycloackAccessToken = token;

      if (string.IsNullOrWhiteSpace(token))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken(Master: true);

      }

      //http://localhost:8088/realms/amana/admin/realms/amana/users?firstName={firstName}"
      var targetURL = KeycloakApis.User.GetUsers
      .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.BaseURL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm);

         //.Replace(KeycloackApiParams.FirstName, firstName);

      var users = await _apiExtentions.GetRequestAsync<List<KeycloakUserDto>>(targetURL, keycloackAccessToken);

      return users.Data;
    }

    
  }
}

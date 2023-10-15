using KeycloakBasedOnOpenApi.Config;
using KeycloakBasedOnOpenApi.Dto;
using Microsoft.Extensions.Options;


namespace KeycloakBasedOnOpenApi.Services
{
  public class AccessTokenService
  {
    private readonly IOptions<KeycloackSettings> _keycloackSettings;
    //public const string GetKeycloackTokenURL = "{keycloak_url}/realms/{realm}/protocol/openid-connect/token";

    public AccessTokenService(
        IOptions<KeycloackSettings> keycloackSettings)
    {
      _keycloackSettings = keycloackSettings;
    }




    public async Task<string> GetAccessToken(bool Master = false)
    {
      try
      {
        string accessToken;


        var keycloackTokenURL = "";

        var formUrlEncodedContentDict = new Dictionary<string, string>();

        if (Master)
        {
          keycloackTokenURL = _keycloackSettings.Value.URL
             .Replace(KeycloackApiParams.Realm,"master");

        }
        else
        {
          keycloackTokenURL = _keycloackSettings.Value.URL
           .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm);

        }

        keycloackTokenURL = keycloackTokenURL + "/protocol/openid-connect/token";

        formUrlEncodedContentDict.Add(KeycloackConfig.ClientId, _keycloackSettings.Value.ClientId);
        //formUrlEncodedContentDict.Add(KeycloackConfig.ClientSecret, _keycloackSettings.Value.ClientSecret);
        formUrlEncodedContentDict.Add(KeycloackConfig.GrantType, _keycloackSettings.Value.GrantType);
        formUrlEncodedContentDict.Add(KeycloackConfig.Username, "sa");
        formUrlEncodedContentDict.Add(KeycloackConfig.Password, "sa");

        var keycloackTokenClient = new HttpClient();

        HttpRequestMessage? req = new HttpRequestMessage(HttpMethod.Post, keycloackTokenURL)
        { Content = new FormUrlEncodedContent(formUrlEncodedContentDict) };

        var res = await keycloackTokenClient.SendAsync(req);

        if (res.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new InvalidOperationException();
        }

        var keycloackTokenResult = await res.Content.ReadAsStringAsync();

        var keycloackToken = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResultDto>(keycloackTokenResult);

        accessToken = keycloackToken.access_token;

        return accessToken;
      }
      catch (Exception ex)
      {

        throw;
      }

    }





  }
}

namespace KeycloakBasedOnOpenApi.Config
{
  public static class KeycloackConfig
  {
    // here is just for replacing the names not the acual values 
    //to avoid magic strings
    public const string Authorization = "Authorization";
    public const string Bearer = "Bearer";

    // get the acual values from appsetting.json using KeycloackSettings with IOptions
    public const string GrantType = "grant_type";
    public const string ClientId = "client_id";
    public const string ClientSecret = "client_secret";

    public const string Username = "username";
    public const string Password = "password";
  }
}

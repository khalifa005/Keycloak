namespace KeycloakBasedOnOpenApi.Config
{
  public static class KeycloackApiParams
  {
    //will replace KeycloackApiTemplate items with dynamic values
    //ex : {keycloak_url}/admin/realms/{realm}/roles/{role_name}/groups -> {http://localhost:8081/auth}/admin/realms/amana/...
    public const string KeycloakUrl = "{keycloak_url}";
    public const string Realm = "{realm}";
    public const string RoleName = "{role_name}";
    public const string GroupId = "{group_id}";
    public const string UserId = "{user_id}";
    public const string Username = "{username}";
    public const string FirstName = "{firstName}";
  }
}

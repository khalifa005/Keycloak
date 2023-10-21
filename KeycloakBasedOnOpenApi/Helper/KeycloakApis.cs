namespace KeycloakBasedOnOpenApi.Helper
{
  public static class KeycloakApis
  {
    public const string GetKeycloackTokenURL = "{keycloak_url}/realms/{realm}/protocol/openid-connect/token";

    public static class Role
    {
      //Return List of Groups that have the specified role name [SAT] 
      public const string GetGroupsByRoleNameURL = "{keycloak_url}/admin/realms/{realm}/roles/{role_name}/groups";
    }

    public static class Group
    {
      //Return List of Groups members 
      public const string GetMembersByGroupId = "{keycloak_url}/admin/realms/{realm}/groups/{group_id}/members";
      public const string GetGroupsWithRepresentation = "{keycloak_url}/admin/realms/{realm}/groups/?briefRepresentation=false";
    }

    public static class User
    {
      //Return List of Groups members 
      public const string GetUserGroupById = "{keycloak_url}/admin/realms/{realm}/users/{user_id}/groups";
      public const string DeleteUserGroupById = "{keycloak_url}/admin/realms/{realm}/users/{user_id}/groups/{group_id}";
      public const string UpdateUserGroupById = "{keycloak_url}/admin/realms/{realm}/users/{user_id}/groups/{group_id}";

      public const string GetUserUsername = "{keycloak_url}/admin/realms/{realm}/users/?exact=true&username={username}";
      public const string GetUsers = "{keycloak_url}/admin/realms/{realm}/users";
      public const string GetUserUsernameWithoutQueryString = "{keycloak_url}/admin/realms/{realm}/users?firstName={firstName}";
    }
  }

}


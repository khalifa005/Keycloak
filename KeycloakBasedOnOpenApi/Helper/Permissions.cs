namespace KeycloakBasedOnOpenApi.Helper
{
  public static class Permissions
  {
    public static List<string> GeneratePermissionsForModule(string module)
    {
      return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
    }
    public static class Complain
    {
      public const string View = "Permissions.Complain.View";
      public const string Create = "Permissions.Complain.Create";
      public const string Edit = "Permissions.Complain.Edit";
      public const string Delete = "Permissions.Complain.Delete";
    }
  }

}


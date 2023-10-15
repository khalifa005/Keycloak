namespace KeycloakBasedOnOpenApi.Helper
{
  public static class SystemGroups
  {
    public static class ServiceOne
    {
      public const string Base = "/ServiceOne";
      public const string View = $"{Base}/view";
      public const string Add = $"{Base}/add";
      public const string Update = $"{Base}/update";
      public const string Delete = $"{Base}/delete";
    }

    public static class ComplainService
    {
      public const string Base = "/complain";
      public const string View = $"{Base}/view";
      public const string Add = $"{Base}/add";
      public const string Update = $"{Base}/update";
      public const string Delete = $"{Base}/delete";

    }

  }
}


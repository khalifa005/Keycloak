using KeycloakBasedOnOpenApi.Config;
using KeycloakBasedOnOpenApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace KeycloakBasedOnOpenApi
{
  public static class ConfigureServicesExtensions
  {


    //from amana
    //public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnvironment = false)
    //{
    //  IDPConfigcs idpPConfigcs = configuration.GetSection("IDP").Get<IDPConfigcs>();
    //  List<string> clientIds = new List<string>();
    //  if (configuration.GetSection("ServiceClients").Exists())
    //  {
    //    ServiceClient[] serviceClients = configuration.GetSection("ServiceClients").Get<ServiceClient[]>();
    //    clientIds = serviceClients.Select(sc => sc.ClientId).ToList();
    //  }
    //  clientIds.Add(idpPConfigcs.ClientID);
    //  services.AddAuthentication(options =>
    //  {
    //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //  })
    //  .AddJwtBearer("Bearer", c =>
    //  {
    //    c.RequireHttpsMetadata = true;
    //    c.Authority = idpPConfigcs.URL;
    //    c.MetadataAddress = idpPConfigcs.WellKnown;
    //    c.TokenValidationParameters = new TokenValidationParameters()
    //    {
    //      ValidAudiences = clientIds,
    //      ValidateIssuer = true,
    //      ValidIssuer = idpPConfigcs.URL,
    //      ValidateAudience = true
    //    };

    //    ////ignore certificate errors when running locally in Development environment.
    //    //if (isDevelopmentEnvironment && IgnoreCertificateErrorsOnDev)
    //    //    c.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = ServerCertificateCustomValidationCallback };

    //  });

    //  return services;
    //}


    public static IServiceCollection AddKeycloackSettings(this IServiceCollection services, IConfiguration configuration)
    {
      

      services.AddTransient<HttpRequestHelper>();

      services.AddSingleton<AccessTokenService>();
      services.AddSingleton<KeycloakGroupService>();
      services.AddSingleton<KeycloakRolesService>();
      services.AddSingleton<KeycloakUserService>();

      services.Configure<KeycloackSettings>(configuration.GetSection("KeycloackSettings"));

      return services;
    }

  }

}

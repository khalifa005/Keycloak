using KeycloakBasedOnOpenApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak.Controllers
{
  [ApiController]
  [Authorize]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AccessTokenService _accessTokenService;
    private readonly KeycloakGroupService _keycloakGroupService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
      KeycloakGroupService keycloakGroupService,
      AccessTokenService accessTokenService)
    {
      _logger = logger;
      _accessTokenService = accessTokenService;
      _keycloakGroupService = keycloakGroupService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {

      var masterToekn = await _accessTokenService.GetAccessToken(Master:true);
      var grousps = await _keycloakGroupService.GetGroupsWithRepresentation(masterToekn);

      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }



  }
}

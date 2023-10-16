using KeycloakBasedOnOpenApi.Dto;
using KeycloakBasedOnOpenApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak.Controllers
{
  [ApiController]
  [Authorize]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AccessTokenService _accessTokenService;
    private readonly KeycloakGroupService _keycloakGroupService;
    private readonly KeycloakRolesService _keycloakRolesService;

    public AuthController(ILogger<WeatherForecastController> logger,
      KeycloakGroupService keycloakGroupService,
      KeycloakRolesService keycloakRolesService,
      AccessTokenService accessTokenService)
    {
      _logger = logger;
      _accessTokenService = accessTokenService;
      _keycloakGroupService = keycloakGroupService;
      _keycloakRolesService = keycloakRolesService;

    }

    [HttpGet(Name = "GetGroups")]
    public async Task<ActionResult<List<KeycloakGroupDto>>> GetGroups()
    {

      var masterToekn = await _accessTokenService.GetAccessToken(Master: true);
      var grousps = await _keycloakGroupService.GetGroupsWithRepresentation(masterToekn);

      return Ok(grousps);
     
    }


    //[HttpGet(Name = "GetRoles")]
    //public async Task<ActionResult<List<KeycloakGroupDto>>> GetRoles()
    //{

    //  var masterToekn = await _accessTokenService.GetAccessToken(Master: true);
    //  var roles = await _keycloakGroupService.GetGroupsWithRepresentation(masterToekn);

    //  return Ok(grousps);
     
    //}



  }
}

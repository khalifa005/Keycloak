using KeycloakBasedOnOpenApi.Dto;
using KeycloakBasedOnOpenApi.Helper;
using KeycloakBasedOnOpenApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Keycloak.Controllers
{
  [ApiController]
  [Authorize]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {

    private readonly ILogger<AuthController> _logger;
    private readonly AccessTokenService _accessTokenService;
    private readonly KeycloakGroupService _keycloakGroupService;
    private readonly KeycloakRolesService _keycloakRolesService;
    private readonly KeycloakUserService _keycloakUserService;

    public AuthController(ILogger<AuthController> logger,
      KeycloakGroupService keycloakGroupService,
      KeycloakRolesService keycloakRolesService,
      KeycloakUserService  keycloakUserService,
      AccessTokenService accessTokenService)
    {
      _logger = logger;
      _accessTokenService = accessTokenService;
      _keycloakGroupService = keycloakGroupService;
      _keycloakRolesService = keycloakRolesService;
      _keycloakUserService = keycloakUserService;

    }

    [HttpGet("GetGroups")]
    public async Task<ActionResult<List<KeycloakGroupDto>>> GetGroups()
    {
      var apiResponse = new ApiResponse<List<KeycloakGroupDto>>((int)HttpStatusCode.OK);

      var masterToekn = await _accessTokenService.GetAccessToken(Master: true);
      var grousps = await _keycloakGroupService.GetGroupsWithRepresentation(masterToekn);

      apiResponse.Data = grousps;

      return Ok(apiResponse);
     
    }


    [HttpGet("GetUsers")]
    public async Task<ActionResult<List<KeycloakUserDto>>> GetUsers()
    {
      var apiResponse = new ApiResponse<List<KeycloakUserDto>>((int)HttpStatusCode.OK);

      var masterToekn = await _accessTokenService.GetAccessToken(Master: true);
      var grousps = await _keycloakUserService.GetUsersByFirstName("",masterToekn);

      apiResponse.Data = grousps;

      return Ok(apiResponse);
     
    }


    [HttpGet("GetUserGroupsById{Id}")]
    public async Task<ActionResult<List<KeycloakGroupDto>>> GetUserGroupsByUserId(string userId)
    {
      var apiResponse = new ApiResponse<List<KeycloakGroupDto>>((int)HttpStatusCode.OK);

      var masterToekn = await _accessTokenService.GetAccessToken(Master: true);
      var userGroups = await _keycloakGroupService.GetMemberGroupsByUserId(userId, masterToekn);

      var userGroupsAsFlatList = _keycloakGroupService.GetGroupsAsFlatList(userGroups);

      apiResponse.Data = userGroupsAsFlatList;

      return Ok(apiResponse);
     
    }


    [HttpGet("GetRepresentationUserGroupsById")]
    public async Task<ActionResult<List<KeycloakGroupDto>>> GetRepresentationUserGroupsById([FromQuery]string id)
    {
      var apiResponse = new ApiResponse<List<KeycloakGroupDto>>((int)HttpStatusCode.OK);

      var masterToekn = await _accessTokenService.GetAccessToken(Master: true);
      var userGroups = await _keycloakGroupService.GetMemberGroupsByUserId(id, masterToekn);

      //var userGroupsAsFlatList = _keycloakGroupService.GetGroupsAsFlatList(userGroups);

      var allSystemGroups = await _keycloakGroupService.GetGroupsWithRepresentation(masterToekn);

      List<KeycloakGroupDto> test = new List<KeycloakGroupDto>();
      _keycloakGroupService.FindMatchingGroups(allSystemGroups, userGroups);

      apiResponse.Data = allSystemGroups;

      return Ok(apiResponse);
     
    }

    [HttpPut("UpdateUserGroups")]
    public async Task<ActionResult<ApiResponse<List<UserGroupsDto>>>> UpdateUserGroups([FromBody] UserGroupsDto userGroupsDto)
    {
      var masterToekn = await _accessTokenService.GetAccessToken(Master: true);
      var userGroups = await _keycloakGroupService.GetMemberGroupsByUserId(userGroupsDto.UserId, masterToekn);
      if (userGroups.Count > 0)
      {
        var currentUserGroupIds = userGroups.Select(x => x.Id).ToList();
        var deleedUserGroups = await _keycloakGroupService.DeleteMemberGroupsByUserId(userGroupsDto.UserId, currentUserGroupIds, masterToekn);

      }

      if (userGroupsDto.GroupIds.Count > 0)
      {
        var newUserGroups = await _keycloakGroupService.UpdateMemberGroupsByUserId(userGroupsDto.UserId, userGroupsDto.GroupIds, masterToekn);

      }

      return Ok();
    }

  }
}

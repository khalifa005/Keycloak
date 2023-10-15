using KeycloakBasedOnOpenApi.Config;
using KeycloakBasedOnOpenApi.Dto;
using KeycloakBasedOnOpenApi.Helper;
using Microsoft.Extensions.Options;
using System.Net;

namespace KeycloakBasedOnOpenApi.Services
{
  public class KeycloakGroupService
  {

    private readonly HttpRequestHelper _apiExtentions;
    private readonly IOptions<KeycloackSettings> _keycloackSettings;
    private readonly AccessTokenService _accessTokenService;
    public KeycloakGroupService(HttpRequestHelper apiExtentions,
         IOptions<KeycloackSettings> keycloackSettings,
         AccessTokenService accessTokenService)
    {
      _apiExtentions = apiExtentions;
      _keycloackSettings = keycloackSettings;
      _accessTokenService = accessTokenService;
    }

    public async Task<List<KeycloakGroupDto>> GetGroupsWithRepresentation(string token)
    {

      var keycloackAccessToken = token;

      if (string.IsNullOrWhiteSpace(token))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken(Master:true);

      }
      var key = "keycloackGroup";

      List<KeycloakGroupDto> groupsWithRepresentation = new List<KeycloakGroupDto>();


      if (groupsWithRepresentation == null || groupsWithRepresentation.Count == 0)
      {

        var targetURL = KeycloakApis.Group.GetGroupsWithRepresentation
      .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.BaseURL)
      .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm);

        var groupsMembersResponse = await _apiExtentions.GetRequestAsync<List<KeycloakGroupDto>>(targetURL, keycloackAccessToken);


        if (groupsMembersResponse.Data == null || groupsMembersResponse.StatusCode != (int) HttpStatusCode.OK)
          return groupsWithRepresentation;

        groupsWithRepresentation = groupsMembersResponse.Data;
      }

      return groupsWithRepresentation;
    }

    public async Task<List<SubGroup>> GetServiceGroupsAsFlat()
    {
      var keycloackGroups = await GetGroupsWithRepresentation("");
      KeycloakGroupDto services = keycloackGroups.FirstOrDefault();

      List<SubGroup> keycloackServices = new List<SubGroup>();

      foreach (var service in services.SubGroups)
      {
        ChangeSubGroupsIntoFlatList(service, keycloackServices);
      }

      return keycloackServices;
    }

    public async Task<List<UserFromKeycloakGroupDto>> GetMembersOfGroupId(string groupId, string keycloackAccessToken)
    {

      var targetURL = KeycloakApis.Group.GetMembersByGroupId
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.GroupId, groupId);

      var groupsMembersRes = await _apiExtentions.GetRequestAsync<List<UserFromKeycloakGroupDto>>(targetURL, keycloackAccessToken);

      //we can use isTrue to validate the request 

      return groupsMembersRes.Data;
    }

    public async Task<List<KeycloakGroupDto>> GetMemberGroupsByUserId(string userId, string keycloackAccessToken)
    {

      var targetURL = KeycloakApis.User.GetUserGroupById
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.UserId, userId);

      var groupsRes = await _apiExtentions.GetRequestAsync<List<KeycloakGroupDto>>(targetURL, keycloackAccessToken);

      return groupsRes.Data;
    }

   
    public async Task<(List<SubGroup> keycloackDeps, List<SubGroup> keycloackServices)> GetKeycloackFlatServicesAndDepsGroups()
    {
      List<KeycloakGroupDto> keycloackGroups = await GetGroupsWithRepresentation("");

      KeycloakGroupDto departments = keycloackGroups.Where(x => x.Name == "deps").FirstOrDefault();

      KeycloakGroupDto services = keycloackGroups.Where(x => x.Name == "Services").FirstOrDefault();


      List<SubGroup> keycloackDeps = new List<SubGroup>();

      foreach (var group in departments.SubGroups)
      {
        ChangeSubGroupsIntoFlatList(group, keycloackDeps);
      }

      List<SubGroup> keycloackServices = new List<SubGroup>();

      foreach (var service in services.SubGroups)
      {
        ChangeSubGroupsIntoFlatList(service, keycloackServices);
      }


      return (keycloackDeps, keycloackServices);
    }



    #region Extentions
    public void ChangeSubGroupsIntoFlatList(SubGroup group, List<SubGroup> relatedUserDeps)
    {
      relatedUserDeps.Add(group);

      if (group.SubGroups.Any())
      {
        foreach (var item in group.SubGroups)
        {
          if (group.Attributes != null && group.Attributes.DepartmentId != null)
          {
            if (group.Attributes.DepartmentId.FirstOrDefault() != null)
            {
              item.InherteedDepartmentId = group.Attributes.DepartmentId.FirstOrDefault();
            }
          }
          ChangeSubGroupsIntoFlatList(item, relatedUserDeps);
        }
      }
    }
    #endregion

  }
}

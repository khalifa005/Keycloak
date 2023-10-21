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

    //public async Task<List<KeycloakGroupDto>> Test()
    //{

    //  List<KeycloakGroupDto> allSystemGroups = new List<KeycloakGroupDto>();
    //  List<KeycloakGroupDto> userGroups = new List<KeycloakGroupDto>();

    //  var systemRoleFunctions = (from s in allSystemGroups
    //                             join srf in userGroups on s.Id equals srf.Id into subSF
    //                             from sf in subSF.DefaultIfEmpty()
    //                             select new KeycloakGroupDto
    //                             {
    //                               Id = s.Id,
    //                               NameEn = s.NameEn,
    //                               NameAr = s.NameAr,
    //                               ParentID = s.ParentID,
    //                               Checked = sf != null,
    //                               SortKey = s.SortKey,
    //                             }).ToList();
    //}

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

        groupsWithRepresentation = groupsMembersResponse.Data.OrderBy(x=> x.Name).ToList();
      }

      return groupsWithRepresentation;
    }



    public async Task<List<KeycloakUserDto>> GetMembersOfGroupId(string groupId, string keycloackAccessToken)
    {

      var targetURL = KeycloakApis.Group.GetMembersByGroupId
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.URL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.GroupId, groupId);

      var groupsMembersRes = await _apiExtentions.GetRequestAsync<List<KeycloakUserDto>>(targetURL, keycloackAccessToken);

      //we can use isTrue to validate the request 

      return groupsMembersRes.Data;
    }

    public async Task<List<KeycloakGroupDto>> GetMemberGroupsByUserId(string userId, string token)
    {
      var keycloackAccessToken = token;

      if (string.IsNullOrWhiteSpace(token))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken(Master: true);

      }


      var targetURL = KeycloakApis.User.GetUserGroupById
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.BaseURL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.UserId, userId);

      var groupsRes = await _apiExtentions.GetRequestAsync<List<KeycloakGroupDto>>(targetURL, keycloackAccessToken);

      return groupsRes.Data;
    }

    public async Task<bool> DeleteMemberGroupsByUserId(string userId,List<string> groupIds, string token)
    {
      var keycloackAccessToken = token;

      if (string.IsNullOrWhiteSpace(token))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken(Master: true);

      }

      foreach (var groupId in groupIds)
      {
        var targetURL = KeycloakApis.User.DeleteUserGroupById
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.BaseURL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.UserId, userId)
         .Replace(KeycloackApiParams.GroupId, groupId);

        var groupsRes = await _apiExtentions.DeleteRequestAsync<List<KeycloakGroupDto>>(targetURL, keycloackAccessToken);

      }

      return true;
    }

    public async Task<bool> UpdateMemberGroupsByUserId(string userId, List<string> groupIds, string token)
    {
      var keycloackAccessToken = token;

      if (string.IsNullOrWhiteSpace(token))
      {
        keycloackAccessToken = await _accessTokenService.GetAccessToken(Master: true);

      }

      foreach (var groupId in groupIds)
      {
        var targetURL = KeycloakApis.User.UpdateUserGroupById
         .Replace(KeycloackApiParams.KeycloakUrl, _keycloackSettings.Value.BaseURL)
         .Replace(KeycloackApiParams.Realm, _keycloackSettings.Value.Realm)
         .Replace(KeycloackApiParams.UserId, userId)
         .Replace(KeycloackApiParams.GroupId, groupId);

        var groupsRes = await _apiExtentions.UpdateRequestAsync<List<KeycloakGroupDto>>(targetURL, keycloackAccessToken);

      }

      return true;
    }

    //public async Task<(List<SubGroup> keycloackDeps, List<SubGroup> keycloackServices)> GetKeycloackFlatServicesAndDepsGroups()
    //{
    //  List<KeycloakGroupDto> keycloackGroups = await GetGroupsWithRepresentation("");

    //  KeycloakGroupDto departments = keycloackGroups.Where(x => x.Name == "deps").FirstOrDefault();

    //  KeycloakGroupDto services = keycloackGroups.Where(x => x.Name == "Services").FirstOrDefault();


    //  List<SubGroup> keycloackDeps = new List<SubGroup>();

    //  foreach (var group in departments.SubGroups)
    //  {
    //    ChangeSubGroupsIntoFlatList(group, keycloackDeps);
    //  }

    //  List<SubGroup> keycloackServices = new List<SubGroup>();

    //  foreach (var service in services.SubGroups)
    //  {
    //    ChangeSubGroupsIntoFlatList(service, keycloackServices);
    //  }


    //  return (keycloackDeps, keycloackServices);
    //}



    #region Extentions

    public void FindMatchingGroups(List<KeycloakGroupDto> allGroups, List<KeycloakGroupDto> userGroups)
    {


      foreach (var group1 in allGroups)
      {
        foreach (var group2 in userGroups)
        {
          if (AreGroupsMatching(group1, group2))
          {
            group1.Checked = true;

          }

        }

        if (group1.SubGroups != null && group1.SubGroups.Count > 0)
        {
          FindMatchingGroups(group1.SubGroups, userGroups);
        }
      }

    }

    private bool AreGroupsMatching(KeycloakGroupDto group1, KeycloakGroupDto group2)
    {
      // Compare the properties to determine if the groups are matching
      return group1.Id == group2.Id && group1.Path == group2.Path;
    }

    public List<KeycloakGroupDto> GetGroupsAsFlatList(List<KeycloakGroupDto> keycloackGroups)
    {
      List<KeycloakGroupDto> keycloackFlatGroups = new List<KeycloakGroupDto>();

      foreach (var service in keycloackGroups)
      {
        ChangeSubGroupsIntoFlatList(service, keycloackFlatGroups);
      }

      return keycloackFlatGroups;
    }
    private void ChangeSubGroupsIntoFlatList(KeycloakGroupDto group, List<KeycloakGroupDto> relatedUserDeps)
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
              //item.InherteedDepartmentId = group.Attributes.DepartmentId.FirstOrDefault();
            }
          }
          ChangeSubGroupsIntoFlatList(item, relatedUserDeps);
        }
      }
    }
    #endregion

  }
}

using Newtonsoft.Json;

namespace KeycloakBasedOnOpenApi.Dto
{



  public class KeycloakGroupDto
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public Attributes Attributes { get; set; }
    public List<object> RealmRoles { get; set; }
    public bool Checked { get; set; } = false;

    public List<KeycloakGroupDto> SubGroups { get; set; }

   

    
  }

  public class Attributes
  {
    public List<string> Arabic { get; set; }
    public List<string> DepartmentId { get; set; }

    [JsonProperty("is_manager")]
    public List<bool> IsManager { get; set; }

    [JsonProperty("manager_groups")]
    public List<string> ManagerGroups { get; set; }
    public List<string> EngineeringOfficeID { get; set; }
  }


  public class SubGroup
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public Attributes Attributes { get; set; }
    public List<object> RealmRoles { get; set; }
    public List<SubGroup> SubGroups { get; set; }
    public string InherteedDepartmentId { get; set; } //no need
  }

  public class UserGroupsDto
  {
    public string UserId { get; set; }
    public List<string> GroupIds { get; set; } = new List<string>();
  }
}






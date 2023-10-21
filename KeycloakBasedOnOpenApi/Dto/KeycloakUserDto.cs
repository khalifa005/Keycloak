namespace KeycloakBasedOnOpenApi.Dto
{
  public class KeycloakUserDto
  {
    public string Id { get; set; }
    public long CreatedTimestamp { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Enabled { get; set; }
    public bool Totp { get; set; }
    public bool EmailVerified { get; set; }
    public string Email { get; set; }
    public List<object> DisableableCredentialTypes { get; set; }
    public List<object> RequiredActions { get; set; }
    public int NotBefore { get; set; }
  }
}


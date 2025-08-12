using System.ComponentModel.DataAnnotations;

public class UserDetails
{
    [Key]
    public string? Id { get; set; } // Same as IdentityUser Id (Foreign Key)
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;
}

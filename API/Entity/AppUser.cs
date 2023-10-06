using System.ComponentModel.DataAnnotations;

namespace API;

public class AppUser
{
    public int Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public byte[] passwordHash {get; set;}

    public byte[] passwordSalt { get; set; }
}

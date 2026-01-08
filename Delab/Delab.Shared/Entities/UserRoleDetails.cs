using Delab.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class UserRoleDetails
{
    [Key]
    public int UserRoleDetailsId { get; set; }

    [Display(Name = "Rol Usuario")]
    public UserType? UserType { get; set; }

    [Display(Name = "User Id")]
    public string? UserId { get; set; }

    // Relaciones entre entidades

    public User? User { get; set; }
}
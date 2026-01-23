using Delab.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delab.Shared.Entities;

public class Manager
{
    [Key]
    public int ManagerId { get; set; }

    [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    [Display(Name = "Nombres")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {1} de largo")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    public string? FullName { get; set; }

    [MaxLength(15, ErrorMessage = "El Maximo de caracteres es {0}")]
    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [Display(Name = "RUC ó DNI")]
    public string? Nro_Document { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(25, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Telefono")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Direccion")]
    public string Address { get; set; } = null!;

    // Correo y Corporation
    
    [MaxLength(256, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string UserName { get; set; } = null!;

    [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {1}")]
    [Display(Name = "Corporacion")]
    public int CorporationId { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Puesto Trabajo")]
    public string Job { get; set; } = null!;

    [Display(Name = "Tipo Usuario")]
    public UserType UserType { get; set; }

    [Display(Name = "Foto")]
    public string? Photo { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    // Propiedad virtual de Acceso a imagen
    
    public string ImageFullPath => Photo == string.Empty || Photo == null
        ? $"https://localhost:7148/Images/NoImage.png"
        : $"https://localhost:7148/Images/ImgManager/{Photo}";

    [NotMapped]
    public string? ImgBase64 { get; set; }

    // Relaciones entre entidades

    public Corporation? Corporation { get; set; }
}
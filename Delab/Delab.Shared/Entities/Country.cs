using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class Country
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} Caracter")]
    [Display(Name = "Pais")]
    public string Name { get; set; } = null!;

    [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} Caracter")]
    [Display(Name = "Cod Phone")]
    public string? CodPhone { get; set; }

    // Expression-bodied property
    public int NumStates => States?.Count ?? 0;

    // Relaciones entre entidades

    public ICollection<State>? States { get; set; }

    public ICollection<Corporation>? Corporations { get; set; }
}
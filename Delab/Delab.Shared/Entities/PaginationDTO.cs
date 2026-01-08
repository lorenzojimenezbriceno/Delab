namespace Delab.Shared.Entities;

public class PaginationDTO
{
    public int Id { get; set; }
    public int Page { get; set; }
    // Numero de Registros que se muestran por paginacion
    public int RecordsNumber { get; set; } = 10; 
    public string? Filter { get; set; }
    public string? Email { get; set; }
}

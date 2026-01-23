using Delab.AccessData.Data;
using Delab.Backend.Helpers;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Controllers.Entities;

[Route("api/countries")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly DataContext _context;

    public CountriesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("loadCombo")]
    public async Task<ActionResult<IEnumerable<Country>>> GetPeriodicidads()
    {
        var listResult = await _context.Countries.OrderBy(x => x.Name).ToListAsync();
        return listResult;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries([FromQuery] PaginationDTO pagination)
    {
        var queryable = _context.Countries.AsQueryable();
        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name!.ToLower().Contains(pagination.Filter.ToLower()));
        }
        // Inserta los dos encabezados en el response
        await HttpContext.InsertParameterPagination(queryable, pagination.RecordsNumber);
        
        return await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCorporation(int id)
    {
        try
        {
            var modelo = await _context.Countries.FindAsync(id);

            if (modelo == null)
            {
                return NotFound();
            }
            return modelo;
        }
        catch (DbUpdateException dbUpdateException)
        {
            return BadRequest(dbUpdateException.InnerException!.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
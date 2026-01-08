using Daleb.Shared.Entities;
using Delab.AccessData.Data;
using Delab.Backend.Helpers;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ven.Backend.Controllers;

[Route("api/countries")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly DataContext _context;

    public CountriesController(DataContext context)
    {
        _context = context;
    }

    // GET: api/countries
    [HttpGet("/pagina")]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries([FromQuery] PaginationDTO pagination)
    {
        var queryable = _context.Countries.AsQueryable();
        await HttpContext.InsertParameterPagination(queryable, pagination.RecordsNumber);
        return await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        return await _context.Countries.OrderBy(x => x.Name).ToListAsync();
    }

    // GET: api/countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalido");
        }

        var country = await _context.Countries.FindAsync(id);
        if (country == null)
        {
            return NotFound("No existge el registro solicitado");
        }

        return country;
    }

    // PUT: api/countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut]
    public async Task<IActionResult> PutCountry(Country country)
    {
        if (country.CountryId <= 0 || String.IsNullOrEmpty(country.Name) || country.Name.Trim() == String.Empty)
        {
            return BadRequest("Id invalido");
        }

        country.Name = country.Name.Trim();

        if (CountryExists(country.Name, country.CountryId))
        {
            return BadRequest("Ya existe un registro con el mismo nombre.");
        }

        _context.Entry(country).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(country.Name))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        catch (DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
            {
                return BadRequest("Ya existe un Registro con el mismo nombre.");
            }
            else
            {
                return BadRequest(dbUpdateException.InnerException.Message);
            }
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

        return Ok();
    }

    // POST: api/countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(Country country)
    {
        if (CountryExists(country.Name))
        {
            return BadRequest("Ya existe un registro con el mismo nombre.");
        }

        country.Name = country.Name.Trim();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(country.Name))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        catch (DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
            {
                return BadRequest("Ya existe un Registro con el mismo nombre.");
            }
            else
            {
                return BadRequest(dbUpdateException.InnerException.Message);
            }
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

        _context.Countries.Add(country);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCountry", new { id = country.CountryId }, country);
    }

    // DELETE: api/countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        try
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException!.Message.Contains("REFERENCE"))
            {
                return BadRequest("Existen Registros Relacionados y no se puede Eliminar el registro");
            }
            else
            {
                return BadRequest(dbUpdateException.InnerException.Message);
            }
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    private bool CountryExists(string name)
    {
        return _context.Countries
            .Any(e => e.Name.Trim().ToLower() == name.Trim().ToLower());
    }

    // Los paises tienen que tener un nombre distinto, comprobacion cuando se renombra un pais.
    private bool CountryExists(string name, int countryId)
    {
        return _context.Countries
            .Any(e => e.Name.ToLower() == name.Trim().ToLower() && e.CountryId != countryId);
    }
}

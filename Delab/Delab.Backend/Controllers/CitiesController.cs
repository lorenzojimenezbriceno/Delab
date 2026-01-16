using Delab.AccessData.Data;
using Delab.Backend.Helpers;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ven.Backend.Controllers;

[Route("api/cities")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly DataContext _context;

    public CitiesController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Cities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCountries([FromQuery] PaginationDTO pagination)
    {
        var queryable = _context.Cities.Where(x => x.StateId == pagination.Id).Include(x => x.State).AsQueryable();

        // Inserta los dos encabezados en el response
        await HttpContext.InsertParameterPagination(queryable, pagination.RecordsNumber);

        return await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
    }


    // GET: api/Cities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(int id)
    {
        var city = await _context.Cities.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        return city;
    }

    // PUT: api/Cities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut]
    public async Task<IActionResult> PutCity(City city)
    {
        if (city.StateId <= 0 || city.Id <= 0 || String.IsNullOrEmpty(city.Name) || city.Name.Trim() == String.Empty)
        {
            return BadRequest("Ids de estado y pais invalidos");
        }

        city.Name = city.Name.Trim();

        if (CityExists(city.Name, city.StateId))
        {
            return BadRequest("Ya existe un registro con el mismo nombre.");
        }

        _context.Entry(city).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(city.Name, city.StateId))
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

    // POST: api/Cities
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<City>> PostCity(City city)
    {
        if (city.StateId <= 0 || String.IsNullOrEmpty(city.Name) || city.Name.Trim() == String.Empty)
        {
            return BadRequest("Ids de estado y pais invalidos");
        }

        city.Name = city.Name.Trim();

        if (CityExist(city.Name, city.StateId))
        {
            return BadRequest("Ya existe un registro con el mismo nombre.");
        }

        _context.Cities.Add(city);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExist(city.Name, city.StateId))
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

        return CreatedAtAction("GetCity", new { id = city.Id }, city);
    }

    // DELETE: api/Cities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        try
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException!.Message.Contains("REFERENCE"))
            {
                return BadRequest("Existen Registros Relacionados y no se puede Eliminar");
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

    // Las ciudades tienen que tener un nombre distinto, comprobacion cuando se renombra una ciudad.
    private bool CityExist(string name, int Id)
    {
        return _context.Cities
            .Any(e => e.Name.ToLower() == name.Trim().ToLower() && e.StateId != Id);
    }

    // En un estado, las ciudades deben tener un nombre distinto, comprobacion cuando se renombra una ciudad.
    private bool CityExists(string name, int stateId)
    {
        return _context.Cities
            .Any(e => e.StateId == stateId && e.Name.Trim().ToLower() == name.Trim().ToLower());
    }
}

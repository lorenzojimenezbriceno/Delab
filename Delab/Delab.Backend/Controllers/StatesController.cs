using Daleb.Shared.Entities;
using Delab.AccessData.Data;
using Delab.Backend.Helpers;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ven.Backend.Controllers;

[Route("api/states")]
[ApiController]
public class StatesController : ControllerBase
{
    private readonly DataContext _context;

    public StatesController(DataContext context)
    {
        _context = context;
    }

    // GET: api/States
    [HttpGet]
    public async Task<ActionResult<IEnumerable<State>>> GetCountries([FromQuery] PaginationDTO pagination)
    {
        var queryable = _context.States.Where(x => x.CountryId == pagination.Id).Include(x => x.Cities).AsQueryable();

        await HttpContext.InsertParameterPagination(queryable, pagination.RecordsNumber);

        return await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
    }


    // GET: api/States/5
    [HttpGet("{id}")]
    public async Task<ActionResult<State>> GetState(int id)
    {
        var state = await _context.States.FindAsync(id);

        if (state == null)
        {
            return NotFound();
        }

        return state;
    }

    // PUT: api/States/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut]
    public async Task<IActionResult> PutState(State state)
    {
        if (state.StateId <= 0 || state.CountryId <= 0 || String.IsNullOrEmpty(state.Name) || state.Name.Trim() == String.Empty)
        {
            return BadRequest("Ids de estado y pais invalidos");
        }

        state.Name = state.Name.Trim();

        if (StateExists(state.Name, state.StateId, state.CountryId))
        {
            return BadRequest("Ya existe un registro con el mismo nombre.");
        }

        _context.Entry(state).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StateExists(state.Name, state.CountryId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok();
    }


    // POST: api/States
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<State>> PostState(State state)
    {
        if (state.CountryId <= 0 || String.IsNullOrEmpty(state.Name) || state.Name.Trim() == String.Empty)
        {
            return BadRequest("Ids de estado y pais invalidos");
        }

        state.Name = state.Name.Trim();

        if (StateExists(state.Name, state.CountryId))
        {
            return BadRequest("Ya existe un registro con el mismo nombre.");
        }

        _context.States.Add(state);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StateExists(state.Name, state.CountryId))
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

        return CreatedAtAction("GetState", new { id = state.StateId }, state);
    }

    // DELETE: api/States/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteState(int id)
    {
        try
        {
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.States.Remove(state);
            
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

    private bool StateExists(string name, int countryId)
    {
        return _context.States
            .Any(e => e.CountryId == countryId && e.Name.Trim().ToLower() == name.Trim().ToLower());
    }

    // Los estados tienen que tener un nombre distinto, comprobacion cuando se renombra un estado.
    private bool StateExists(string name, int stateId, int countryId)
    {
        return _context.States
            .Any(e => e.Name.ToLower() == name.Trim().ToLower() && e.StateId != stateId && e.CountryId == countryId);
    }
}

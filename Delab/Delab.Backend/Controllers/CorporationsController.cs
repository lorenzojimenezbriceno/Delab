using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Delab.AccessData.Data;
using Delab.Shared.Entities;

namespace Delab.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorporationsController : ControllerBase
    {
        private readonly DataContext _context;

        public CorporationsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Corporations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Corporation>>> GetCorporations()
        {
            return await _context.Corporations.ToListAsync();
        }

        // GET: api/Corporations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Corporation>> GetCorporation(int id)
        {
            var corporation = await _context.Corporations.FindAsync(id);

            if (corporation == null)
            {
                return NotFound();
            }

            return corporation;
        }

        // PUT: api/Corporations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCorporation(int id, Corporation corporation)
        {
            if (id != corporation.CorporationId)
            {
                return BadRequest();
            }

            _context.Entry(corporation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CorporationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Corporations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Corporation>> PostCorporation(Corporation corporation)
        {
            _context.Corporations.Add(corporation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCorporation", new { id = corporation.CorporationId }, corporation);
        }

        // DELETE: api/Corporations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorporation(int id)
        {
            var corporation = await _context.Corporations.FindAsync(id);
            if (corporation == null)
            {
                return NotFound();
            }

            _context.Corporations.Remove(corporation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CorporationExists(int id)
        {
            return _context.Corporations.Any(e => e.CorporationId == id);
        }
    }
}

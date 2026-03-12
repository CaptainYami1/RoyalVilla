using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Context;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTOs;
using System.Threading.Tasks;

namespace RoyalVilla_API.Controllers
{
    [Route("GetVillas")]
    [ApiController]
    public class RoyalVillaController : ControllerBase
    {
        private readonly RoyalVilleDbContext _context;

        public RoyalVillaController(RoyalVilleDbContext context) { 
        _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillas(int id)
        {
            return Ok(await _context.Villas.ToListAsync());
        }

        [HttpGet("GetVillasById/{id}")]
        public async Task<ActionResult<Villa>> GetVillasById(int id)
        {
            try
            {
                
                if (id <= 0)
                {
                    return BadRequest("Villa id must be greater than 0");
                }
                var villa = await _context.Villas.FirstOrDefaultAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with id {id} wasn't found");
                }
                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while trying to retrieve villa with id {id}: {ex.Message}");
            }
        }
        [HttpPost("CreateVilla")]

        public async Task<ActionResult<Villa>> CreateVilla(VillaCreateDTO villaDTO)
        {
            try
            {

                if (villaDTO == null)
                {
                    return BadRequest("Villa id must be greater than 0");
                }
                Villa villa = new()
                {
                    Name = villaDTO.Name,
                    Details = villaDTO.Details,
                    ImageUrl = villaDTO.ImageUrl,
                    Rate = villaDTO.Rate,
                    Occupancy = villaDTO.Occupancy,
                    Sqft = villaDTO.Sqft,
                    CreatedDate = DateTime.Now
                };
                await _context.AddAsync(villa);
                await _context.SaveChangesAsync();
                    
                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while trying to create villa.: {ex.Message}");
            }
        }
    }
}

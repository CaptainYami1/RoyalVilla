using AutoMapper;
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

        private readonly IMapper _mapper;

        public RoyalVillaController(RoyalVilleDbContext context, IMapper mapper) { 
        _context = context;
        _mapper = mapper; 
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
                var duplicateVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());
                
                if (duplicateVilla != null)
                {
                    return Conflict($"Villa with name {villaDTO.Name} already exist.");
                }
                Villa villa = _mapper.Map < Villa>(villaDTO);
                
                await _context.AddAsync(villa);
                await _context.SaveChangesAsync();
                    
                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while trying to create villa.: {ex.Message}");
            }
        }

        [HttpPut("updateVilla/{id}")]

        public async Task<ActionResult<Villa>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
        {
            try
            {

                if (villaDTO == null)
                {
                    return BadRequest("Villa id is required");
                }

                if (id != villaDTO.Id)
                {
                    return BadRequest("Villa id doesnt match villa id in request body. ");
                }
                var existingVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound($"Villa with id {id} wasn't found");
                }

                var duplicateVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());

                if (duplicateVilla != null)
                {
                    return Conflict($"Villa with name {villaDTO.Name} already exist.");
                }
                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now; 
                await _context.SaveChangesAsync();

                return Ok(villaDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while trying to create villa.: {ex.Message}");
            }
        }

        [HttpDelete("deleteVilla/{id}")]

        public async Task<ActionResult<Villa>> DeleteVilla(int id)
        {
            try
            {
                var existingVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return BadRequest("Villa id is required");
                }

                if (id != existingVilla.Id)
                {
                    return BadRequest("Villa id doesnt match villa id in request body.");
                }
                
               
                _context.Villas.Remove(existingVilla);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while trying to create villa.: {ex.Message}");
            }
        }
    }
}

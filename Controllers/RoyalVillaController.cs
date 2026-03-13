using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Context;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTOs;
using System.Collections.Generic;
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
        public async Task<ActionResult <ApiResponse<IEnumerable<VillaDTO>>>> GetVillas(int id)
        {
            var villas = await _context.Villas.ToListAsync();
            var dtoResponse = _mapper.Map<List<VillaDTO>>(villas);
            var response = ApiResponse<IEnumerable<VillaDTO>>.Ok(dtoResponse, "Villas retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillasById(int id)
        {
            try
            {
                
                if (id <= 0)
                {

                    return BadRequest(ApiResponse<VillaDTO>.BadRequest("Villa id must be greater than 0"));
                    
                }
                var villa = await _context.Villas.FirstOrDefaultAsync(u => u.Id ==  id);
                if (villa == null)
                {
                    return NotFound(ApiResponse<VillaDTO>.NotFound($"Villa with id {id} wasn't found"));
                   
                }

                var dtoResponse = _mapper.Map<VillaDTO>(villa);
                var response = ApiResponse<VillaDTO>.Ok(dtoResponse, "Records Retrieved Successfully");
                return Ok(response);
               
            }
            catch (Exception ex)
            {
            //    return Error(ApiResponse<VillaDTO>.Error(StatusCodes.Status500InternalServerError, $"An error occured while trying to retrieve villa with id {id}:{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while trying to retrieve villa with id {id}: {ex.Message}");
            }
        }
        [HttpPost("CreateVilla")]

        public async Task<ActionResult<ApiResponse<VillaDTO>>> CreateVilla(VillaCreateDTO villaDTO)
        {
            try
            {

                if (villaDTO == null)
                {
                    return BadRequest(ApiResponse<VillaDTO>.BadRequest("Villa id must be greater than 0"));
                }
                var duplicateVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());
                
                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<VillaDTO>.Conflict($"Villa with name {villaDTO.Name} already exist.")) ;
                }
                Villa villa = _mapper.Map < Villa>(villaDTO);
                
                await _context.AddAsync(villa);
                await _context.SaveChangesAsync();

                var dtoResponse = _mapper.Map<VillaCreateDTO>(villa);
                var response = ApiResponse<VillaCreateDTO>.Ok(dtoResponse, "Records Created Successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occured while trying to create villa.:", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("updateVilla/{id}")]

        public async Task<ActionResult<ApiResponse<VillaUpdateDTO>>> UpdateVilla(int id, VillaDTO villaDTO)
        {
            try
            {

                if (villaDTO == null)
                {
                    return BadRequest(ApiResponse<VillaDTO>.BadRequest("Villa id is required"));
                }

                if (id != villaDTO.Id)
                {
                    return BadRequest(ApiResponse<VillaDTO>.BadRequest("Villa id doesnt match villa id in request body."));
                }
                var existingVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<VillaDTO>.NotFound($"Villa with id {id} wasn't found"));
                }

                var duplicateVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());

                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<VillaDTO>.Conflict($"Villa with name {villaDTO.Name} already exist."));
                }
                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now; 
                await _context.SaveChangesAsync();

                
                var response = ApiResponse<VillaDTO>.Ok(villaDTO, "Records Updated Successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occured while trying to update villa.:", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("deleteVilla/{id}")]

        public async Task<ActionResult<ApiResponse<VillaDTO>>> DeleteVilla(int id)
        {
            try
            {
                var existingVilla = await _context.Villas.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla == null)
                {
                    return BadRequest(ApiResponse<VillaDTO>.BadRequest("Villa id is required"));
                }

                if (id != existingVilla.Id)
                {
                    return BadRequest(ApiResponse<VillaDTO>.BadRequest("Villa id doesnt match villa id in request body."));
                }
                
               
                _context.Villas.Remove(existingVilla);
                await _context.SaveChangesAsync();

                var response = ApiResponse<VillaDTO>.Ok(null!,"Record deleted Successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occured while trying to delete villa.:", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}

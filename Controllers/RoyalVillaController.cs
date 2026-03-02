using Microsoft.AspNetCore.Mvc;

namespace RoyalVilla_API.Controllers
{
    [Route("villa")]
    [ApiController]
    public class RoyalVillaController : ControllerBase
    {
        [HttpGet]
        public string GetVillas()
        {
            return "Get All Villas";
        }

        [HttpGet("GetVillasById{id}")]
        public string GetVillasById(int id)
        {
            return ("Get Vila " + id );
        }
    }
}

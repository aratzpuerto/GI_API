using GI_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GI_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public DbController(IConfiguration configuration) { _configuration = configuration; }

        [HttpPost("ResetSeed")]
        public async Task<ActionResult> ResetSeed(string tableName, int newSeedId, IConfiguration configuration)
        {
            try
            {

                int newId = await DbService.ResetSeed(tableName, newSeedId, _configuration);
                return Ok(new
                {
                    success = true,
                    message = "Database reseeded successfully",
                    id = newId,
                    table = tableName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while reseeding the database",
                    error = ex.Message
                });
            }
        }

    }
}

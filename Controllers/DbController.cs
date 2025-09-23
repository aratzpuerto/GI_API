using GI_API.Contracts;
using GI_API.Models;
using GI_API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GI_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private readonly ILoggerService _logger;
        public readonly IConfiguration _configuration;
        public DbController(IConfiguration configuration, ILoggerService logger) { 
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("ResetSeed")]
        public async Task<ActionResult> ResetSeed(string tableName, int newSeedId, IConfiguration configuration)
        {
            ActionResult result;

            _logger.LogInfo(string.Format("ResetSeed/{0}/{1}:",tableName, newSeedId));

            int newId = await DbService.ResetSeed(tableName, newSeedId, _configuration);

            return Ok(new
            {
                success = true,
                message = "Database reseeded successfully",
                id = newId,
                table = tableName
            });
            
        }

    }
}

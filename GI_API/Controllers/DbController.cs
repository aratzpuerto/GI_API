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
        public readonly DbService _service;
        public DbController(DbService service, ILoggerService logger) { 
            _service = service;
            _logger = logger;
        }

        [HttpPost("ResetSeed")]
        public async Task<ActionResult> ResetSeed(string tableName, int newSeedId, IConfiguration configuration)
        {
            _logger.LogInfo(string.Format("ResetSeed/{0}/{1}:",tableName, newSeedId));

            var (tableExists, newId) = await _service.ResetSeed(tableName, newSeedId);

            if (!tableExists)
                return NotFound(new
                {
                    success = false,
                    message = $"Table '{tableName}' does not exist."
                });

            return Ok(new
            {
                success = true,
                message = "Database reseeded successfully",
                id = newId,
                table = tableName
            });
            
        }

        [HttpGet("CurrentIdentity")]
        public async Task<ActionResult> GetCurrentIdentity(string tableName)
        {
            _logger.LogInfo($"GetCurrentIdentity/{tableName}");

            var (tableExists, currentId) = await _service.GetCurrentIdentity(tableName);

            if (!tableExists)
                return NotFound(new
                {
                    success = false,
                    message = $"Table '{tableName}' does not exist."
                });

            return Ok(new
            {
                success = true,
                table = tableName,
                currentIdentity = currentId
            });
        }


    }
}

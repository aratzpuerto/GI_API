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
            try
            {
                _logger.LogInfo(string.Format("ResetSeed/{0}/{1}:",tableName, newSeedId));

                int newId = await DbService.ResetSeed(tableName, newSeedId, _configuration);

                result = Ok(new
                {
                    success = true,
                    message = "Database reseeded successfully",
                    id = newId,
                    table = tableName
                });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while reseeding the database",
                    error = ex.Message
                });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());
                _logger.LogInfo(ex.Message);

                return result;
            }
        }

    }
}

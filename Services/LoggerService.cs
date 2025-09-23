using GI_API.Contracts;
using Microsoft.IdentityModel.Tokens;
using NLog;
using ILogger = NLog.ILogger;

namespace GI_API.Services
{
    public class LoggerService : ILoggerService
    {
        //private static ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message) => _logger.LogInformation(message);
        public void LogWarn(string message) => _logger.LogWarning(message);
        public void LogDebug(string message) => _logger.LogDebug(message);
        public void LogError(string message) => _logger.LogError(message);
        public void LogFatal(string message) => _logger.LogCritical(message);

    }
}

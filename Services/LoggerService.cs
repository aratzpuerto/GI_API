using GI_API.Contracts;
using Microsoft.IdentityModel.Tokens;
using NLog;
using ILogger = NLog.ILogger;

namespace GI_API.Services
{
    public class LoggerService : ILoggerService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }
        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogFatal(string message)
        {
            _logger.Fatal(message);
        }
    }
}

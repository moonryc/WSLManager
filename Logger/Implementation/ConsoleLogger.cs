using System.Diagnostics;
using WSLManager.Logger.Core;

namespace WSLManager.Logger.Implementation
{
    public class ConsoleLogger:ILogger
    {
        /// <summary>
        /// Logs the given message to the system Console
        /// </summary>
        /// <param name="message">Message to Log</param>
        /// <param name="level">The Level of the Message</param>
        public void Log(string message, LogLevel level)
        {
            //Write Message to console
            Trace.WriteLine($"[{level.ToString()}]" + message);
        }
    }
}
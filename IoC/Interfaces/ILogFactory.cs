using System;
using WSLManager.Logger.Core;

namespace WSLManager.IoC.Interfaces
{
    /// <summary>
    /// Holds a bunch of loggers to log messages for the user
    /// </summary>
    public interface ILogFactory
    {
        #region Events

        event Action<(string message, LogLevel level)> NewLog;

        #endregion

        #region Properties
        
        /// <summary>
        /// The level of logging to output
        /// </summary>
        LogLevel LogOutputLevel { get; set; }
        
        /// <summary>
        /// If true, includes the origin of where the log message was logged
        /// from such as the class name, line number and file name 
        /// </summary>
        bool IncludeLogOriginDetails { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specific logger to this factory
        /// </summary>
        /// <param name="logger">The logger</param>
        void AddLogger(ILogger logger);

        /// <summary>
        /// Removes the specific logger from this factory
        /// </summary>
        /// <param name="logger">The logger</param>
        void RemoveLogger(ILogger logger);

        /// <summary>
        /// Logs the specific message to all loggers in this factory
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level of message to be logged</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">the filename that this was logged from</param>
        /// <param name="lineNumber">The line of code in the filename that this was logged on</param>
        void Log(string message, LogLevel level = LogLevel.Informative, string origin = "", string filePath = "", int lineNumber = 0);

        #endregion
        
        T Get<T>();
    }
}
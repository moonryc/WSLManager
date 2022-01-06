using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using WSLManager.IoC.Interfaces;
using WSLManager.Logger.Core;

namespace WSLManager.Logger.Implementation
{
    /// <summary>
    /// The standard log factory
    /// Logs details to the console, by default
    /// </summary>
    public class BaseLogFactory : ILogFactory
    {

        #region Protected Methods

        /// <summary>
        /// The list of loggers in this factory
        /// </summary>
        protected List<ILogger> mLoggers = new List<ILogger>();

        /// <summary>
        /// A lock for the logger list to keep it thread safe
        /// </summary>
        protected object mLoggersLock = new object();

        #endregion
        
        #region Public Properties

        /// <summary>
        /// Level of logging to output
        /// </summary>
        public LogLevel LogOutputLevel { get; set; }

        /// <summary>
        /// If true, includes the origin of where the log message was logged
        /// from such as the class name, line number and file name 
        /// </summary>
        public bool IncludeLogOriginDetails { get; set; } = true;

        #endregion

        #region Public Events

        /// <summary>
        /// Fires when a new log arrives
        /// </summary>
        public event Action<(string message, LogLevel level)> NewLog = details => { };

        #endregion

        #region Constructor

        public BaseLogFactory()
        {
            //adds console logger by default
            AddLogger((new ConsoleLogger()));
        }

        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Adds the specific logger to this factory
        /// </summary>
        /// <param name="logger">The logger</param>
        public void AddLogger(ILogger logger)
        {
            //Log the list so that it is thread-safe
            lock (mLoggersLock)
            {
                //if the logger is not already in the list
                if (!mLoggers.Contains(logger))
                {
                    //adds the logger to the list
                    mLoggers.Add(logger);    
                }
            }
        }

        /// <summary>
        /// Removes the specific logger from this factory
        /// </summary>
        /// <param name="logger">The logger</param>
        public void RemoveLogger(ILogger logger)
        {
            //Log the list so that it is thread-safe
            lock (mLoggersLock)
            {
                //if the logger is already in the list
                if (mLoggers.Contains(logger))
                {
                    //removes the logger from the list
                    mLoggers.Remove(logger);    
                }
            }
        }

        /// <summary>
        /// Logs the specific message to all loggers in this factory
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The level of message to be logged</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">the filename that this was logged from</param>
        /// <param name="lineNumber">The line of code in the filename that this was logged on</param>
        public void Log(
            string message, 
            LogLevel level = LogLevel.Informative, 
            [CallerMemberName] string origin = "", 
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            //if user wants to know origin of log message
            if (IncludeLogOriginDetails)
            {
                message = $" > [FileName: {Path.GetFileName(filePath)}, LineNumber: {lineNumber}, Origin: {origin}()] > {message}";
            }
            
            // Log to all Loggers
            lock (mLoggersLock)
            {
                mLoggers.ForEach(logger=> logger.Log(message,level));
            }
            
            //inform listeners
            NewLog.Invoke((message,level));
        }
        
        #endregion

        public T Get<T>()
        {
            return default;
        }
    }
}
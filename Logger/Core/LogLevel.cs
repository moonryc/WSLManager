namespace WSLManager.Logger.Core
{
    /// <summary>
    /// The Severity of the Log MEssage
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Logs Everything
        /// </summary>
        Debug,
        
        /// <summary>
        /// Logs all but Debug Messages
        /// </summary>
        Verbose,
        
        /// <summary>
        /// Logs all but Debug and Verbose Messages
        /// </summary>
        Informative,
        
        /// <summary>
        /// Logs only warnings, errors, and standard messages
        /// </summary>
        Normal,
        
        /// <summary>
        /// Logs only critical errors and warnings 
        /// </summary>
        Critical,
        
        /// <summary>
        /// Logs nothing
        /// </summary>
        Nothing
    }
}
namespace CzomPack.Logging
{
    /// <summary>
    /// Log message types
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Debug log type
        /// </summary>
        Debug = 0,

        /// <summary>
        /// Info log type
        /// </summary>
        Info = 1,
        
        /// <summary>
        /// Warning log type
        /// </summary>
        Warning = 2,
        
        /// <summary>
        /// Error log type
        /// </summary>
        Error = 3,
        
        /// <summary>
        /// Fatal Error log type
        /// </summary>
        FatalError = 4,

        /// <summary>
        /// Verbose log type
        /// </summary>
        Verbose = 5
    }
}

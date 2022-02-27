using CzomPack.Coloring;
using CzomPack.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CzomPack.Logging;

/// <summary>
/// Log manager implementation by Czompi Software.
/// </summary>
public class Logger
{

    #region Obsolete
    /// <summary>
    /// Log a <i>Debug</i> type message.
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="ShowInConsole">Show log in console</param>
    [Obsolete("Use Debug(string, params string[]) instead")]
    public static void Debug(string message, bool ShowInConsole) => WriteLine(message, LogType.Debug);

    /// <summary>
    /// Log an <i>Info</i> type message.
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="ShowInConsole">Show log in console</param>
    [Obsolete("Use Info(string, params string[]) instead")]
    public static void Info(string message, bool ShowInConsole) => WriteLine(message, LogType.Info);

    /// <summary>
    /// Log a <i>Warning</i> type message.
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="ShowInConsole">Show log in console</param>
    [Obsolete("Use Warning(string, params string[]) instead")]
    public static void Warning(string message, bool ShowInConsole) => WriteLine(message, LogType.Warning);

    /// <summary>
    /// Log an <i>Error</i> type message.
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="ShowInConsole">Show log in console</param>
    [Obsolete("Use Error(string, params string[]) instead")]
    public static void Error(string message, bool ShowInConsole) => WriteLine(message, LogType.Error);

    /// <summary>
    /// Log a <i>Fatal Error</i> type message.
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="ShowInConsole">Show log in console</param>
    [Obsolete("Use FatalError(string, params string[]) instead")]
    public static void FatalError(string message, bool ShowInConsole) => WriteLine(message, LogType.FatalError);
    #endregion

    #region Serilog
    /// <summary>
    /// Log an <i>Verbose</i> type message.
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues">Object positionally formatted into the message template.</param>
    public static void Verbose(string messageTemplate, params object[] propertyValues) => WriteLine(messageTemplate, LogType.Verbose, propertyValues);

    /// <summary>
    /// Log a <i>Debug</i> type message.
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues">Object positionally formatted into the message template.</param>
    public static void Debug(string messageTemplate, params object[] propertyValues) => WriteLine(messageTemplate, LogType.Debug, propertyValues);

    /// <summary>
    /// Log an <i>Info</i> type message.
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues">Object positionally formatted into the message template.</param>
    public static void Info(string messageTemplate, params object[] propertyValues) => WriteLine(messageTemplate, LogType.Info, propertyValues);

    /// <summary>
    /// Log a <i>Warning</i> type message.
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues">Object positionally formatted into the message template.</param>
    public static void Warning(string messageTemplate, params object[] propertyValues) => WriteLine(messageTemplate, LogType.Warning, propertyValues);

    /// <summary>
    /// Log an <i>Error</i> type message.
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues">Object positionally formatted into the message template.</param>
    public static void Error(string messageTemplate, params object[] propertyValues) => WriteLine(messageTemplate, LogType.Error, propertyValues);

    /// <summary>
    /// Log a <i>Fatal Error</i> type message.
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="propertyValues">Object positionally formatted into the message template.</param>
    public static void FatalError(string messageTemplate, params object[] propertyValues) => WriteLine(messageTemplate, LogType.FatalError, propertyValues);
    #endregion

    #region WriteLine - Global write function
    private static void WriteLine(string messageTemplate, LogType Type = LogType.Info, params object[] propertyValues)
    {
        Globals.SetupLogger();

        Log.Logger = Log.Logger.ForContext("SourceContext", new StackTrace().GetFrame(2).GetMethod().DeclaringType.FullName);
        Log.Write(Type.ToLogEventLevel(), messageTemplate, propertyValues);
    }
    #endregion

    public static ILogger GetLogger() => Log.Logger;
}

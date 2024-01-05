using System;
using System.IO;
using System.Text;
using Serilog;
using Sharenian.Api.MapleStory.Models;

namespace Sharenian.Utils;

public class Logger
{
    #region Singleton

    private static readonly Lazy<Logger> LayInstance = new(() => new Logger());

    public static Logger Instance => LayInstance.Value;

    #endregion

    private ILogger Log { get; }

    private Logger()
    {
        const string layout = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}\t[{Level:u3}]\t{Message}{NewLine}{Exception}";
        var logFileDir = Path.Combine(Directory.GetCurrentDirectory(), "Log");

        Log = new LoggerConfiguration()
            .WriteTo.File(@$"{logFileDir}\{DateTime.Now:yyyyMMdd}.log",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 2_000_000,
                retainedFileCountLimit: 100,
                encoding: Encoding.UTF8,
                outputTemplate: layout)
            .CreateLogger();
    }

    public void Error(Exception exception, string message = "") => Log.Error(exception, message);

    public void HttpError(MapleStoryApiError error) => Log.Error($"{error.ErrorMessage.Name} - {error.ErrorMessage.Message}");
}

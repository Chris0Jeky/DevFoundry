using System.Globalization;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class TimestampConverterTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "time.timestamp",
        DisplayName = "Timestamp Converter",
        Description = "Convert between Unix timestamp and human-readable date/time.",
        Category = ToolCategory.Time,
        Tags = new[] { "timestamp", "unix", "date", "time", "convert" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "mode",
                DisplayName = "Conversion Mode",
                Description = "Direction of conversion: 'to-unix' or 'from-unix'",
                Type = "string",
                DefaultValue = "from-unix"
            },
            new ToolParameterDescriptor
            {
                Name = "useMilliseconds",
                DisplayName = "Use Milliseconds",
                Description = "Whether to use milliseconds (true) or seconds (false) for Unix timestamp",
                Type = "bool",
                DefaultValue = false
            }
        }
    };

    public ToolResult Execute(ToolInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Text))
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = "No input text provided."
            };
        }

        var mode = input.Parameters.TryGetValue("mode", out var m) && m is string modeStr
            ? modeStr.ToLowerInvariant()
            : "from-unix";

        var useMilliseconds = input.Parameters.TryGetValue("useMilliseconds", out var ms)
            && ms is bool b && b;

        try
        {
            if (mode == "from-unix")
            {
                return ConvertFromUnix(input.Text.Trim(), useMilliseconds);
            }
            else if (mode == "to-unix")
            {
                return ConvertToUnix(input.Text.Trim(), useMilliseconds);
            }
            else
            {
                return new ToolResult
                {
                    Success = false,
                    ErrorMessage = $"Invalid mode '{mode}'. Use 'to-unix' or 'from-unix'."
                };
            }
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"Conversion error: {ex.Message}"
            };
        }
    }

    private static ToolResult ConvertFromUnix(string timestamp, bool useMilliseconds)
    {
        if (!long.TryParse(timestamp, out var unixTime))
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = "Invalid Unix timestamp. Must be a valid integer."
            };
        }

        DateTimeOffset dateTime;

        if (useMilliseconds)
        {
            dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
        }
        else
        {
            dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        }

        var result = new System.Text.StringBuilder();
        result.AppendLine($"UTC: {dateTime.UtcDateTime:yyyy-MM-dd HH:mm:ss} UTC");
        result.AppendLine($"Local: {dateTime.LocalDateTime:yyyy-MM-dd HH:mm:ss}");
        result.AppendLine($"ISO 8601: {dateTime.UtcDateTime:yyyy-MM-ddTHH:mm:ssZ}");
        result.AppendLine($"Unix (seconds): {dateTime.ToUnixTimeSeconds()}");
        result.AppendLine($"Unix (milliseconds): {dateTime.ToUnixTimeMilliseconds()}");

        return new ToolResult
        {
            Success = true,
            OutputText = result.ToString().TrimEnd()
        };
    }

    private static ToolResult ConvertToUnix(string dateTimeStr, bool useMilliseconds)
    {
        DateTimeOffset dateTime;

        // Try parsing as ISO 8601 first
        if (DateTimeOffset.TryParse(dateTimeStr, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dateTime))
        {
            var unixTime = useMilliseconds
                ? dateTime.ToUnixTimeMilliseconds()
                : dateTime.ToUnixTimeSeconds();

            var result = new System.Text.StringBuilder();
            result.AppendLine($"Input: {dateTime:yyyy-MM-dd HH:mm:ss zzz}");
            result.AppendLine($"Unix ({(useMilliseconds ? "milliseconds" : "seconds")}): {unixTime}");
            result.AppendLine($"UTC: {dateTime.UtcDateTime:yyyy-MM-dd HH:mm:ss} UTC");
            result.AppendLine($"ISO 8601: {dateTime.UtcDateTime:yyyy-MM-ddTHH:mm:ssZ}");

            return new ToolResult
            {
                Success = true,
                OutputText = result.ToString().TrimEnd()
            };
        }

        return new ToolResult
        {
            Success = false,
            ErrorMessage = "Invalid date/time format. Try formats like '2024-01-01 12:00:00' or ISO 8601."
        };
    }
}

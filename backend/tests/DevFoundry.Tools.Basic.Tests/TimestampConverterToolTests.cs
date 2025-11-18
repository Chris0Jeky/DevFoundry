using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class TimestampConverterToolTests
{
    private readonly TimestampConverterTool _tool = new();

    [Fact]
    public void Execute_ToUnix_AssumesUtcWhenOffsetMissing()
    {
        var input = new ToolInput
        {
            Text = "2024-01-01 00:00:00",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "to-unix",
                ["useMilliseconds"] = false
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("Unix (seconds): 1704067200", result.OutputText);
        Assert.Contains("UTC: 2024-01-01 00:00:00 UTC", result.OutputText);
    }

    [Fact]
    public void Execute_FromUnix_ReturnsFormattedRepresentations()
    {
        var input = new ToolInput
        {
            Text = "1704067200",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "from-unix",
                ["useMilliseconds"] = false
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("UTC: 2024-01-01 00:00:00 UTC", result.OutputText);
        Assert.Contains("Unix (milliseconds): 1704067200000", result.OutputText);
    }

    [Fact]
    public void Execute_InvalidDate_ReturnsError()
    {
        var input = new ToolInput
        {
            Text = "not-a-date",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "to-unix"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.Contains("Invalid date/time format", result.ErrorMessage);
    }
}

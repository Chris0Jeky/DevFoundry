using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class UrlEncoderToolTests
{
    private readonly UrlEncoderTool _tool = new();

    [Fact]
    public void Execute_EncodesWithPercentEncoding()
    {
        var input = new ToolInput
        {
            Text = "Hello World/+",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "encode"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("Hello%20World%2F%2B", result.OutputText);
    }

    [Fact]
    public void Execute_DecodesPercentEncodedInput()
    {
        var input = new ToolInput
        {
            Text = "Hello%20World%2F%2B",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "decode"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("Hello World/+", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidEncodedInput()
    {
        var input = new ToolInput
        {
            Text = "%E4%ZZ",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "decode"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.Contains("URL decode error", result.ErrorMessage);
    }
}

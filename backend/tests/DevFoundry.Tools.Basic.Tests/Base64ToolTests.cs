using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class Base64ToolTests
{
    private readonly Base64Tool _tool = new();

    [Fact]
    public void Execute_EncodesText_Successfully()
    {
        var input = new ToolInput
        {
            Text = "hello world",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "encode"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("aGVsbG8gd29ybGQ=", result.OutputText);
    }

    [Fact]
    public void Execute_DecodesBase64_Successfully()
    {
        var input = new ToolInput
        {
            Text = "aGVsbG8gd29ybGQ=",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "decode"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("hello world", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidBase64()
    {
        var input = new ToolInput
        {
            Text = "not valid base64!!!",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "decode"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }
}

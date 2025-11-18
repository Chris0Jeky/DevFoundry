using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class JsonFormatterToolTests
{
    private readonly JsonFormatterTool _tool = new();

    [Fact]
    public void Execute_FormatsValidJson_Successfully()
    {
        var input = new ToolInput
        {
            Text = "{\"name\":\"Alice\",\"age\":30}"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.Contains("\"name\"", result.OutputText);
        Assert.Contains("\"Alice\"", result.OutputText);
    }

    [Fact]
    public void Execute_MinifiesJson_WhenMinifyIsTrue()
    {
        var input = new ToolInput
        {
            Text = "{\n  \"name\": \"Alice\",\n  \"age\": 30\n}",
            Parameters = new Dictionary<string, object?>
            {
                ["minify"] = true
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.DoesNotContain("\n", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidJson()
    {
        var input = new ToolInput
        {
            Text = "{invalid json}"
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("parse error", result.ErrorMessage.ToLower());
    }

    [Fact]
    public void Execute_ReturnsError_WhenNoInputProvided()
    {
        var input = new ToolInput
        {
            Text = null
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }
}

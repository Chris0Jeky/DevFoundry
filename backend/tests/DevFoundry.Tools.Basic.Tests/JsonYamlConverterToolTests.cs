using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class JsonYamlConverterToolTests
{
    private readonly JsonYamlConverterTool _tool = new();

    [Fact]
    public void Execute_ConvertsJsonToYaml_Successfully()
    {
        var input = new ToolInput
        {
            Text = "{\"name\":\"Alice\",\"age\":30}",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "json-to-yaml"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.Contains("name:", result.OutputText);
        Assert.Contains("Alice", result.OutputText);
        Assert.Contains("age:", result.OutputText);
        Assert.Contains("30", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsYamlToJson_Successfully()
    {
        var input = new ToolInput
        {
            Text = "name: Alice\nage: 30",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "yaml-to-json"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.Contains("\"name\"", result.OutputText);
        Assert.Contains("\"Alice\"", result.OutputText);
        Assert.Contains("\"age\"", result.OutputText);
        Assert.Contains("30", result.OutputText);
    }

    [Fact]
    public void Execute_UsesDefaultMode_WhenNotSpecified()
    {
        var input = new ToolInput
        {
            Text = "{\"test\":\"value\"}"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        // Default mode is json-to-yaml, so output should be YAML
        Assert.Contains("test:", result.OutputText);
        Assert.Contains("value", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidMode()
    {
        var input = new ToolInput
        {
            Text = "{\"test\":\"value\"}",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "invalid-mode"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("Invalid mode", result.ErrorMessage);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidJson()
    {
        var input = new ToolInput
        {
            Text = "{invalid json}",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "json-to-yaml"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("error", result.ErrorMessage.ToLower());
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidYaml()
    {
        var input = new ToolInput
        {
            Text = ":\ninvalid: yaml: structure:",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "yaml-to-json"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
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
        Assert.Contains("No input", result.ErrorMessage);
    }

    [Fact]
    public void Execute_HandlesCaseInsensitiveMode()
    {
        var input = new ToolInput
        {
            Text = "{\"test\":\"value\"}",
            Parameters = new Dictionary<string, object?>
            {
                ["mode"] = "JSON-TO-YAML"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
    }
}

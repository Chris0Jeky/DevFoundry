using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class ColorConverterToolTests
{
    private readonly ColorConverterTool _tool = new();

    [Fact]
    public void Execute_ConvertsHexToRgb_Successfully()
    {
        var input = new ToolInput
        {
            Text = "#FF5733",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "rgb"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("rgb(255, 87, 51)", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsShortHexToRgb_Successfully()
    {
        var input = new ToolInput
        {
            Text = "#F53",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "rgb"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("rgb(255, 85, 51)", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsRgbToHex_Successfully()
    {
        var input = new ToolInput
        {
            Text = "rgb(255, 87, 51)",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "hex"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("#FF5733", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsRgbaToHex_Successfully()
    {
        var input = new ToolInput
        {
            Text = "rgba(255, 87, 51, 0.5)",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "hex"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("#FF573380", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsRgbToHsl_Successfully()
    {
        var input = new ToolInput
        {
            Text = "rgb(255, 87, 51)",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "hsl"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("hsl(", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsHslToRgb_Successfully()
    {
        var input = new ToolInput
        {
            Text = "hsl(11, 100%, 60%)",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "rgb"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.StartsWith("rgb(", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsRgbaToRgba_Successfully()
    {
        var input = new ToolInput
        {
            Text = "rgba(255, 87, 51, 0.75)",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "rgba"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("rgba(255, 87, 51, 0.75)", result.OutputText);
    }

    [Fact]
    public void Execute_ConvertsHslaToHsla_Successfully()
    {
        var input = new ToolInput
        {
            Text = "hsla(11, 100%, 60%, 0.5)",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "hsla"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.StartsWith("hsla(", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidHex()
    {
        var input = new ToolInput
        {
            Text = "#GGGGGG",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "rgb"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidFormat()
    {
        var input = new ToolInput
        {
            Text = "invalid color",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "rgb"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.Contains("Unrecognized color format", result.ErrorMessage!);
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
        Assert.Contains("No color value", result.ErrorMessage!);
    }

    [Fact]
    public void Execute_UsesDefaultTargetFormat_WhenNotSpecified()
    {
        var input = new ToolInput
        {
            Text = "#FF5733"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.StartsWith("rgb(", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidTargetFormat()
    {
        var input = new ToolInput
        {
            Text = "#FF5733",
            Parameters = new Dictionary<string, object?>
            {
                ["targetFormat"] = "invalid"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.Contains("Unknown target format", result.ErrorMessage!);
    }
}

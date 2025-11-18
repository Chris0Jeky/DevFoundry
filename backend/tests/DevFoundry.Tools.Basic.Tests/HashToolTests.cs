using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class HashToolTests
{
    private readonly HashTool _tool = new();

    [Fact]
    public void Execute_CalculatesMD5Hash_Successfully()
    {
        var input = new ToolInput
        {
            Text = "hello",
            Parameters = new Dictionary<string, object?>
            {
                ["algorithm"] = "md5"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("5d41402abc4b2a76b9719d911017c592", result.OutputText);
    }

    [Fact]
    public void Execute_CalculatesSHA256Hash_ByDefault()
    {
        var input = new ToolInput
        {
            Text = "hello"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.Equal(64, result.OutputText.Length); // SHA-256 is 64 hex characters
    }

    [Fact]
    public void Execute_CalculatesUppercaseHash_WhenRequested()
    {
        var input = new ToolInput
        {
            Text = "hello",
            Parameters = new Dictionary<string, object?>
            {
                ["algorithm"] = "sha256",
                ["uppercase"] = true
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.Equal(result.OutputText, result.OutputText.ToUpperInvariant());
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidAlgorithm()
    {
        var input = new ToolInput
        {
            Text = "hello",
            Parameters = new Dictionary<string, object?>
            {
                ["algorithm"] = "invalid"
            }
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }
}

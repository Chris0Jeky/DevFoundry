using DevFoundry.Core;
using Xunit;

namespace DevFoundry.Core.Tests;

public class ToolResultTests
{
    [Fact]
    public void ToolResult_CanBeCreated_WithSuccessTrue()
    {
        var result = new ToolResult
        {
            Success = true,
            OutputText = "test output"
        };

        Assert.True(result.Success);
        Assert.Equal("test output", result.OutputText);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void ToolResult_CanBeCreated_WithSuccessFalse()
    {
        var result = new ToolResult
        {
            Success = false,
            ErrorMessage = "test error"
        };

        Assert.False(result.Success);
        Assert.Equal("test error", result.ErrorMessage);
        Assert.Null(result.OutputText);
    }
}

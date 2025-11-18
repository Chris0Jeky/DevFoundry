using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class JwtDecoderToolTests
{
    private readonly JwtDecoderTool _tool = new();

    [Fact]
    public void Execute_DecodesValidToken()
    {
        const string token = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0."
            + "eyJzdWIiOiIxMjMiLCJuYW1lIjoiSm9obiBEb2UifQ."
            + "c2lnbmF0dXJl";

        var input = new ToolInput { Text = token };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("\"alg\": \"none\"", result.OutputText);
        Assert.Contains("\"name\": \"John Doe\"", result.OutputText);
        Assert.Contains("=== SIGNATURE ===", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_ForInvalidBase64Segment()
    {
        var input = new ToolInput { Text = "abc.def.ghi" };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.Contains("JWT decode error", result.ErrorMessage);
    }
}

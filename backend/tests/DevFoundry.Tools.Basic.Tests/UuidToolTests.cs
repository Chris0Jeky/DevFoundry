using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class UuidToolTests
{
    private readonly UuidTool _tool = new();

    [Fact]
    public void Execute_GeneratesSingleUuid_ByDefault()
    {
        var input = new ToolInput();

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.True(Guid.TryParse(result.OutputText, out _));
    }

    [Fact]
    public void Execute_GeneratesMultipleUuids_WhenCountSpecified()
    {
        var input = new ToolInput
        {
            Parameters = new Dictionary<string, object?>
            {
                ["count"] = 5
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);

        var uuids = result.OutputText.Split(Environment.NewLine);
        Assert.Equal(5, uuids.Length);

        foreach (var uuid in uuids)
        {
            Assert.True(Guid.TryParse(uuid, out _));
        }
    }

    [Fact]
    public void Execute_GeneratesUppercaseUuids_WhenRequested()
    {
        var input = new ToolInput
        {
            Parameters = new Dictionary<string, object?>
            {
                ["uppercase"] = true
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
        Assert.Equal(result.OutputText, result.OutputText.ToUpperInvariant());
    }
}

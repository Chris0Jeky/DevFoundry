using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class TextDiffToolTests
{
    private readonly TextDiffTool _tool = new();

    [Fact]
    public void Execute_ShowsNoDifference_WhenTextsAreIdentical()
    {
        var input = new ToolInput
        {
            Text = "Hello World\nLine 2",
            SecondaryText = "Hello World\nLine 2"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("Hello World", result.OutputText);
        Assert.DoesNotContain("-", result.OutputText);
        Assert.DoesNotContain("+", result.OutputText);
    }

    [Fact]
    public void Execute_ShowsDeletions_InUnifiedFormat()
    {
        var input = new ToolInput
        {
            Text = "Line 1\nLine 2\nLine 3",
            SecondaryText = "Line 1\nLine 3",
            Parameters = new Dictionary<string, object?>
            {
                ["format"] = "unified"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("- Line 2", result.OutputText);
    }

    [Fact]
    public void Execute_ShowsAdditions_InUnifiedFormat()
    {
        var input = new ToolInput
        {
            Text = "Line 1\nLine 3",
            SecondaryText = "Line 1\nLine 2\nLine 3",
            Parameters = new Dictionary<string, object?>
            {
                ["format"] = "unified"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("+ Line 2", result.OutputText);
    }

    [Fact]
    public void Execute_ShowsChanges_InUnifiedFormat()
    {
        var input = new ToolInput
        {
            Text = "Line 1\nOld Line\nLine 3",
            SecondaryText = "Line 1\nNew Line\nLine 3",
            Parameters = new Dictionary<string, object?>
            {
                ["format"] = "unified"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("- Old Line", result.OutputText);
        Assert.Contains("+ New Line", result.OutputText);
    }

    [Fact]
    public void Execute_UsesSideBySideFormat_WhenSpecified()
    {
        var input = new ToolInput
        {
            Text = "Line 1",
            SecondaryText = "Line 2",
            Parameters = new Dictionary<string, object?>
            {
                ["format"] = "sidebyside"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("Original", result.OutputText);
        Assert.Contains("Modified", result.OutputText);
        Assert.Contains("|", result.OutputText);
    }

    [Fact]
    public void Execute_IgnoresWhitespace_WhenEnabled()
    {
        var input = new ToolInput
        {
            Text = "Hello  World",
            SecondaryText = "Hello World",
            Parameters = new Dictionary<string, object?>
            {
                ["ignoreWhitespace"] = true
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.DoesNotContain("-", result.OutputText);
        Assert.DoesNotContain("+", result.OutputText);
    }

    [Fact]
    public void Execute_IgnoresCase_WhenEnabled()
    {
        var input = new ToolInput
        {
            Text = "HELLO WORLD",
            SecondaryText = "hello world",
            Parameters = new Dictionary<string, object?>
            {
                ["ignoreCase"] = true
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.DoesNotContain("-", result.OutputText);
        Assert.DoesNotContain("+", result.OutputText);
    }

    [Fact]
    public void Execute_ReturnsError_WhenFirstTextMissing()
    {
        var input = new ToolInput
        {
            Text = null,
            SecondaryText = "Some text"
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.Contains("first input", result.ErrorMessage!);
    }

    [Fact]
    public void Execute_ReturnsError_WhenSecondTextMissing()
    {
        var input = new ToolInput
        {
            Text = "Some text",
            SecondaryText = null
        };

        var result = _tool.Execute(input);

        Assert.False(result.Success);
        Assert.Contains("second input", result.ErrorMessage!);
    }

    [Fact]
    public void Execute_HandlesEmptyLines_Correctly()
    {
        var input = new ToolInput
        {
            Text = "Line 1\n\nLine 3",
            SecondaryText = "Line 1\nLine 2\nLine 3"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.NotNull(result.OutputText);
    }

    [Fact]
    public void Execute_UsesUnifiedFormat_ByDefault()
    {
        var input = new ToolInput
        {
            Text = "Line 1",
            SecondaryText = "Line 2"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("---", result.OutputText);
        Assert.Contains("+++", result.OutputText);
    }

    [Fact]
    public void Execute_HandlesMultipleChanges_Correctly()
    {
        var input = new ToolInput
        {
            Text = "A\nB\nC\nD\nE",
            SecondaryText = "A\nX\nC\nY\nE"
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Contains("- B", result.OutputText);
        Assert.Contains("+ X", result.OutputText);
        Assert.Contains("- D", result.OutputText);
        Assert.Contains("+ Y", result.OutputText);
    }
}

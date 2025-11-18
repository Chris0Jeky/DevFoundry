using DevFoundry.Core;
using DevFoundry.Tools.Basic;
using Xunit;

namespace DevFoundry.Tools.Basic.Tests;

public class StringCaseConverterToolTests
{
    private readonly StringCaseConverterTool _tool = new();

    [Fact]
    public void Execute_SplitsPunctuationAndDigits_ForSnakeCase()
    {
        var input = new ToolInput
        {
            Text = "version2.0-release",
            Parameters = new Dictionary<string, object?>
            {
                ["targetCase"] = "snake"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("version2_0_release", result.OutputText);
    }

    [Fact]
    public void Execute_KeepsWords_WhenConvertingToCamelCase()
    {
        var input = new ToolInput
        {
            Text = "Hello-world.test 123Example",
            Parameters = new Dictionary<string, object?>
            {
                ["targetCase"] = "camel"
            }
        };

        var result = _tool.Execute(input);

        Assert.True(result.Success);
        Assert.Equal("helloWorldTest123Example", result.OutputText);
    }
}

using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class UuidTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "generation.uuid",
        DisplayName = "UUID Generator",
        Description = "Generate random UUIDs (v4).",
        Category = ToolCategory.Generation,
        Tags = new[] { "uuid", "guid", "generate", "random" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "count",
                DisplayName = "Count",
                Description = "Number of UUIDs to generate",
                Type = "int",
                DefaultValue = 1
            },
            new ToolParameterDescriptor
            {
                Name = "uppercase",
                DisplayName = "Uppercase",
                Description = "Generate UUIDs in uppercase",
                Type = "bool",
                DefaultValue = false
            }
        }
    };

    public ToolResult Execute(ToolInput input)
    {
        var count = input.Parameters.TryGetValue("count", out var c) && c is int cnt && cnt > 0
            ? cnt
            : 1;

        var uppercase = input.Parameters.TryGetValue("uppercase", out var u) && u is bool upper && upper;

        if (count > 100)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = "Maximum count is 100 UUIDs."
            };
        }

        var uuids = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var uuid = Guid.NewGuid().ToString();
            uuids.Add(uppercase ? uuid.ToUpperInvariant() : uuid);
        }

        return new ToolResult
        {
            Success = true,
            OutputText = string.Join(Environment.NewLine, uuids)
        };
    }
}

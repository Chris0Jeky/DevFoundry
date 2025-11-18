using System.Text;
using System.Text.Json;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class JsonFormatterTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "json.formatter",
        DisplayName = "JSON Formatter",
        Description = "Pretty-print or minify JSON.",
        Category = ToolCategory.DataFormat,
        Tags = new[] { "json", "format", "minify", "beautify" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "indentSize",
                DisplayName = "Indent Size",
                Description = "Number of spaces for indentation.",
                Type = "int",
                DefaultValue = 2
            },
            new ToolParameterDescriptor
            {
                Name = "minify",
                DisplayName = "Minify",
                Description = "Remove whitespace instead of formatting.",
                Type = "bool",
                DefaultValue = false
            }
        }
    };

    public ToolResult Execute(ToolInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Text))
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = "No input text provided."
            };
        }

        try
        {
            using var doc = JsonDocument.Parse(input.Text);

            var minify = input.Parameters.TryGetValue("minify", out var m) && m is bool b && b;
            var indentSize = input.Parameters.TryGetValue("indentSize", out var i) && i is int n ? n : 2;

            var options = new JsonWriterOptions
            {
                Indented = !minify
            };

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, options))
            {
                doc.WriteTo(writer);
            }

            var resultText = Encoding.UTF8.GetString(stream.ToArray());

            // Apply custom indentation if needed (default is 2 spaces)
            if (!minify && indentSize != 2)
            {
                resultText = AdjustIndentation(resultText, indentSize);
            }

            return new ToolResult
            {
                Success = true,
                OutputText = resultText
            };
        }
        catch (JsonException ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"JSON parse error: {ex.Message}"
            };
        }
    }

    private static string AdjustIndentation(string json, int indentSize)
    {
        if (indentSize == 2) return json;

        var lines = json.Split('\n');
        var result = new StringBuilder();

        foreach (var line in lines)
        {
            var leadingSpaces = line.TakeWhile(c => c == ' ').Count();
            if (leadingSpaces > 0)
            {
                var indentLevel = leadingSpaces / 2;
                var newIndent = new string(' ', indentLevel * indentSize);
                result.AppendLine(newIndent + line.TrimStart());
            }
            else
            {
                result.AppendLine(line);
            }
        }

        return result.ToString().TrimEnd();
    }
}

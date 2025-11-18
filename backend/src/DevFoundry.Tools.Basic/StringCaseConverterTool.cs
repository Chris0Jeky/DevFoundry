using System.Text;
using System.Text.RegularExpressions;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class StringCaseConverterTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "text.case",
        DisplayName = "String Case Converter",
        Description = "Convert strings between different case formats (camelCase, PascalCase, snake_case, kebab-case, etc.).",
        Category = ToolCategory.Other,
        Tags = new[] { "string", "case", "camel", "pascal", "snake", "kebab", "convert" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "targetCase",
                DisplayName = "Target Case",
                Description = "Target case format: 'camel', 'pascal', 'snake', 'kebab', 'upper', 'lower', 'title'",
                Type = "string",
                DefaultValue = "camel"
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

        var targetCase = input.Parameters.TryGetValue("targetCase", out var tc) && tc is string tcStr
            ? tcStr.ToLowerInvariant()
            : "camel";

        try
        {
            var result = targetCase switch
            {
                "camel" => ToCamelCase(input.Text),
                "pascal" => ToPascalCase(input.Text),
                "snake" => ToSnakeCase(input.Text),
                "kebab" => ToKebabCase(input.Text),
                "upper" => input.Text.ToUpperInvariant(),
                "lower" => input.Text.ToLowerInvariant(),
                "title" => ToTitleCase(input.Text),
                _ => throw new ArgumentException($"Invalid target case '{targetCase}'. Use 'camel', 'pascal', 'snake', 'kebab', 'upper', 'lower', or 'title'.")
            };

            return new ToolResult
            {
                Success = true,
                OutputText = result
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"Conversion error: {ex.Message}"
            };
        }
    }

    private static string[] SplitWords(string input)
    {
        // Handle common separators and case transitions
        var withSpaces = Regex.Replace(input, @"([a-z])([A-Z])", "$1 $2"); // camelCase -> camel Case
        withSpaces = Regex.Replace(withSpaces, @"([A-Z]+)([A-Z][a-z])", "$1 $2"); // XMLParser -> XML Parser
        withSpaces = withSpaces.Replace("_", " ").Replace("-", " "); // snake_case, kebab-case

        return withSpaces.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static string ToCamelCase(string input)
    {
        var words = SplitWords(input);
        if (words.Length == 0) return string.Empty;

        var result = new StringBuilder();
        result.Append(words[0].ToLowerInvariant());

        for (int i = 1; i < words.Length; i++)
        {
            result.Append(char.ToUpperInvariant(words[i][0]));
            if (words[i].Length > 1)
            {
                result.Append(words[i].Substring(1).ToLowerInvariant());
            }
        }

        return result.ToString();
    }

    private static string ToPascalCase(string input)
    {
        var words = SplitWords(input);
        var result = new StringBuilder();

        foreach (var word in words)
        {
            if (word.Length > 0)
            {
                result.Append(char.ToUpperInvariant(word[0]));
                if (word.Length > 1)
                {
                    result.Append(word.Substring(1).ToLowerInvariant());
                }
            }
        }

        return result.ToString();
    }

    private static string ToSnakeCase(string input)
    {
        var words = SplitWords(input);
        return string.Join("_", words.Select(w => w.ToLowerInvariant()));
    }

    private static string ToKebabCase(string input)
    {
        var words = SplitWords(input);
        return string.Join("-", words.Select(w => w.ToLowerInvariant()));
    }

    private static string ToTitleCase(string input)
    {
        var words = SplitWords(input);
        return string.Join(" ", words.Select(w =>
        {
            if (w.Length == 0) return w;
            return char.ToUpperInvariant(w[0]) + (w.Length > 1 ? w.Substring(1).ToLowerInvariant() : "");
        }));
    }
}

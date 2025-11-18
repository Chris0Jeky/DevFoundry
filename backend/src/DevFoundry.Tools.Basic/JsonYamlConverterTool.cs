using System.Text.Json;
using DevFoundry.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DevFoundry.Tools.Basic;

public sealed class JsonYamlConverterTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "json.yaml",
        DisplayName = "JSON ⇄ YAML Converter",
        Description = "Convert between JSON and YAML formats.",
        Category = ToolCategory.DataFormat,
        Tags = new[] { "json", "yaml", "convert" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "mode",
                DisplayName = "Conversion Mode",
                Description = "Direction of conversion: 'json-to-yaml' or 'yaml-to-json'",
                Type = "string",
                DefaultValue = "json-to-yaml"
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

        var mode = input.Parameters.TryGetValue("mode", out var m) && m is string modeStr
            ? modeStr.ToLowerInvariant()
            : "json-to-yaml";

        try
        {
            if (mode == "json-to-yaml")
            {
                return ConvertJsonToYaml(input.Text);
            }
            else if (mode == "yaml-to-json")
            {
                return ConvertYamlToJson(input.Text);
            }
            else
            {
                return new ToolResult
                {
                    Success = false,
                    ErrorMessage = $"Invalid mode '{mode}'. Use 'json-to-yaml' or 'yaml-to-json'."
                };
            }
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

    private static ToolResult ConvertJsonToYaml(string json)
    {
        var jsonObject = JsonSerializer.Deserialize<object>(json);

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yaml = serializer.Serialize(jsonObject);

        return new ToolResult
        {
            Success = true,
            OutputText = yaml
        };
    }

    private static ToolResult ConvertYamlToJson(string yaml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = deserializer.Deserialize<object>(yaml);

        var json = JsonSerializer.Serialize(yamlObject, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return new ToolResult
        {
            Success = true,
            OutputText = json
        };
    }
}

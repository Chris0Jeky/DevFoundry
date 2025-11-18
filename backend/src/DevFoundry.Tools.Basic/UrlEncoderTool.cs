using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class UrlEncoderTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "encoding.url",
        DisplayName = "URL Encoder/Decoder",
        Description = "Encode or decode URL strings (percent encoding).",
        Category = ToolCategory.Encoding,
        Tags = new[] { "url", "encode", "decode", "percent", "uri" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "mode",
                DisplayName = "Mode",
                Description = "Operation mode: 'encode' or 'decode'",
                Type = "string",
                DefaultValue = "encode"
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
            : "encode";

        try
        {
            string result = mode switch
            {
                "encode" => Uri.EscapeDataString(input.Text),
                "decode" => Uri.UnescapeDataString(input.Text),
                _ => throw new ArgumentException($"Invalid mode '{mode}'. Use 'encode' or 'decode'.")
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
                ErrorMessage = $"URL {mode} error: {ex.Message}"
            };
        }
    }
}

using System.Text;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class Base64Tool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "encoding.base64",
        DisplayName = "Base64 Encoder/Decoder",
        Description = "Encode or decode text using Base64.",
        Category = ToolCategory.Encoding,
        Tags = new[] { "base64", "encode", "decode" },
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
        if (string.IsNullOrEmpty(input.Text))
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
            if (mode == "encode")
            {
                var bytes = Encoding.UTF8.GetBytes(input.Text);
                var encoded = Convert.ToBase64String(bytes);

                return new ToolResult
                {
                    Success = true,
                    OutputText = encoded
                };
            }
            else if (mode == "decode")
            {
                var bytes = Convert.FromBase64String(input.Text);
                var decoded = Encoding.UTF8.GetString(bytes);

                return new ToolResult
                {
                    Success = true,
                    OutputText = decoded
                };
            }
            else
            {
                return new ToolResult
                {
                    Success = false,
                    ErrorMessage = $"Invalid mode '{mode}'. Use 'encode' or 'decode'."
                };
            }
        }
        catch (FormatException ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"Invalid Base64 format: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"Error: {ex.Message}"
            };
        }
    }
}

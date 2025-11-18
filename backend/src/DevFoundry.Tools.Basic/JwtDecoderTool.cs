using System.Text;
using System.Text.Json;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class JwtDecoderTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "crypto.jwt",
        DisplayName = "JWT Decoder",
        Description = "Decode JWT tokens and display header and payload in human-readable format.",
        Category = ToolCategory.Crypto,
        Tags = new[] { "jwt", "token", "decode", "json", "web", "authentication" },
        Parameters = Array.Empty<ToolParameterDescriptor>()
    };

    public ToolResult Execute(ToolInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Text))
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = "No JWT token provided."
            };
        }

        var token = input.Text.Trim();

        try
        {
            var parts = token.Split('.');

            if (parts.Length != 3)
            {
                return new ToolResult
                {
                    Success = false,
                    ErrorMessage = "Invalid JWT format. Expected format: header.payload.signature"
                };
            }

            var header = DecodeBase64UrlPart(parts[0]);
            var payload = DecodeBase64UrlPart(parts[1]);

            var result = new StringBuilder();
            result.AppendLine("=== HEADER ===");
            result.AppendLine(FormatJson(header));
            result.AppendLine();
            result.AppendLine("=== PAYLOAD ===");
            result.AppendLine(FormatJson(payload));
            result.AppendLine();
            result.AppendLine("=== SIGNATURE ===");
            result.AppendLine(parts[2]);
            result.AppendLine();
            result.AppendLine("Note: This tool only decodes the JWT. It does NOT verify the signature.");

            return new ToolResult
            {
                Success = true,
                OutputText = result.ToString()
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"JWT decode error: {ex.Message}"
            };
        }
    }

    private static string DecodeBase64UrlPart(string base64Url)
    {
        // Convert base64url to base64
        var base64 = base64Url.Replace('-', '+').Replace('_', '/');

        // Add padding if necessary
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        var bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }

    private static string FormatJson(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(doc, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch
        {
            // If it's not valid JSON, return as-is
            return json;
        }
    }
}

using System.Security.Cryptography;
using System.Text;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class HashTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "crypto.hash",
        DisplayName = "Hash Calculator",
        Description = "Calculate MD5, SHA-1, SHA-256, or SHA-512 hashes.",
        Category = ToolCategory.Crypto,
        Tags = new[] { "hash", "md5", "sha1", "sha256", "sha512", "checksum" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "algorithm",
                DisplayName = "Algorithm",
                Description = "Hash algorithm: 'md5', 'sha1', 'sha256', or 'sha512'",
                Type = "string",
                DefaultValue = "sha256"
            },
            new ToolParameterDescriptor
            {
                Name = "uppercase",
                DisplayName = "Uppercase",
                Description = "Output hash in uppercase",
                Type = "bool",
                DefaultValue = false
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

        var algorithm = input.Parameters.TryGetValue("algorithm", out var a) && a is string algo
            ? algo.ToLowerInvariant()
            : "sha256";

        var uppercase = input.Parameters.TryGetValue("uppercase", out var u) && u is bool upper && upper;

        try
        {
            var bytes = Encoding.UTF8.GetBytes(input.Text);
            byte[] hash;

            switch (algorithm)
            {
                case "md5":
                    hash = MD5.HashData(bytes);
                    break;
                case "sha1":
                    hash = SHA1.HashData(bytes);
                    break;
                case "sha256":
                    hash = SHA256.HashData(bytes);
                    break;
                case "sha512":
                    hash = SHA512.HashData(bytes);
                    break;
                default:
                    return new ToolResult
                    {
                        Success = false,
                        ErrorMessage = $"Invalid algorithm '{algorithm}'. Use 'md5', 'sha1', 'sha256', or 'sha512'."
                    };
            }

            var hashString = Convert.ToHexString(hash);
            var result = uppercase ? hashString : hashString.ToLowerInvariant();

            return new ToolResult
            {
                Success = true,
                OutputText = result,
                Metadata = new Dictionary<string, object?>
                {
                    ["algorithm"] = algorithm.ToUpperInvariant(),
                    ["length"] = hash.Length * 8 // bits
                }
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"Error calculating hash: {ex.Message}"
            };
        }
    }
}

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class ColorConverterTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "color.converter",
        DisplayName = "Color Converter",
        Description = "Convert between color formats: HEX, RGB, RGBA, HSL, HSLA.",
        Category = ToolCategory.Other,
        Tags = new[] { "color", "hex", "rgb", "hsl", "rgba", "hsla", "convert" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "targetFormat",
                DisplayName = "Target Format",
                Description = "Target color format: 'hex', 'rgb', 'rgba', 'hsl', 'hsla'",
                Type = "string",
                DefaultValue = "rgb"
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
                ErrorMessage = "No color value provided."
            };
        }

        var targetFormat = input.Parameters.TryGetValue("targetFormat", out var tf) && tf is string tfStr
            ? tfStr.ToLowerInvariant()
            : "rgb";

        try
        {
            var color = ParseColor(input.Text.Trim());
            var result = ConvertColor(color, targetFormat);

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
                ErrorMessage = $"Color conversion error: {ex.Message}"
            };
        }
    }

    private static Color ParseColor(string colorStr)
    {
        colorStr = colorStr.Trim();

        // Try HEX format (#RRGGBB or #RRGGBBAA or #RGB)
        if (colorStr.StartsWith("#"))
        {
            return ParseHex(colorStr);
        }

        // Try RGB/RGBA format
        if (colorStr.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
        {
            return ParseRgb(colorStr);
        }

        // Try HSL/HSLA format
        if (colorStr.StartsWith("hsl", StringComparison.OrdinalIgnoreCase))
        {
            return ParseHsl(colorStr);
        }

        throw new ArgumentException($"Unrecognized color format: {colorStr}");
    }

    private static Color ParseHex(string hex)
    {
        hex = hex.TrimStart('#');

        if (hex.Length == 3)
        {
            // #RGB -> #RRGGBB
            hex = $"{hex[0]}{hex[0]}{hex[1]}{hex[1]}{hex[2]}{hex[2]}";
        }

        if (hex.Length == 6)
        {
            var r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            var g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            var b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return CreateColor(r, g, b, 1.0);
        }

        if (hex.Length == 8)
        {
            var r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            var g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            var b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            var a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;
            return CreateColor(r, g, b, a);
        }

        throw new ArgumentException("Invalid HEX color format. Use #RGB, #RRGGBB, or #RRGGBBAA");
    }

    private static Color ParseRgb(string rgb)
    {
        var match = Regex.Match(rgb, @"rgba?\s*\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*([\d.]+)\s*)?\)", RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid RGB/RGBA format. Use rgb(r, g, b) or rgba(r, g, b, a)");
        }

        var r = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var g = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var b = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
        var a = match.Groups[4].Success ? double.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture) : 1.0;

        return CreateColor(r, g, b, a);
    }

    private static Color ParseHsl(string hsl)
    {
        var match = Regex.Match(hsl, @"hsla?\s*\(\s*([\d.]+)\s*,\s*([\d.]+)%\s*,\s*([\d.]+)%\s*(?:,\s*([\d.]+)\s*)?\)", RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ArgumentException("Invalid HSL/HSLA format. Use hsl(h, s%, l%) or hsla(h, s%, l%, a)");
        }

        var h = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var s = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var l = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
        var a = match.Groups[4].Success ? double.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture) : 1.0;

        ValidateHslRange(h, s, l, a);

        return HslToRgb(h, s, l, a);
    }

    private static string ConvertColor(Color color, string targetFormat)
    {
        return targetFormat switch
        {
            "hex" => color.ToHex(),
            "rgb" => color.ToRgb(),
            "rgba" => color.ToRgba(),
            "hsl" => color.ToHsl(),
            "hsla" => color.ToHsla(),
            _ => throw new ArgumentException($"Unknown target format: {targetFormat}. Use 'hex', 'rgb', 'rgba', 'hsl', or 'hsla'")
        };
    }

    private static Color HslToRgb(double h, double s, double l, double a)
    {
        h = h / 360.0;
        s = s / 100.0;
        l = l / 100.0;

        double r, g, b;

        if (s == 0)
        {
            r = g = b = l;
        }
        else
        {
            var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            var p = 2 * l - q;
            r = HueToRgb(p, q, h + 1.0 / 3.0);
            g = HueToRgb(p, q, h);
            b = HueToRgb(p, q, h - 1.0 / 3.0);
        }

        return CreateColor(
            Math.Clamp((int)Math.Round(r * 255), 0, 255),
            Math.Clamp((int)Math.Round(g * 255), 0, 255),
            Math.Clamp((int)Math.Round(b * 255), 0, 255),
            a);
    }

    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0 / 6.0) return p + (q - p) * 6 * t;
        if (t < 1.0 / 2.0) return q;
        if (t < 2.0 / 3.0) return p + (q - p) * (2.0 / 3.0 - t) * 6;
        return p;
    }

    private static Color CreateColor(int r, int g, int b, double a)
    {
        if (r is < 0 or > 255 || g is < 0 or > 255 || b is < 0 or > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(r), "Color components must be between 0 and 255.");
        }

        if (a is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(a), "Alpha must be between 0 and 1.");
        }

        return new Color(r, g, b, a);
    }

    private static void ValidateHslRange(double h, double s, double l, double a)
    {
        if (h is < 0 or > 360)
        {
            throw new ArgumentOutOfRangeException(nameof(h), "Hue must be between 0 and 360.");
        }

        if (s is < 0 or > 100 || l is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(s), "Saturation and lightness must be between 0 and 100.");
        }

        if (a is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(a), "Alpha must be between 0 and 1.");
        }
    }

    private record Color(int R, int G, int B, double A)
    {
        public string ToHex()
        {
            if (A < 1.0)
            {
                var alpha = Math.Clamp((int)Math.Round(A * 255, MidpointRounding.AwayFromZero), 0, 255);
                return $"#{R:X2}{G:X2}{B:X2}{alpha:X2}";
            }
            return $"#{R:X2}{G:X2}{B:X2}";
        }

        public string ToRgb() => $"rgb({R}, {G}, {B})";

        public string ToRgba()
        {
            return $"rgba({R}, {G}, {B}, {A.ToString("0.##", CultureInfo.InvariantCulture)})";
        }

        public string ToHsl()
        {
            var (h, s, l) = RgbToHsl();
            return $"hsl({h:F0}, {s:F0}%, {l:F0}%)";
        }

        public string ToHsla()
        {
            var (h, s, l) = RgbToHsl();
            return $"hsla({h:F0}, {s:F0}%, {l:F0}%, {A.ToString("0.##", CultureInfo.InvariantCulture)})";
        }

        private (double h, double s, double l) RgbToHsl()
        {
            var r = R / 255.0;
            var g = G / 255.0;
            var b = B / 255.0;

            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var l = (max + min) / 2.0;

            if (max == min)
            {
                return (0, 0, l * 100);
            }

            var d = max - min;
            var s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

            double h;
            if (max == r)
            {
                h = (g - b) / d + (g < b ? 6 : 0);
            }
            else if (max == g)
            {
                h = (b - r) / d + 2;
            }
            else
            {
                h = (r - g) / d + 4;
            }

            h /= 6;

            return (h * 360, s * 100, l * 100);
        }
    }
}

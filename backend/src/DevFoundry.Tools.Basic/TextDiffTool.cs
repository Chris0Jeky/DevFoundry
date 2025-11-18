using System.Text;
using DevFoundry.Core;

namespace DevFoundry.Tools.Basic;

public sealed class TextDiffTool : ITool
{
    public ToolDescriptor Descriptor { get; } = new()
    {
        Id = "text.diff",
        DisplayName = "Text Diff",
        Description = "Compare two texts and show differences line by line.",
        Category = ToolCategory.Other,
        Tags = new[] { "text", "diff", "compare", "difference" },
        Parameters = new[]
        {
            new ToolParameterDescriptor
            {
                Name = "format",
                DisplayName = "Output Format",
                Description = "Output format: 'unified' or 'sidebyside'",
                Type = "string",
                DefaultValue = "unified"
            },
            new ToolParameterDescriptor
            {
                Name = "ignoreWhitespace",
                DisplayName = "Ignore Whitespace",
                Description = "Ignore whitespace differences",
                Type = "bool",
                DefaultValue = false
            },
            new ToolParameterDescriptor
            {
                Name = "ignoreCase",
                DisplayName = "Ignore Case",
                Description = "Ignore case differences",
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
                ErrorMessage = "No text provided for first input."
            };
        }

        if (string.IsNullOrWhiteSpace(input.SecondaryText))
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = "No text provided for second input."
            };
        }

        var format = input.Parameters.TryGetValue("format", out var f) && f is string fStr
            ? fStr.ToLowerInvariant()
            : "unified";

        var ignoreWhitespace = input.Parameters.TryGetValue("ignoreWhitespace", out var iw)
            && iw is bool iwb && iwb;

        var ignoreCase = input.Parameters.TryGetValue("ignoreCase", out var ic)
            && ic is bool icb && icb;

        try
        {
            var lines1 = SplitLines(input.Text);
            var lines2 = SplitLines(input.SecondaryText);

            var diff = ComputeDiff(lines1, lines2, ignoreWhitespace, ignoreCase);
            var output = format == "sidebyside"
                ? FormatSideBySide(diff)
                : FormatUnified(diff);

            return new ToolResult
            {
                Success = true,
                OutputText = output
            };
        }
        catch (Exception ex)
        {
            return new ToolResult
            {
                Success = false,
                ErrorMessage = $"Diff error: {ex.Message}"
            };
        }
    }

    private static string[] SplitLines(string text)
    {
        return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    private static List<DiffLine> ComputeDiff(string[] lines1, string[] lines2, bool ignoreWhitespace, bool ignoreCase)
    {
        var diff = new List<DiffLine>();
        var lcs = LongestCommonSubsequence(lines1, lines2, ignoreWhitespace, ignoreCase);

        int i = 0, j = 0, k = 0;

        while (i < lines1.Length || j < lines2.Length)
        {
            if (k < lcs.Count)
            {
                var (idx1, idx2) = lcs[k];

                // Add deletions
                while (i < idx1)
                {
                    diff.Add(new DiffLine(DiffType.Deleted, i + 1, lines1[i], null, null));
                    i++;
                }

                // Add additions
                while (j < idx2)
                {
                    diff.Add(new DiffLine(DiffType.Added, null, null, j + 1, lines2[j]));
                    j++;
                }

                // Add unchanged
                diff.Add(new DiffLine(DiffType.Unchanged, i + 1, lines1[i], j + 1, lines2[j]));
                i++;
                j++;
                k++;
            }
            else
            {
                // Handle remaining deletions
                while (i < lines1.Length)
                {
                    diff.Add(new DiffLine(DiffType.Deleted, i + 1, lines1[i], null, null));
                    i++;
                }

                // Handle remaining additions
                while (j < lines2.Length)
                {
                    diff.Add(new DiffLine(DiffType.Added, null, null, j + 1, lines2[j]));
                    j++;
                }
            }
        }

        return diff;
    }

    private static List<(int, int)> LongestCommonSubsequence(string[] lines1, string[] lines2, bool ignoreWhitespace, bool ignoreCase)
    {
        var m = lines1.Length;
        var n = lines2.Length;
        var dp = new int[m + 1, n + 1];

        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                if (AreEqual(lines1[i - 1], lines2[j - 1], ignoreWhitespace, ignoreCase))
                {
                    dp[i, j] = dp[i - 1, j - 1] + 1;
                }
                else
                {
                    dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                }
            }
        }

        // Backtrack to find the LCS
        var lcs = new List<(int, int)>();
        int x = m, y = n;

        while (x > 0 && y > 0)
        {
            if (AreEqual(lines1[x - 1], lines2[y - 1], ignoreWhitespace, ignoreCase))
            {
                lcs.Insert(0, (x - 1, y - 1));
                x--;
                y--;
            }
            else if (dp[x - 1, y] > dp[x, y - 1])
            {
                x--;
            }
            else
            {
                y--;
            }
        }

        return lcs;
    }

    private static bool AreEqual(string line1, string line2, bool ignoreWhitespace, bool ignoreCase)
    {
        var s1 = line1;
        var s2 = line2;

        if (ignoreWhitespace)
        {
            s1 = new string(s1.Where(c => !char.IsWhiteSpace(c)).ToArray());
            s2 = new string(s2.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return string.Equals(s1, s2, comparison);
    }

    private static string FormatUnified(List<DiffLine> diff)
    {
        var sb = new StringBuilder();
        sb.AppendLine("--- Original");
        sb.AppendLine("+++ Modified");
        sb.AppendLine();

        foreach (var line in diff)
        {
            switch (line.Type)
            {
                case DiffType.Unchanged:
                    sb.AppendLine($"  {line.LeftLine}");
                    break;
                case DiffType.Deleted:
                    sb.AppendLine($"- {line.LeftLine}");
                    break;
                case DiffType.Added:
                    sb.AppendLine($"+ {line.RightLine}");
                    break;
            }
        }

        return sb.ToString();
    }

    private static string FormatSideBySide(List<DiffLine> diff)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Original                                | Modified");
        sb.AppendLine("----------------------------------------|----------------------------------------");

        foreach (var line in diff)
        {
            var left = line.LeftLine ?? "";
            var right = line.RightLine ?? "";

            var leftPadded = left.Length > 40 ? left.Substring(0, 37) + "..." : left.PadRight(40);
            var indicator = line.Type switch
            {
                DiffType.Unchanged => " ",
                DiffType.Deleted => "<",
                DiffType.Added => ">",
                _ => " "
            };

            sb.AppendLine($"{leftPadded}|{indicator}{right}");
        }

        return sb.ToString();
    }

    private enum DiffType
    {
        Unchanged,
        Deleted,
        Added
    }

    private record DiffLine(
        DiffType Type,
        int? LeftLineNum,
        string? LeftLine,
        int? RightLineNum,
        string? RightLine
    );
}

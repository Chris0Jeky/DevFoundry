namespace DevFoundry.Runtime;

/// <summary>
/// Configuration for plugin loading
/// </summary>
public class PluginConfiguration
{
    private HashSet<string>? enabledToolIds;
    private HashSet<string> disabledToolIds = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Directory to search for plugin assemblies
    /// </summary>
    public string PluginsDirectory { get; set; } = "plugins";

    /// <summary>
    /// Whether to enable plugin discovery
    /// </summary>
    public bool EnablePluginDiscovery { get; set; } = true;

    /// <summary>
    /// List of enabled tool IDs (null = all tools enabled)
    /// </summary>
    public HashSet<string>? EnabledToolIds
    {
        get => enabledToolIds;
        set => enabledToolIds = value is null
            ? null
            : new HashSet<string>(value, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// List of disabled tool IDs
    /// </summary>
    public HashSet<string> DisabledToolIds
    {
        get => disabledToolIds;
        set => disabledToolIds = value is null
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(value, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines if a tool should be loaded based on configuration
    /// </summary>
    public bool IsToolEnabled(string toolId)
    {
        if (DisabledToolIds.Contains(toolId))
        {
            return false;
        }

        if (EnabledToolIds != null)
        {
            return EnabledToolIds.Contains(toolId);
        }

        return true;
    }
}

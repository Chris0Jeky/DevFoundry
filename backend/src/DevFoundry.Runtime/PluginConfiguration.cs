namespace DevFoundry.Runtime;

/// <summary>
/// Configuration for plugin loading
/// </summary>
public class PluginConfiguration
{
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
    public HashSet<string>? EnabledToolIds { get; set; }

    /// <summary>
    /// List of disabled tool IDs
    /// </summary>
    public HashSet<string> DisabledToolIds { get; set; } = new();

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

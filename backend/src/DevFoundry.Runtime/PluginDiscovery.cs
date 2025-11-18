using System.Reflection;
using DevFoundry.Core;

namespace DevFoundry.Runtime;

/// <summary>
/// Discovers and loads tools from external plugin assemblies
/// </summary>
public class PluginDiscovery
{
    private readonly string _pluginsDirectory;

    public PluginDiscovery(string? pluginsDirectory = null)
    {
        _pluginsDirectory = pluginsDirectory ?? Path.Combine(AppContext.BaseDirectory, "plugins");
    }

    /// <summary>
    /// Discovers all tools from assemblies in the plugins directory
    /// </summary>
    public IEnumerable<ITool> DiscoverTools()
    {
        var tools = new List<ITool>();

        if (!Directory.Exists(_pluginsDirectory))
        {
            return tools;
        }

        var assemblyFiles = Directory.GetFiles(_pluginsDirectory, "*.dll", SearchOption.AllDirectories);

        foreach (var assemblyPath in assemblyFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                var discoveredTools = DiscoverToolsFromAssembly(assembly);
                tools.AddRange(discoveredTools);
            }
            catch (Exception ex)
            {
                // Log or ignore assemblies that can't be loaded
                Console.Error.WriteLine($"Warning: Could not load plugin assembly '{assemblyPath}': {ex.Message}");
            }
        }

        return tools;
    }

    /// <summary>
    /// Discovers all ITool implementations from a specific assembly
    /// </summary>
    public IEnumerable<ITool> DiscoverToolsFromAssembly(Assembly assembly)
    {
        var tools = new List<ITool>();

        try
        {
            var toolTypes = assembly.GetTypes()
                .Where(t => typeof(ITool).IsAssignableFrom(t)
                         && !t.IsInterface
                         && !t.IsAbstract
                         && t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var toolType in toolTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(toolType) as ITool;
                    if (instance != null)
                    {
                        tools.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Warning: Could not instantiate tool '{toolType.FullName}': {ex.Message}");
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            Console.Error.WriteLine($"Warning: Could not load types from assembly '{assembly.FullName}': {ex.Message}");
        }

        return tools;
    }
}

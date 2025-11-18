using DevFoundry.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DevFoundry.Runtime;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDevFoundryRuntime(this IServiceCollection services)
    {
        // Register the tool registry
        services.AddSingleton<IToolRegistry, ToolRegistry>();

        return services;
    }

    public static IServiceCollection AddDevFoundryRuntime(
        this IServiceCollection services,
        PluginConfiguration configuration)
    {
        // Register configuration
        services.AddSingleton(configuration);

        // Register plugin discovery
        if (configuration.EnablePluginDiscovery)
        {
            services.AddSingleton(new PluginDiscovery(configuration.PluginsDirectory));
        }

        // Register the tool registry with configuration
        services.AddSingleton<IToolRegistry>(sp =>
        {
            var tools = sp.GetServices<ITool>().ToList();

            // Add plugins if enabled
            if (configuration.EnablePluginDiscovery)
            {
                var pluginDiscovery = sp.GetRequiredService<PluginDiscovery>();
                var pluginTools = pluginDiscovery.DiscoverTools();
                tools.AddRange(pluginTools);
            }

            return new ToolRegistry(tools, configuration);
        });

        return services;
    }

    public static IServiceCollection AddTool<TTool>(this IServiceCollection services)
        where TTool : class, ITool
    {
        services.AddSingleton<ITool, TTool>();
        return services;
    }

    public static IServiceCollection AddBasicTools(this IServiceCollection services)
    {
        // This will be used to register all tools from DevFoundry.Tools.Basic
        // The actual tools will be registered by the CLI and API projects
        return services;
    }
}

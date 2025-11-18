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

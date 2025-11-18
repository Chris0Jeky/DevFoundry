using System.CommandLine;
using DevFoundry.Runtime;

namespace DevFoundry.Cli.Commands;

public class ListCommand : Command
{
    public ListCommand(IToolRegistry toolRegistry) : base("list", "List all available tools")
    {
        var categoryOption = new Option<string?>(
            name: "--category",
            description: "Filter by category"
        );

        AddOption(categoryOption);

        this.SetHandler((string? category) =>
        {
            var tools = toolRegistry.GetAllTools();

            if (!string.IsNullOrWhiteSpace(category))
            {
                tools = tools.Where(t => t.Descriptor.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            Console.WriteLine("Available tools:\n");

            var groupedTools = tools.GroupBy(t => t.Descriptor.Category).OrderBy(g => g.Key);

            foreach (var group in groupedTools)
            {
                Console.WriteLine($"{group.Key}:");
                foreach (var tool in group.OrderBy(t => t.Descriptor.Id))
                {
                    Console.WriteLine($"  {tool.Descriptor.Id,-25} {tool.Descriptor.DisplayName}");
                    Console.WriteLine($"    {tool.Descriptor.Description}");
                }
                Console.WriteLine();
            }
        }, categoryOption);
    }
}

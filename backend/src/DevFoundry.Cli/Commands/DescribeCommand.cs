using System.CommandLine;
using DevFoundry.Runtime;

namespace DevFoundry.Cli.Commands;

public class DescribeCommand : Command
{
    public DescribeCommand(IToolRegistry toolRegistry) : base("describe", "Describe a tool in detail")
    {
        var toolIdArgument = new Argument<string>(
            name: "toolId",
            description: "The ID of the tool to describe"
        );

        AddArgument(toolIdArgument);

        this.SetHandler((string toolId) =>
        {
            var tool = toolRegistry.GetTool(toolId);

            if (tool == null)
            {
                Console.Error.WriteLine($"Error: Tool '{toolId}' not found.");
                Console.Error.WriteLine("Use 'devfoundry list' to see available tools.");
                Environment.Exit(2);
                return;
            }

            var descriptor = tool.Descriptor;

            Console.WriteLine($"Tool: {descriptor.DisplayName}");
            Console.WriteLine($"ID: {descriptor.Id}");
            Console.WriteLine($"Category: {descriptor.Category}");
            Console.WriteLine($"Description: {descriptor.Description}");

            if (descriptor.Tags.Any())
            {
                Console.WriteLine($"Tags: {string.Join(", ", descriptor.Tags)}");
            }

            if (descriptor.Parameters.Any())
            {
                Console.WriteLine("\nParameters:");
                foreach (var param in descriptor.Parameters)
                {
                    Console.WriteLine($"  --{param.Name} ({param.Type})");
                    Console.WriteLine($"    {param.Description}");
                    Console.WriteLine($"    Default: {param.DefaultValue ?? "none"}");
                }
            }
        }, toolIdArgument);
    }
}

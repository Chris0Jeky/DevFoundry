using System.CommandLine;
using DevFoundry.Runtime;
using Spectre.Console;

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

            AnsiConsole.MarkupLine("[bold cyan]Available DevFoundry Tools[/]\n");

            var groupedTools = tools.GroupBy(t => t.Descriptor.Category).OrderBy(g => g.Key);

            foreach (var group in groupedTools)
            {
                var table = new Table()
                    .BorderColor(Color.Grey)
                    .Border(TableBorder.Rounded)
                    .Title($"[yellow]{group.Key}[/]")
                    .AddColumn(new TableColumn("[bold]Tool ID[/]").Width(30))
                    .AddColumn(new TableColumn("[bold]Name[/]").Width(35))
                    .AddColumn(new TableColumn("[bold]Description[/]"));

                foreach (var tool in group.OrderBy(t => t.Descriptor.Id))
                {
                    table.AddRow(
                        $"[cyan]{tool.Descriptor.Id}[/]",
                        $"[white]{tool.Descriptor.DisplayName}[/]",
                        $"[grey]{tool.Descriptor.Description}[/]"
                    );
                }

                AnsiConsole.Write(table);
                AnsiConsole.WriteLine();
            }

            var totalCount = tools.Count();
            AnsiConsole.MarkupLine($"[dim]Total: {totalCount} tool(s)[/]");
        }, categoryOption);
    }
}

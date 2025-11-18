using System.CommandLine;
using DevFoundry.Runtime;
using Spectre.Console;

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
                AnsiConsole.MarkupLine($"[red]Error:[/] Tool '[yellow]{toolId}[/]' not found.");
                AnsiConsole.MarkupLine("[dim]Use 'devfoundry list' to see available tools.[/]");
                Environment.Exit(2);
                return;
            }

            var descriptor = tool.Descriptor;

            // Main info panel
            var panel = new Panel(
                new Markup($"[bold]{descriptor.DisplayName}[/]\n\n" +
                          $"[cyan]ID:[/] {descriptor.Id}\n" +
                          $"[cyan]Category:[/] {descriptor.Category}\n" +
                          $"[cyan]Description:[/] {descriptor.Description}\n\n" +
                          (descriptor.Tags.Any()
                            ? $"[cyan]Tags:[/] {string.Join(", ", descriptor.Tags.Select(t => $"[yellow]{t}[/]"))}"
                            : "")))
            {
                Header = new PanelHeader($"[bold green]Tool Details[/]"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Green)
            };

            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();

            if (descriptor.Parameters.Any())
            {
                var paramsTable = new Table()
                    .BorderColor(Color.Grey)
                    .Border(TableBorder.Rounded)
                    .Title("[yellow]Parameters[/]")
                    .AddColumn(new TableColumn("[bold]Name[/]").Width(20))
                    .AddColumn(new TableColumn("[bold]Type[/]").Width(15))
                    .AddColumn(new TableColumn("[bold]Description[/]").Width(40))
                    .AddColumn(new TableColumn("[bold]Default[/]").Width(15));

                foreach (var param in descriptor.Parameters)
                {
                    paramsTable.AddRow(
                        $"[cyan]--{param.Name}[/]",
                        $"[yellow]{param.Type}[/]",
                        $"[grey]{param.Description}[/]",
                        $"[dim]{param.DefaultValue?.ToString() ?? "none"}[/]"
                    );
                }

                AnsiConsole.Write(paramsTable);
                AnsiConsole.WriteLine();
            }

            // Usage example
            var exampleMarkup = new Markup(
                $"[dim]Example usage:[/]\n" +
                $"  devfoundry run [cyan]{descriptor.Id}[/] --text \"your input\""
            );
            AnsiConsole.Write(exampleMarkup);
            AnsiConsole.WriteLine();
        }, toolIdArgument);
    }
}

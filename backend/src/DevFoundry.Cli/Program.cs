using System.CommandLine;
using DevFoundry.Cli.Commands;
using DevFoundry.Core;
using DevFoundry.Runtime;
using DevFoundry.Tools.Basic;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Register tools
services.AddTool<JsonFormatterTool>();
services.AddTool<JsonYamlConverterTool>();
services.AddTool<Base64Tool>();
services.AddTool<UuidTool>();
services.AddTool<HashTool>();
services.AddTool<TimestampConverterTool>();
services.AddTool<StringCaseConverterTool>();
services.AddTool<UrlEncoderTool>();
services.AddTool<JwtDecoderTool>();

// Register runtime
services.AddDevFoundryRuntime();

var serviceProvider = services.BuildServiceProvider();
var toolRegistry = serviceProvider.GetRequiredService<IToolRegistry>();

// Create root command
var rootCommand = new RootCommand("DevFoundry - Offline developer toolkit");

// Add subcommands
rootCommand.AddCommand(new ListCommand(toolRegistry));
rootCommand.AddCommand(new DescribeCommand(toolRegistry));
rootCommand.AddCommand(new RunCommand(toolRegistry));

return await rootCommand.InvokeAsync(args);

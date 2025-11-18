using System.CommandLine;
using System.Text;
using DevFoundry.Core;
using DevFoundry.Runtime;

namespace DevFoundry.Cli.Commands;

public class RunCommand : Command
{
    public RunCommand(IToolRegistry toolRegistry) : base("run", "Run a tool")
    {
        var toolIdArgument = new Argument<string>(
            name: "toolId",
            description: "The ID of the tool to run"
        );

        var textOption = new Option<string?>(
            name: "--text",
            description: "Input text for the tool"
        );

        var fileOption = new Option<FileInfo?>(
            name: "--file",
            description: "Input file for the tool"
        );

        var paramOption = new Option<string[]>(
            name: "--param",
            description: "Parameters in the format key=value"
        )
        { AllowMultipleArgumentsPerToken = true };

        AddArgument(toolIdArgument);
        AddOption(textOption);
        AddOption(fileOption);
        AddOption(paramOption);

        this.SetHandler(async (string toolId, string? text, FileInfo? file, string[] parameters) =>
        {
            var tool = toolRegistry.GetTool(toolId);

            if (tool == null)
            {
                Console.Error.WriteLine($"Error: Tool '{toolId}' not found.");
                Console.Error.WriteLine("Use 'devfoundry list' to see available tools.");
                Environment.Exit(2);
                return;
            }

            // Determine input text
            string? inputText = text;

            if (file != null)
            {
                if (!file.Exists)
                {
                    Console.Error.WriteLine($"Error: File '{file.FullName}' not found.");
                    Environment.Exit(3);
                    return;
                }

                inputText = await File.ReadAllTextAsync(file.FullName);
            }
            else if (inputText == null && Console.IsInputRedirected)
            {
                // Read from stdin
                using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
                inputText = await reader.ReadToEndAsync();
            }

            // Parse parameters
            var paramDict = new Dictionary<string, object?>();
            foreach (var param in parameters)
            {
                var parts = param.Split('=', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    // Try to infer the type
                    if (int.TryParse(value, out var intValue))
                    {
                        paramDict[key] = intValue;
                    }
                    else if (bool.TryParse(value, out var boolValue))
                    {
                        paramDict[key] = boolValue;
                    }
                    else
                    {
                        paramDict[key] = value;
                    }
                }
            }

            // Execute the tool
            var input = new ToolInput
            {
                Text = inputText,
                Parameters = paramDict
            };

            var result = tool.Execute(input);

            if (result.Success)
            {
                Console.WriteLine(result.OutputText);

                if (!string.IsNullOrEmpty(result.SecondaryOutputText))
                {
                    Console.WriteLine("\n--- Secondary Output ---");
                    Console.WriteLine(result.SecondaryOutputText);
                }

                Environment.Exit(0);
            }
            else
            {
                Console.Error.WriteLine($"Error: {result.ErrorMessage}");
                Environment.Exit(1);
            }
        }, toolIdArgument, textOption, fileOption, paramOption);
    }
}

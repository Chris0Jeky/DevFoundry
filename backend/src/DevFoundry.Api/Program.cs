using DevFoundry.Api.DTOs;
using DevFoundry.Core;
using DevFoundry.Runtime;
using DevFoundry.Tools.Basic;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register tools
builder.Services.AddTool<JsonFormatterTool>();
builder.Services.AddTool<JsonYamlConverterTool>();
builder.Services.AddTool<Base64Tool>();
builder.Services.AddTool<UuidTool>();
builder.Services.AddTool<HashTool>();
builder.Services.AddTool<TimestampConverterTool>();
builder.Services.AddTool<StringCaseConverterTool>();
builder.Services.AddTool<UrlEncoderTool>();
builder.Services.AddTool<JwtDecoderTool>();

// Register runtime
builder.Services.AddDevFoundryRuntime();

var app = builder.Build();

app.UseCors();

// API endpoints
var api = app.MapGroup("/api");

// GET /api/tools - List all tools
api.MapGet("/tools", (IToolRegistry toolRegistry) =>
{
    var tools = toolRegistry.GetAllTools();

    var dtos = tools.Select(t => new ToolDescriptorDto
    {
        Id = t.Descriptor.Id,
        DisplayName = t.Descriptor.DisplayName,
        Description = t.Descriptor.Description,
        Category = t.Descriptor.Category.ToString(),
        Tags = t.Descriptor.Tags,
        Parameters = t.Descriptor.Parameters.Select(p => new ToolParameterDescriptorDto
        {
            Name = p.Name,
            DisplayName = p.DisplayName,
            Description = p.Description,
            Type = p.Type,
            DefaultValue = p.DefaultValue
        }).ToList()
    }).ToList();

    return Results.Ok(dtos);
});

// POST /api/tools/{toolId}/run - Run a tool
api.MapPost("/tools/{toolId}/run", (string toolId, ToolRunRequest request, IToolRegistry toolRegistry) =>
{
    var tool = toolRegistry.GetTool(toolId);

    if (tool == null)
    {
        return Results.NotFound(new ToolRunResult
        {
            Success = false,
            ErrorMessage = $"Tool '{toolId}' not found."
        });
    }

    var input = new ToolInput
    {
        Text = request.Text,
        SecondaryText = request.SecondaryText,
        Parameters = request.Parameters
    };

    var result = tool.Execute(input);

    var dto = new ToolRunResult
    {
        Success = result.Success,
        OutputText = result.OutputText,
        SecondaryOutputText = result.SecondaryOutputText,
        ErrorMessage = result.ErrorMessage,
        Metadata = new Dictionary<string, object?>(result.Metadata)
    };

    return result.Success ? Results.Ok(dto) : Results.BadRequest(dto);
});

app.Run();

namespace DevFoundry.Core;

public interface ITool
{
    ToolDescriptor Descriptor { get; }
    ToolResult Execute(ToolInput input);
}

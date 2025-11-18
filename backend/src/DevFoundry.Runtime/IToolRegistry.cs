using DevFoundry.Core;

namespace DevFoundry.Runtime;

public interface IToolRegistry
{
    IEnumerable<ITool> GetAllTools();
    ITool? GetTool(string id);
}

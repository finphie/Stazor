using System.Threading.Tasks;

namespace Stazor.Engine
{
    public interface IEngine
    {
        string Name { get; }

        string Description { get; }

        ValueTask ExecuteAsync();
    }
}
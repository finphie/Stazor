using System.Threading.Tasks;
using Stazor.Engine;

namespace Stazor.Core
{
    public interface ITheme
    {
        IEngine Engine { get; }

        Pipeline Pipeline { get; }

        ValueTask ExecuteAsync();
    }
}
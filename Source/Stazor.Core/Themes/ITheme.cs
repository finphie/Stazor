using System.Threading.Tasks;
using Stazor.Core.Pipelines;

namespace Stazor.Core.Themes
{
    public interface ITheme
    {
        Pipeline Pipeline { get; }

        ValueTask ExecuteAsync();
    }
}
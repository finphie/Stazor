using System.Threading.Tasks;

namespace Stazor.Core
{
    public interface ITheme
    {
        Pipeline Pipeline { get; }

        ValueTask ExecuteAsync();
    }
}
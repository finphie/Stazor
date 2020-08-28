using System.Threading.Tasks;

namespace Stazor.Core.Themes
{
    public interface ITheme
    {
        Pipeline Pipeline { get; }

        ValueTask ExecuteAsync();
    }
}
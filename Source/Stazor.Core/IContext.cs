using Utf8Utility;

namespace Stazor.Core
{
    /// <summary>
    /// コンテキスト
    /// </summary>
    public interface IContext : SimpleTextTemplate.Abstractions.IContext
    {
        void Add(Utf8String key, Utf8String value);
    }
}
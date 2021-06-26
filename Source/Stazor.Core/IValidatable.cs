namespace Stazor.Core
{
    public interface IValidatable
    {
        /// <summary>
        /// 入力値を検証します。
        /// </summary>
        void Validate();
    }
}
namespace Stazor.Core
{
    public interface IStazorLogger
    {
        void Info(string message);
    }

    public interface IStazorLogger<out TCategoryName> : IStazorLogger
    {
    }
}
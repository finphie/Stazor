namespace Stazor.Core
{
    public interface IStazorLogger
    {
        void Trace(string message);

        void Debug(string message);

        void Information(string message);

        void Warning(string message);

        void Error(string message);

        void Critical(string message);
    }

    public interface IStazorLogger<out TCategoryName> : IStazorLogger
    {
    }
}
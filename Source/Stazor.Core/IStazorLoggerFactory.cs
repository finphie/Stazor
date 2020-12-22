namespace Stazor.Core
{
    public interface IStazorLoggerFactory
    {
        IStazorLogger CreateLogger<TCategoryName>();
    }
}
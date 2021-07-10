namespace Stazor.Plugins
{
    public interface IPipeline
    {
        IStazorDocument[] Execute(string[] filePaths);
    }
}
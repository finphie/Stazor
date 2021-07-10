namespace Stazor.Themes
{
    public interface IPipeline
    {
        IStazorDocument[] Execute(string[] filePaths);
    }
}
namespace Stazor.Core
{
    public interface IDocumentList
    {
        int Length { get; }

        IStazorDocument this[int index] { get; set; }

        void Add(IStazorDocument document);

        IStazorDocument[] ToArray();
    }
}
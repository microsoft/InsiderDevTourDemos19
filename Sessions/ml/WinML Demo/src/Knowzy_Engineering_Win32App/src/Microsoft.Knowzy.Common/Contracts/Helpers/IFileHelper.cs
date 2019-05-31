namespace Microsoft.Knowzy.Common.Contracts.Helpers
{
    public interface IFileHelper
    {
        string ActualPath { get; }
        string ReadTextFile(string filePath);
        void WriteTextFile(string filePath, string content);
    }
}
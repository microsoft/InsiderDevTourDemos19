namespace Microsoft.Knowzy.Common.Contracts.Helpers
{
    public interface IJsonHelper
    {
        T Deserialize<T>(string serializedObject);
        string Serialize<T>(T objectToSerialize);
    }
}
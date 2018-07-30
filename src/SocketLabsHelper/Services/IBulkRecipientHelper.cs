using InjectionApi.Sdk.Email;

namespace SocketLabsHelper.Services
{
    public interface IBulkRecipientHelper
    {
        MergeData GetMergeData();
        void AddGlobalMergeField(string name, string value);
        bool AddCustomFieldToRecipient(string emailAddress, string name, string value);
        void AddRecipient(string emailAddress, string friendlyName = null);
    }
}

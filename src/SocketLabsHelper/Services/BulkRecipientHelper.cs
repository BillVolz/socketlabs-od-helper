using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InjectionApi.Sdk.Email;

namespace SocketLabsHelper.Services
{
    public class BulkRecipientHelper: IBulkRecipientHelper
    {
        private List<List<MergeRow>> RecipientRow { get; } = new List<List<MergeRow>>();
        private List<MergeRow> GlobalMessageMergeData { get; } = new List<MergeRow>();

        public int RecipientCount => RecipientRow.Count;

        public void ClearRecipients()
        {
            RecipientRow.Clear();
        }
        /// <summary>
        /// Returns SocketLabs SDK save MergeData object.
        /// </summary>
        /// <returns></returns>
        public MergeData GetMergeData()
        {
            //Needed to convert the inner list in to an array.
            var tempPerMessage = new List<MergeRow[]>();
            foreach (var row in RecipientRow)
            {
                tempPerMessage.Add(row.ToArray());
            }

            var mergeData = new MergeData
            {
                Global = GlobalMessageMergeData.ToArray(),
                PerMessage = tempPerMessage.ToArray()
            };
            return mergeData;
        }

        public void AddGlobalMergeField(string name, string value)
        {
            GlobalMessageMergeData.Add(new MergeRow(){Field=name,Value=value});
        }

        /// <summary>
        /// Adds a custom field for an address.
        /// </summary>
        /// <param name="emailAddress">Email address to add the custom field for.  (must already exists in list)</param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddCustomFieldToRecipient(string emailAddress, string name, string value)
        {
            foreach (var row in RecipientRow)
            {
                var found = row.FirstOrDefault(y => y.Field == "DeliveryAddress" && y.Value == emailAddress);
                if (found == null) continue;

                row.Add(new MergeRow(){Field=name,Value=value});
                return true;

            }
            return false;

        }
        public void AddRecipient(string emailAddress, string friendlyName=null)
        {
            var variables = new List<MergeRow>
            {
                new MergeRow()
                {
                    Field = "DeliveryAddress",
                    Value = emailAddress
                }
            };
            //If using friendly name add it.
            if (!string.IsNullOrEmpty(friendlyName))
            {
                variables.Add(new MergeRow()
                {
                    Field = "FriendlyName",
                    Value = friendlyName
                });
            }

            RecipientRow.Add(variables);
        }
    }
}

using System;
using System.Collections.Generic;
using InjectionApi.Sdk.Client;
using InjectionApi.Sdk.Email;
using SocketLabsHelper.Services;

namespace SocketLabsHelper
{
    public class Mailer
    {
        private static string _apiKey;
        private static int _serverId;

        public Mailer(string apiKey, int serverId)
        {
            _apiKey = apiKey;
            _serverId = serverId;
        }
        public void SendMergeMessageWithUnlimitedRecipientsUsingHelper(System.Net.Mail.MailAddress[] addresses)
        {
            int totalAllowedRecipientsPerMessage = 50;

        
            // Construct the object used to generate JSON for the POST request.
            // The you can add any merge field you like if you are using API templates.
            var postBody = new PostBody
            {
                ServerId = _serverId,
                ApiKey = _apiKey,
                Messages = new[]
                {
                    new EmailMessage
                    {
                        Subject = "%%Subject%%",
                        To = new[]
                        {
                            new Address
                            {
                                EmailAddress = "%%DeliveryAddress%%",
                                FriendlyName = "%%FriendlyName%%"
                            }
                        },
                        From = new Address
                        {
                            EmailAddress = "%%FromEmail%%",
                            FriendlyName = "%%FromName%%"
                        },
                        TextBody = "%%TextBody%%",
                        HtmlBody = "%%HtmlBody%%",
                    }
                }
            };

            var bulkRecipientData = new BulkRecipientHelper();
            bulkRecipientData.AddGlobalMergeField("Subject", "Email subject line for SDK example.");
            bulkRecipientData.AddGlobalMergeField("TextBody",
                "The text portion of the message. Using Merge %%CustomField%%.");
            bulkRecipientData.AddGlobalMergeField("HtmlBody",
                "<h1>The HTML portion of the message</h1><br/><p>A paragraph using Merge %%CustomField%%..</p>");
            bulkRecipientData.AddGlobalMergeField("FromName", "Example Name");
            bulkRecipientData.AddGlobalMergeField("FromEmail", "example@example.com");

            int reciepientsForThisMessage = 0;
            foreach (var recipient in addresses)
            {
                bulkRecipientData.AddRecipient(recipient.Address, recipient.DisplayName);
                reciepientsForThisMessage++;

                if (reciepientsForThisMessage == totalAllowedRecipientsPerMessage)
                {
                    postBody.Messages[0].MergeData = bulkRecipientData.GetMergeData();
                    SendMEssage(postBody);
                    reciepientsForThisMessage = 0;
                    bulkRecipientData.ClearRecipients();
                }      
            }
            //Recipients that didn't send yet?
            if (bulkRecipientData.RecipientCount>0)
            {
                postBody.Messages[0].MergeData = bulkRecipientData.GetMergeData();
                SendMEssage(postBody);
            }
        }

        private void SendMEssage(PostBody postBody)
        {
            try
            {
                var httpPostServer = new HttpPostService();
                var response = httpPostServer.PostAndGetResponse<PostResponse>(postBody,
                    "https://inject.socketlabs.com/api/v1/email", null);
                // Display the results.
                if (response.ErrorCode.Equals("Success"))
                {
                    Console.WriteLine("Successful injection!");
                }
                else if (response.ErrorCode.Equals("Warning"))
                {
                    Console.WriteLine("Warning, not all recipients/messages were sent.");
                    foreach (var result in response.MessageResults)
                    {
                        Console.WriteLine($"Message #{result.Index} {result.ErrorCode}");
                        foreach (var address in result.AddressResults)
                        {
                            Console.WriteLine(
                                $"{address.EmailAddress} {address.ErrorCode} Sent: {address.Accepted}");
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Failed injection!");
                    Console.WriteLine(response.ErrorCode);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, something bad happened: " + ex.Message);
            }
        }

        public void SendMergeMessageWithMultipleRecipientsUsingHelper()
        {

            // Construct the object used to generate JSON for the POST request.
            // The you can add any merge field you like if you are using API templates.
            var postBody = new PostBody
            {
                ServerId = _serverId,
                ApiKey = _apiKey,
                Messages = new[]
                {
                    new EmailMessage
                    {
                        Subject = "%%Subject%%",
                        To = new[]
                        {
                            new Address
                            {
                                EmailAddress = "%%DeliveryAddress%%",
                                FriendlyName = "%%FriendlyName%%"
                            }
                        },
                        From = new Address
                        {
                            EmailAddress = "%%FromEmail%%",
                            FriendlyName = "%%FromName%%"
                        },
                        TextBody = "%%TextBody%%",
                        HtmlBody = "%%HtmlBody%%",
                    }
                }
            };

            var bulkRecipientData = new BulkRecipientHelper();
            bulkRecipientData.AddGlobalMergeField("Subject", "Email subject line for SDK example.");
            bulkRecipientData.AddGlobalMergeField("TextBody", "The text portion of the message. Using Merge %%CustomField%%.");
            bulkRecipientData.AddGlobalMergeField("HtmlBody", "<h1>The HTML portion of the message</h1><br/><p>A paragraph using Merge %%CustomField%%..</p>");
            bulkRecipientData.AddGlobalMergeField("FromName", "Example Name");
            bulkRecipientData.AddGlobalMergeField("FromEmail", "example@example.com");

            bulkRecipientData.AddRecipient("recipient1@example.com","recipient 1");
            bulkRecipientData.AddCustomFieldToRecipient("recipient1@example.com", "CustomField", "Example 1");

            bulkRecipientData.AddRecipient("recipient2@example.com", "recipient 2");
            bulkRecipientData.AddCustomFieldToRecipient("recipient2@example.com", "CustomField", "Example 2");

            bulkRecipientData.AddRecipient("recipient3@example.com", "recipient 3");
            bulkRecipientData.AddCustomFieldToRecipient("recipient3@example.com", "CustomField", "Example 3");

            postBody.Messages[0].MergeData = bulkRecipientData.GetMergeData();

            try
            {
                var httpPostServer = new HttpPostService();
                var response = httpPostServer.PostAndGetResponse<PostResponse>(postBody, "https://inject.socketlabs.com/api/v1/email",null);
                // Display the results.
                if (response.ErrorCode.Equals("Success"))
                {
                    Console.WriteLine("Successful injection!");
                }
                else if (response.ErrorCode.Equals("Warning"))
                {
                    Console.WriteLine("Warning, not all recipients/messages were sent.");
                    foreach (var result in response.MessageResults)
                    {
                        Console.WriteLine($"Message #{result.Index} {result.ErrorCode}");
                        foreach (var address in result.AddressResults)
                        {
                            Console.WriteLine($"{address.EmailAddress} {address.ErrorCode} Sent: {address.Accepted}");
                        }
                    
                    }
                }
                else
                {
                    Console.WriteLine("Failed injection!");
                    Console.WriteLine(response.ErrorCode);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, something bad happened: " + ex.Message);
            }
        }
        public void SendWithInlineRecipientAdding()
        {
            // Construct the object used to generate JSON for the POST request.
            var postBody = new PostBody
            {
                ServerId = _serverId,
                ApiKey = _apiKey,
                Messages = new[]
                {
                    new EmailMessage
                    {
                        Subject = "Email subject line for SDK example.",
                        To = new[]
                        {
                            new Address
                            {
                                EmailAddress = "%%DeliveryAddress%%",
                                FriendlyName = "%%DeliveryName%%"
                            }
                        },
                        From = new Address
                        {
                            EmailAddress = "from@example.com",
                            FriendlyName = "From Address"
                        },
                        TextBody = "The text portion of the message.",
                        HtmlBody = "<h1>The HTML portion of the message</h1><br/><p>A paragraph.</p>",
                    }
                }
            };

            var mergeRows = new List<MergeRow[]>();

            var contact1 = new List<MergeRow>();
            contact1.Add(new MergeRow() { Field = "DeliveryAddress", Value = "recipient1@example.com" });
            contact1.Add(new MergeRow() { Field = "DeliveryName", Value = "recipient1" });
            mergeRows.Add(contact1.ToArray());
            var contact2 = new List<MergeRow>();
            contact2.Add(new MergeRow() { Field = "DeliveryAddress", Value = "recipient1@example.com" });
            contact2.Add(new MergeRow() { Field = "DeliveryName", Value = "recipient1" });
            mergeRows.Add(contact2.ToArray());

            postBody.Messages[0].MergeData = new MergeData();
            postBody.Messages[0].MergeData.PerMessage = mergeRows.ToArray();

            try
            {
                var httpPostServer = new HttpPostService();
                var response = httpPostServer.PostAndGetResponse<PostResponse>(postBody, "https://inject.socketlabs.com/api/v1/email", null);



                if (response.ErrorCode.Equals("Success"))
                {
                    Console.WriteLine("Successful injection!");
                }
                else
                {
                    Console.WriteLine("Failed injection!");
                    Console.WriteLine(response.ErrorCode);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, something bad happened: " + ex.Message);
            }
        }
    }
}

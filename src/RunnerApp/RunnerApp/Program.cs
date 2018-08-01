using System;
using System.Collections.Generic;
using System.Net.Mail;
using SocketLabsHelper;

namespace RunnerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set these before running.
            var serverid = 0000;
            var marketinApiKey = "";
            var injectionApiKey = "";
            

            var marketing = new Marketing(marketinApiKey, serverid);
            var content = marketing.GetContent();
            foreach (var tent in content)
            {
                Console.WriteLine(tent.ToString());
            }

            var recipients = new List<System.Net.Mail.MailAddress>();
            //Keep adding as many recipients as you need.
            recipients.Add(new MailAddress("exmaple@example.com","My Example"));

            var mailer = new Mailer(injectionApiKey, serverid);
            mailer.SendMergeMessageWithUnlimitedRecipientsUsingHelper(recipients.ToArray());


            //mailer.SendMergeMessageWithMultipleRecipientsUsingHelper();

            Console.WriteLine("Press Enter to Continue.");
            Console.ReadLine();
        }
    }
}

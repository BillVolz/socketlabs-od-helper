using System;
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

            var mailer = new Mailer(injectionApiKey, serverid);
            mailer.SendMergeMessageWithMultipleRecipientsUsingHelper();

            Console.WriteLine("Press Enter to Continue.");
            Console.ReadLine();
        }
    }
}

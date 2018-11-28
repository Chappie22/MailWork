using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Limilabs.Client.IMAP;

namespace MailApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MailTask.Mail mail = new MailTask.Mail();
            Console.WriteLine( mail.Login("bsssbhope@gmail.com", "road24@1997"));
           
            
           
           // mail.SearchByUser("sususik52@gmail.com");
            
            
           // mail.NewThreadDownloadAllMailsToDirectory("/Users/kirill/Desktop/MailTask/MailTask/AllEmails", false);

            int counter = 0;
           for (int i = 0; i < 50; i++)
           {
               try
               {
                   mail.WriteMessage("I love you", "Hello", AutoGenerate());
                   Console.WriteLine("Sended" + counter++);
               }
               catch (Exception e)
               {
                   Console.WriteLine(e);
               }              
           }
            
         
            
            
          //var flag = Flag.All;
          //mail.AllMailsFlag(flag);

        }

        private static string AutoGenerate()
        {

            List<string> words = new List<string>()
            {
                "best", ".", "mail", "andrey", "artem", "love",
                "my", "kirill", "wow", "only", "hello", "super"
            };

            string mail = String.Empty;
            
            for (int i = 0; i < 3; i++)
            {
                
                mail += words[new Random().Next(0, words.Count-1)];
            }


            Console.WriteLine(mail + "@gmail.com");
            return mail + "@gmail.com";
        }
    }
    
    
}
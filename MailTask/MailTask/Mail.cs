using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Limilabs.Client.IMAP;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Limilabs.Mail.MIME;
using Limilabs.Mail.Tools.MBox;

namespace MailTask
{
    public partial class Mail
    {
        private string _login;
        private string _psw;
        private Imap _imap;

        public bool Login(string login, string pass)
        {
            {
                _imap = new Imap();
                _imap.ConnectSSL("imap.gmail.com", 993);

                try
                {
                    _imap.UseBestLogin(login, pass);
                }
                catch (Exception e)
                {
                    return false;
                }

                if (!_imap.Connected) return false;
                _login = login;
                _psw = pass;
                return true;
            }
        }

        public void WriteMessage(string subject,string text, string to)
        {
           // _imap.SelectInbox();
           
            MailBuilder builder = new MailBuilder();
            
            builder.Subject = subject;
            builder.Text = text;
            
            builder.From.Add(new MailBox(_login));
            builder.To.Add(new MailBox(to));

            IMail email = builder.Create();
            
            using(Smtp smtp = new Smtp())
            {
                smtp.Connect("smtp.gmail.com");       // or ConnectSSL for SSL
                smtp.UseBestLogin(_login, _psw);

                smtp.SendMessage(email);                      

                smtp.Close();    
            }  


        }

        public void SearchByUser(string user)
        {
            _imap.SelectInbox();
            List<long> uids = _imap.Search( Expression.From(user));

            foreach (var uid in uids)
            {
                MessageShow(uid);
            }
        }

        private void DownloadAllMailsToDirectory()
        {
            _imap.SelectInbox();

            List<long> uids = _imap.Search(Expression.All());               
            foreach (long uid in uids)
            {
                var eml = _imap.GetMessageByUID(uid);
                string fileName = string.Format(@"C:\Users\Админ\MailApplication\MailApplication\Emails\{0}.eml", uid);
                File.WriteAllBytes(fileName, eml);
            }

        }
             
        public async void AsyncDownloadAllMailsToDirectory(string directory)
        {
            await Task.Run(delegate
            {
                _imap.SelectInbox();
                List<long> uids = _imap.Search(Expression.All());

                
                int a = 0;
                foreach (long uid in uids)
                {
                    
                    Console.Clear();
                    Console.WriteLine("Together files: " + uids.Count);
                    float x = (float) ((double)100 / uids.Count * a);
                    Console.WriteLine(x  + "% Loading... Files copied: " + a);
                    
                    
                    var eml = _imap.GetMessageByUID(uid);
                    string fileName = string.Format(@"/Users/kirill/Desktop/MailTask/MailTask/AllEmails/email_{0}.eml", uid);
                    File.WriteAllBytes(fileName, eml);
                    a++;
                }

                Console.WriteLine("Finished");
                
                
            });
        }

        public void NewThreadDownloadAllMailsToDirectory(string directory, bool background)
        {
            ParameterizedThreadStart parameterizedThreadStart = delegate(object o) { _imap.SelectInbox();
                List<long> uids = _imap.Search(Expression.All());

                
                int a = 0;
                foreach (long uid in uids)
                {
                    //Output
                    Console.Clear();
                    Console.WriteLine("Together files: " + uids.Count);
                    float x = (float) ((double)100 / uids.Count * a);
                    Console.WriteLine(x  + "% Loading... Files copied: " + a);
                    
                    
                    var eml = _imap.GetMessageByUID(uid);
                    string fileName = string.Format(@"/Users/kirill/Desktop/MailTask/MailTask/AllEmails/email_{0}.eml", uid);
                    File.WriteAllBytes(fileName, eml);
                    a++;
                }

                Console.WriteLine("Finished");
            };
            Thread thread = new Thread(parameterizedThreadStart);
            thread.Start();
            thread.IsBackground = background;

        }

        public void MessageShow(long uid)
        {           
            var messageInfo = _imap.GetMessageInfoByUID(uid);
            Console.WriteLine(messageInfo.Envelope.InternalDate.ToString());
            foreach (var messageInfoGmailLabel in messageInfo.GmailLabels)
            {
                Console.WriteLine(messageInfoGmailLabel);
            }
            foreach (var mailBox in messageInfo.Envelope.From)
            {
                Console.WriteLine(mailBox);
            }
            foreach (var mailAddress in messageInfo.Envelope.To)
            {
                Console.WriteLine(mailAddress);
            }
            Console.WriteLine(messageInfo.Envelope.Subject);
            
            IMail iMail = new MailBuilder().CreateFromEml(_imap.GetMessageByUID(uid));
            Console.WriteLine(iMail.Text);
            
            
            
            //no text
            
            Console.WriteLine();            
        }

        public void LastFiveUnseen()
        {
            _imap.SelectInbox();
            List<long> uids = _imap.Search(Flag.Unseen);

            int counter = 0;
            foreach (var uid in uids)
            {
                MessageShow(uid);
                if (counter == 5) break;
                counter++;
            }
        }
        
         
        public void AllMailsFlag(Flag flag)
        {
            _imap.SelectInbox();
            List<long> uids = _imap.Search(flag);

            Console.WriteLine(uids.Count);
            
            
//            foreach (var uid in uids)
//            {
//                MessageShow(uid);
//            }

foreach (var uid in uids)
{
    MessageShow(uid);
}
           












        }
    }
   
    

}

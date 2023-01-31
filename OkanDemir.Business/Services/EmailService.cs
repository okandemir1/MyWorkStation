using OkanDemir.Dto;
using System.Net;
using System.Net.Mail;

namespace OkanDemir.Business.Services
{
    public class EmailService
    {
        private List<string> _files = new List<string>();

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtmlBody { get; set; }
        public string Password { get; set; }
        public EmailProviderType EmailProvider { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }

        /// <summary>
        /// Get or Set files to attach with email.
        /// </summary>
        public string[] AttachmentFileArray
        {
            get
            {
                int a = 0;
                if (_files != null)
                    a = _files.Count;
                string[] f = new string[a];
                int count = -1;
                foreach (string s in _files)
                {
                    count++;
                    f[count] = s;
                }
                return f;
            }
            set
            {
                _files = new List<string>();
                foreach (string s in value)
                {
                    _files.Add(s);
                }
            }
        }

        /// <summary>
        /// Get or Set files to attach with email.
        /// </summary>
        public List<string> AttachmentFileList
        {
            get
            {
                return _files;
            }
            set
            {
                _files = value;
            }
        }

        /// <summary>
        /// Set a file to attach with email.
        /// </summary>
        public string AttachFile
        {
            set
            {
                _files = new List<string>();
                _files.Add(value);
            }
        }

        public enum EmailProviderType
        {
            Yahoo,
            Gmail,
            Hotmail,
            Natro,
            Yandex,
            Other
        }

        public DbOperationResult Send(string host = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(From);
                mail.To.Add(To);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = IsHtmlBody;
                foreach (string s in _files)
                {
                    mail.Attachments.Add(new Attachment(s));
                }

                SetProvider(host);

                SendMail(mail);

                return new DbOperationResult(true, "Mail gönderilmiştir (Lütfen spam bölümünü de kontrol etmeyi unutmayınız)");
            }
            catch (Exception exc)
            {
                return new DbOperationResult(true, exc.Message);
            }
        }

        public void Send(MailMessage mail, string password, EmailProviderType emailProvider)
        {
            Password = password;
            EmailProvider = emailProvider;

            SetProvider();

            SendMail(mail);
        }

        public DbOperationResult Send(string from, string to, string subject, string body,
            string password, EmailProviderType emailProvider, string host = "")
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            IsHtmlBody = true;
            Password = password;
            EmailProvider = emailProvider;

            return Send(host);
        }

        private void SetProvider(string host = "")
        {
            switch (EmailProvider)
            {
                case EmailProviderType.Yahoo:
                    Host = "smtp.mail.yahoo.com";
                    Port = 587;
                    EnableSSL = false;
                    break;
                case EmailProviderType.Gmail:
                    Host = "smtp.gmail.com";
                    Port = 587;
                    EnableSSL = true;
                    break;
                case EmailProviderType.Hotmail:
                    Host = "smtp.live.com";
                    Port = 587;
                    EnableSSL = true;
                    break;
                case EmailProviderType.Yandex:
                    Host = "smtp.yandex.com.tr";
                    Port = 587;
                    EnableSSL = true;
                    break;
                case EmailProviderType.Natro: // send host with like "mail.otososyal.com"
                    Host = host;
                    Port = 587;
                    EnableSSL = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// EmailSender es = new EmailSender();
        /// es.From = "myemail@yahoo.com";
        /// es.To = "anyaddress@anydomain.com";
        /// es.Password = "mypwd";
        /// es.Subject = "Test Mail";
        /// es.Body = "hello";
        /// es.IsHtmlBody = false;
        /// es.EmailProvider = EmailSender.EmailProviderType.Yahoo;
        /// es.Send(); 
        /// </summary>
        /// <param name="mail"></param>
        private void SendMail(MailMessage mail)
        {
            CheckMailParameters();

            SmtpClient smtp = new SmtpClient(Host, Port);
            smtp.Credentials = new NetworkCredential(From, Password);
            smtp.EnableSsl = EnableSSL;
            smtp.Send(mail);

            smtp = null;
            mail = null;
        }

        private void CheckMailParameters()
        {
            if (From + "" == "")
                throw new Exception("Email (From) address is blank.");
            if (To + "" == "")
                throw new Exception("Email (To) address is blank.");
            if (Password + "" == "")
                throw new Exception("Password is blank.");
            if (Host == "")
                throw new Exception("Host is blank.");
            if (Port == 0)
                throw new Exception("Port is blank.");
        }
    }
}

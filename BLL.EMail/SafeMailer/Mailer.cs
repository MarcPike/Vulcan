using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace BLL.EMail.SafeMailer
{

    /// <summary>
    /// Provides SMTP email functionality.
    /// </summary>
    public class Mailer
    {

        #region "Variables"

        private List<MailAddress> _toRecipients = new List<MailAddress>();
        private List<MailAddress> _ccRecipients = new List<MailAddress>();
        private List<MailAddress> _bccRecipients = new List<MailAddress>();

        private string _subject;
        private string _body;

        private const string TEST_SUBJECT = "TESTING. PLEASE DISREGARD! ";

        private EmailFormat _mailFormat = EmailFormat.PlainText;


        #endregion


        #region Enums

        public enum EmailFormat
        {
            HTML,
            PlainText
        }

        #endregion


        #region "Properties"

        /// <summary>
        /// CSV text that will be added to the email as an attachment.
        /// </summary>
        public string AttachmentCSV { get; set; }

        /// <summary>
        /// Gets or sets the body text of the email to be sent. Must include HTML tags in HTML mode. 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled.
        /// </summary>
        //public bool EnableSSL { get; set; }

        /// <summary>
        /// Gets or sets the From email address.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the mail format (HTML or Plain Text).
        /// </summary>
        public EmailFormat MailFormat
        {
            get { return _mailFormat; }
            set { _mailFormat = value; }
        }

        /// <summary>
        /// When testing, set to True to prevent emails from being sent to real recipients. When true, 
        /// the SendMail() method will ignore any recipients that have been added to the recipient properties, and will instead
        /// send an email to the Test_Email address specified in the config file. The subject and body
        /// properties will also have a 'Testing' message appended to the front.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Automatically set to True (in the constructor) when in DEBUG mode, but this can be overwritten if required.</remarks>
        public bool SafeMode { get; set; }

        /// <summary>
        /// Gets or sets the subject line of the email.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Appends TEST_SUBJECT to front when SafeMode = True.</remarks>
        public string Subject
        {
            get
            {
                if (!SafeMode)
                {
                    return _subject;
                }
                else
                {
                    return TEST_SUBJECT + _subject;
                }
            }
            set { _subject = value; }
        }



        //*******************************************************
        //* Shared Properties that read values from config file *
        //*******************************************************

        /// <summary>
        /// Gets a comma-separated list of emails from the config file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Admin_Emails
        {
            get { return HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["Admin_Emails"]); }
        }

        /// <summary>
        /// Gets the From_Email from the config file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string From_Email
        {
            get { return HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["From_Email"]); }
        }

        /// <summary>
        /// Gets the Test_Email from the config file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Test_Email
        {
            get { return HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["Test_Email"]); }
        }

        /// <summary>
        /// Gets the SMTP Server specification from the configuration file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SMTPServer
        {
            get { return ConfigurationManager.AppSettings["SMTP_Host"]; }
        }

        /// <summary>
        /// Gets the SMTP UserName from the config file.
        /// </summary>
        public static string SMTP_UserName
        {
            get { return ConfigurationManager.AppSettings["SMTP_Username"]; }
        }

        /// <summary>
        /// Gets the SMTP Password from the config file.
        /// </summary>
        public static string SMTP_Password
        {
            get { return ConfigurationManager.AppSettings["SMTP_Password"]; }
        }

        /// <summary>
        /// Gets the SMTP_EnableSSL value from the config file.
        /// </summary>
        public static bool SMTP_EnableSSL
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["SMTP_EnableSSL"]); }
        }

        /// <summary>
        /// Gets Email_SafeMode. If set to true, then Mailer class ignores everyone in To, CC, and BCC lists, 
        /// and instead sends the email only to the recipients in the Test_Email config file entry.
        /// </summary>
        public static bool Email_SafeMode
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["Email_SafeMode"]); }
        }


        #endregion


        #region "Constructor"


        public Mailer()
        {
            SafeMode = Mailer.Email_SafeMode;
        }

        #endregion


        #region "Public Methods"

        /// <summary>
        /// Adds one or more email addresses to the TO recipients list from a comma-separated list of addresses.
        /// </summary>
        /// <param name="csv_list"></param>
        /// <remarks></remarks>
        public void AddToRecipients(string csv_list)
        {
            if (string.IsNullOrEmpty(csv_list)) return;

            string[] addresses = csv_list.Split(',');

            foreach (string address in addresses)
            {
                _toRecipients.Add(new MailAddress(address));
            }

        }

        /// <summary>
        /// Adds a single email recipient to the TO list.
        /// </summary>
        /// <param name="recipientName"></param>
        /// <param name="recipientEmail"></param>
        /// <remarks></remarks>
        public void AddToRecipients(string recipientName, string recipientEmail)
        {
            MailAddress recipient = new MailAddress(String.Format("{0}<{1}>", recipientName, recipientEmail));
            _toRecipients.Add(recipient);
        }

        /// <summary>
        /// Adds one or more email addresses to the CC recipients list from a comma-separated list of addresses.
        /// </summary>
        /// <param name="csv_list"></param>
        /// <remarks></remarks>
        public void AddCcRecipients(string csv_list)
        {
            if (string.IsNullOrEmpty(csv_list))
                return;

            string[] addresses = csv_list.Split(',');

            foreach (string address in addresses)
            {
                _ccRecipients.Add(new MailAddress(address));
            }

        }

        /// <summary>
        /// Adds a single email recipient to the CC list.
        /// </summary>
        /// <param name="recipientName"></param>
        /// <param name="recipientEmail"></param>
        /// <remarks></remarks>
        public void AddCcRecipients(string recipientName, string recipientEmail)
        {
            MailAddress recipient = new MailAddress(String.Format("{0}<{1}>", recipientName, recipientEmail));
            _ccRecipients.Add(recipient);
        }

        /// <summary>
        /// Adds one or more email addresses to the BCC recipients list from a comma-separated list of addresses.
        /// </summary>
        /// <param name="csv_list"></param>
        /// <remarks></remarks>
        public void AddBccRecipients(string csv_list)
        {
            if (string.IsNullOrEmpty(csv_list))
                return;

            string[] addresses = csv_list.Split(',');

            foreach (string address in addresses)
            {
                _bccRecipients.Add(new MailAddress(address));
            }

        }

        /// <summary>
        /// Adds a single email recipient to the BCC list.
        /// </summary>
        /// <param name="recipientName"></param>
        /// <param name="recipientEmail"></param>
        /// <remarks></remarks>
        public void AddBccRecipients(string recipientName, string recipientEmail)
        {
            MailAddress recipient = new MailAddress(String.Format("{0}<{1}>", recipientName, recipientEmail));
            _bccRecipients.Add(recipient);
        }


        /// <summary>
        /// Clears all recipients in the TO, CC, and BCC fields.
        /// </summary>
        /// <remarks></remarks>
        public void ClearAllRecipients()
        {
            _toRecipients.Clear();
            _ccRecipients.Clear();
            _bccRecipients.Clear();
        }

        /// <summary>
        /// Clears the TO recipient list.
        /// </summary>
        /// <remarks></remarks>
        public void ClearToRecipients()
        {
            _toRecipients.Clear();
        }

        /// <summary>
        /// Clears the CC recipient list.
        /// </summary>
        /// <remarks></remarks>
        public void ClearCcRecipients()
        {
            _ccRecipients.Clear();
        }

        /// <summary>
        /// Clears the BCC recipient list.
        /// </summary>
        /// <remarks></remarks>
        public void ClearBccRecipients()
        {
            _bccRecipients.Clear();
        }



        /// <summary>
        /// Sends an HTML-formatted email with the subject, body, and recipients defined in the relevant Mailer class properties.
        /// </summary>
        /// <remarks></remarks>
        public void SendMail()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.IsBodyHtml = (_mailFormat == EmailFormat.HTML);
                mail.From = new MailAddress(From);

                //Just to be extra safe...
                mail.To.Clear();
                mail.CC.Clear();
                mail.Bcc.Clear();

                //Add recipients
                if (!SafeMode)
                {
                    foreach (MailAddress recipient in _toRecipients) mail.To.Add(recipient);
                    foreach (MailAddress recipient in _ccRecipients) mail.CC.Add(recipient);
                    foreach (MailAddress recipient in _bccRecipients) mail.Bcc.Add(recipient);
                }
                else
                {
                    //In Safe mode, only add the Test_Email address.
                    string[] testRecipients = Mailer.Test_Email.Split(',');
                    foreach (string testRecipient in testRecipients) mail.To.Add(testRecipient);
                }

                //Subject and Body
                //(use Subject property rather than field so that it appends test subject notification when SafeMode = True)
                mail.Subject = Subject;
                mail.Body = Body;

                //Create SMTP client and set credentials
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = Mailer.SMTP_EnableSSL;
                    smtp.Host = Mailer.SMTPServer;
                    smtp.Credentials = new System.Net.NetworkCredential(Mailer.SMTP_UserName, Mailer.SMTP_Password);

                    //Add CSV attachment
                    if (!string.IsNullOrEmpty(this.AttachmentCSV))
                    {
                        byte[] data = ASCIIEncoding.Default.GetBytes(this.AttachmentCSV);
                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            mail.Attachments.Add(new Attachment(ms, "", "text/csv"));
                            //Send it with attachment
                            smtp.Send(mail);
                        }
                    }
                    else
                    {
                        //Send it without attachment
                        smtp.Send(mail);
                    }
                } // using
            }

        }

        #endregion


    } // class

} // namespace
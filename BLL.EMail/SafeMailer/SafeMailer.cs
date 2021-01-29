using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace BLL.EMail.SafeMailer
{

    /// <summary>
    /// A simplified version of Mailer that provides "SafeMode" for testing.
    /// </summary>
    public class SafeMailer
    {

        #region Field Members

        private List<MailAddress> _toRecipients = new List<MailAddress>();
        private List<MailAddress> _ccRecipients = new List<MailAddress>();
        private List<MailAddress> _bccRecipients = new List<MailAddress>();

        private string _subject;
        //private string _body;

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


        #region Properties

        /// <summary>
        /// Gets or sets the body text of the email to be sent. Must include HTML tags in HTML mode. 
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the From email address.  Must be a single email address.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the TestEmail address (can be multiple addresses separated with commas).
        /// TestEmail is only used when SafeMode = true;
        /// </summary>
        public string TestEmail { get; set; }

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
        public bool SafeMode { get; set; }

        /// <summary>
        /// Gets or sets the subject line of the email.
        /// Appends TEST_SUBJECT to front when SafeMode = True.
        /// </summary>
        public string Subject
        {
            get { return (!SafeMode) ? _subject : TEST_SUBJECT + _subject; }
            set { _subject = value; }
        }

        /// <summary>
        /// Gets or sets the SMTP Host.
        /// </summary>
        public string SMTP_Host { get; set; }

        /// <summary>
        /// Returns true if any recipients are defined in either the To, CC, or BCC fields.
        /// </summary>
        public bool HasRecipients
        {
            get
            {
                return ((_toRecipients != null && _toRecipients.Count > 0) ||
                        (_ccRecipients != null && _ccRecipients.Count > 0) ||
                        (_bccRecipients != null && _bccRecipients.Count > 0));
            }
        }

        #endregion


        #region Constructors




        #endregion


        #region Public Methods

        /// <summary>
        /// Adds one or more email addresses to the TO recipients list from a comma-separated list of addresses.
        /// </summary>
        /// <param name="csvList"></param>
        /// <remarks></remarks>
        public void AddToRecipients(string csvList)
        {
            if (string.IsNullOrWhiteSpace(csvList)) return;

            var addresses = csvList.Split(',');

            foreach (var address in addresses)
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (_toRecipients.All(r => r.Address != address)) _toRecipients.Add(new MailAddress(address));
                }
            }
        }

        /// <summary>
        ///  Adds a single email recipient to the TO list.
        /// </summary>
        /// <param name="recipientEmail"></param>
        /// <param name="recipientDisplayName"></param>
        public void AddToRecipients(string recipientEmail, string recipientDisplayName)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail)) return;
            if (_toRecipients.All(r => r.Address != recipientEmail))
            {
                var combinedAddress = (!string.IsNullOrWhiteSpace(recipientDisplayName)) ? String.Format("{0}<{1}>", recipientDisplayName, recipientEmail) : recipientEmail;
                var recipient = new MailAddress(combinedAddress);
                _toRecipients.Add(recipient);
            }
        }

        /// <summary>
        /// Adds one or more email addresses to the CC recipients list from a comma-separated list of addresses.
        /// </summary>
        /// <param name="csvList"></param>
        /// <remarks></remarks>
        public void AddCcRecipients(string csvList)
        {
            if (string.IsNullOrWhiteSpace(csvList)) return;

            var addresses = csvList.Split(',');

            foreach (var address in addresses)
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (_ccRecipients.All(r => r.Address != address)) _ccRecipients.Add(new MailAddress(address));
                }
            }
        }

        /// <summary>
        ///  Adds a single email recipient to the CC list.
        /// </summary>
        /// <param name="recipientEmail"></param>
        /// <param name="recipientDisplayName"></param>
        public void AddCcRecipients(string recipientEmail, string recipientDisplayName)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail)) return;
            if (_ccRecipients.All(r => r.Address != recipientEmail))
            {
                var combinedAddress = (!string.IsNullOrWhiteSpace(recipientDisplayName)) ? String.Format("{0}<{1}>", recipientDisplayName, recipientEmail) : recipientEmail;
                var recipient = new MailAddress(combinedAddress);
                _ccRecipients.Add(recipient);
            }
        }

        /// <summary>
        /// Adds one or more email addresses to the BCC recipients list from a comma-separated list of addresses.
        /// </summary>
        /// <param name="csvList"></param>
        /// <remarks></remarks>
        public void AddBccRecipients(string csvList)
        {
            if (string.IsNullOrWhiteSpace(csvList)) return;

            var addresses = csvList.Split(',');

            foreach (var address in addresses)
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    if (_bccRecipients.All(r => r.Address != address)) _bccRecipients.Add(new MailAddress(address));
                }
            }
        }

        /// <summary>
        ///  Adds a single email recipient to the BCC list.
        /// </summary>
        /// <param name="recipientEmail"></param>
        /// <param name="recipientDisplayName"></param>
        public void AddBccRecipients(string recipientEmail, string recipientDisplayName)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail)) return;
            if (_bccRecipients.All(r => r.Address != recipientEmail))
            {
                var combinedAddress = (!string.IsNullOrWhiteSpace(recipientDisplayName)) ? String.Format("{0}<{1}>", recipientDisplayName, recipientEmail) : recipientEmail;
                var recipient = new MailAddress(combinedAddress);
                _bccRecipients.Add(recipient);
            }
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
            //Validate input first.
            if (string.IsNullOrWhiteSpace(SMTP_Host)) throw new ArgumentNullException("SMTP_Host cannot be Empty.");
            if (string.IsNullOrWhiteSpace(FromEmail)) throw new ArgumentNullException("'From Email' cannot be Empty.");
            if (!HasRecipients) throw new ArgumentNullException("No recipients are defined.");

            //Good to go.
            using (var mail = new MailMessage())
            {
                mail.IsBodyHtml = (_mailFormat == EmailFormat.HTML);
                mail.From = new MailAddress(FromEmail);

                //Just to be extra safe...
                mail.To.Clear();
                mail.CC.Clear();
                mail.Bcc.Clear();

                //Add recipients
                if (!SafeMode)
                {
                    foreach (var recipient in _toRecipients) mail.To.Add(recipient);
                    foreach (var recipient in _ccRecipients) mail.CC.Add(recipient);
                    foreach (var recipient in _bccRecipients) mail.Bcc.Add(recipient);
                }
                else
                {
                    //In Safe mode, only add the Test_Email address.
                    var testRecipients = TestEmail.Split(',');
                    foreach (var testRecipient in testRecipients) mail.To.Add(testRecipient);
                }

                //Subject and Body
                //(use Subject property rather than field so that it appends test subject notification when SafeMode = True)
                mail.Subject = Subject;
                mail.Body = Body;

                //Create SMTP client and set credentials
                using (var smtp = new SmtpClient(SMTP_Host))
                {
                    smtp.Send(mail);
                }
            }
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// Creates a SafeMailer object with the specified parameters and sends an HTML-formatted email.
        /// </summary>
        /// <param name="smtpHost"></param>
        /// <param name="csvRecipients"></param>
        /// <param name="fromEmail"></param>
        /// <param name="testEmail"></param>
        /// <param name="safeMode"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void QuickSendHTML(string smtpHost, string csvRecipients, string fromEmail, string testEmail, bool safeMode, string subject, string body)
        {
            var safeMailer = new SafeMailer()
            {
                SMTP_Host = smtpHost,
                FromEmail = fromEmail,
                TestEmail = testEmail,
                SafeMode = safeMode,
                Subject = subject,
                Body = body,
                MailFormat = EmailFormat.HTML,
            };

            safeMailer.AddToRecipients(csvRecipients);
            safeMailer.SendMail();
        }

        /// <summary>
        /// Creates a SafeMailer object with the specified parameters and sends plain text email.
        /// </summary>
        /// <param name="smtpHost"></param>
        /// <param name="csvRecipients"></param>
        /// <param name="fromEmail"></param>
        /// <param name="testEmail"></param>
        /// <param name="safeMode"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void QuickSendPlainText(string smtpHost, string csvRecipients, string fromEmail, string testEmail, bool safeMode, string subject, string body)
        {
            var safeMailer = new SafeMailer()
            {
                SMTP_Host = smtpHost,
                FromEmail = fromEmail,
                TestEmail = testEmail,
                SafeMode = safeMode,
                Subject = subject,
                Body = body,
                MailFormat = EmailFormat.PlainText,
            };

            safeMailer.AddToRecipients(csvRecipients);
            safeMailer.SendMail();
        }

        #endregion

    }
}
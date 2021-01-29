Ways of producing an EMail:

SendEMail.Execute(string subject, List<string> emailRecipients, string body, string smtpHost = null, string emailFromAddress = null)

-or-

var eMailBuilder = new eMailBuilder()
{
	Subject = subject,
	Body = body,
	Recipients = emailRecipients
};

emailBuilder.SmtpHost = smtpHost;

emailBuilder.EMailFromAddress = emailFromAddress;

emailBuilder.Send();
	

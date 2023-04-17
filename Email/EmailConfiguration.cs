  using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
namespace Agro_Express.Email
{
    public class EmailConfiguration
    {
        public static void EmailSending(string reciepientEmail, string reciepientName, string subject, string body)
        {
            
              Configuration.Default.ApiKey.Add("api-key", "api-key");

                var apiInstance = new TransactionalEmailsApi();
                string SenderName = "Wazobia Agro Express";
                string SenderEmail = "tijaniadebayoabdllahi@gmail.com";
                SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
                string ToEmail = reciepientEmail;
                string ToName = reciepientName;
                SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
                List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
                To.Add(smtpEmailTo);

                //Bcc the reciever also have the copy of the message but name do not visisble to othe reciepient of the email
                string BccName = reciepientName;
                string BccEmail = reciepientEmail;
                SendSmtpEmailBcc BccData = new SendSmtpEmailBcc(BccEmail, BccName);
                List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>();
                Bcc.Add(BccData);

                //CC Sender also recieve the email
                string CcName = "Wazobia agro Wxpress";
                string CcEmail = "tijaniadebayoabdllahi@gmail.com.com";
                SendSmtpEmailCc CcData = new SendSmtpEmailCc(CcEmail, CcName);
                List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
                Cc.Add(CcData);
                string HtmlContent = "<html><body><h1>From Wazobia Agro Express</h1><br><h3> {{params.parameter}}</h3></body></html>";
                string TextContent = null;
                string Subject = "{{params.subject}}";
                string ReplyToName = "Wazobia Agro Express";
                string ReplyToEmail = "tijaniadebayoabdllahi@gmail.com";
                SendSmtpEmailReplyTo ReplyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
                string AttachmentUrl = null;
                 string stringInBase64 = "aGVsbG8gdGhpcyBpcyB0ZXN0";
                byte[] Content = System.Convert.FromBase64String(stringInBase64);
                 string AttachmentName = "test.txt";
                SendSmtpEmailAttachment AttachmentContent = new SendSmtpEmailAttachment(AttachmentUrl, Content, AttachmentName);
                List<SendSmtpEmailAttachment> Attachment = new List<SendSmtpEmailAttachment>();
                Attachment.Add(AttachmentContent);
                JObject Headers = new JObject();
                Headers.Add("Some-Custom-Name", "unique-id-1234");
                long? TemplateId = null;
                JObject Params = new JObject();

                //this is subtituted by the params.parameter
                Params.Add("parameter", body);

                //this is subtituted by the params.subbject
                Params.Add("subject", subject);
                List<string> Tags = new List<string>();
                Tags.Add("mytag");
                SendSmtpEmailTo1 smtpEmailTo1 = new SendSmtpEmailTo1(ToEmail, ToName);
                List<SendSmtpEmailTo1> To1 = new List<SendSmtpEmailTo1>();
                To1.Add(smtpEmailTo1);
                Dictionary<string, object> _parmas = new Dictionary<string, object>();
                _parmas.Add("params", Params);
                SendSmtpEmailReplyTo1 ReplyTo1 = new SendSmtpEmailReplyTo1(ReplyToEmail, ReplyToName);
                SendSmtpEmailMessageVersions messageVersion = new SendSmtpEmailMessageVersions(To1, _parmas, Bcc, Cc, ReplyTo1, Subject);
                List<SendSmtpEmailMessageVersions> messageVersiopns = new List<SendSmtpEmailMessageVersions>();
                messageVersiopns.Add(messageVersion);
                try
                {
                    var sendSmtpEmail = new SendSmtpEmail(Email, To, Bcc, Cc, HtmlContent, TextContent, Subject, ReplyTo, Attachment, Headers, TemplateId, Params, messageVersiopns, Tags);
                    CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                    // Debug.WriteLine(result.ToJson());
                    // Console.WriteLine(result.ToJson());
                    // Console.ReadLine();
                }
                catch (Exception ex)
                {
                    throw ex;
                    // Debug.WriteLine(ex.Message);
                    // Console.WriteLine(ex.Message);
                    // Console.ReadLine();
                }
    
            }
    }
}
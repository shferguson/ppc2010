using System;
using System.Net.Mail;
using System.Web.UI;
using PPC_2010.Data;

namespace PPC_2010
{
    public partial class ContactForm : System.Web.UI.UserControl
    {
        public string EmailGroupName { get; set; }
        public string ThanksPageUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            title.Text = string.Format("Contact {0}", EmailGroupName);

            if (Page.IsPostBack)
            {
                SendEmail();
                Response.Redirect(ThanksPageUrl);
            }
        }

        private void SendEmail()
        {
            var repo = ServiceLocator.Instance.Locate<IEmailGroupRepository>();
            var emailGroup = repo.GetEmailGroupByName(EmailGroupName);

            if (emailGroup != null)
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("noreplay@providence-pca.net");
                foreach (var emailAddress in emailGroup.EmailAddresses)
                    mailMessage.To.Add(emailAddress);
                mailMessage.ReplyToList.Add(email.Text);
                mailMessage.Subject = subject.Text;
                mailMessage.Body = message.Text;

                using (var smptClient = new SmtpClient())
                {
                    smptClient.Send(mailMessage);
                }
            }
        }
    }
}
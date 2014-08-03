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
                Page.Validate();
                if (!Page.IsValid)
                    return;

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
                mailMessage.From = new MailAddress("noreply@providence-pca.net");
                foreach (var emailAddress in emailGroup.EmailAddresses)
                    mailMessage.To.Add(emailAddress);
                mailMessage.ReplyToList.Add(email.Text);
                mailMessage.Subject = subject.Text.Length > 0 ? string.Format("{0} - {1}", emailGroup.Name, subject.Text) : emailGroup.Name;

                string fromName = null;
                if (name.Text.Length == 0)
                    fromName = email.Text;
                else
                    fromName = string.Format("{0} ({1})", name.Text, email.Text);

                string formattedMessage = string.Format("You have a new message from {0} about the {1} ministry at Providence.\r\nYou can reply directly to this email.\r\n\r\n\r\n{2}",
                    fromName, emailGroup.Name, message.Text);

                mailMessage.Body = formattedMessage;
                
                using (var smptClient = new SmtpClient())
                {
                    smptClient.Send(mailMessage);
                }
            }
        }
    }
}
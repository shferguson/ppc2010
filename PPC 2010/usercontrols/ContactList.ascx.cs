using System;
using System.Collections.Generic;
using System.Web.UI;
using umbraco.cms.businesslogic.media;
using umbraco.NodeFactory;
using Umbraco.Core.Services;

namespace PPC_2010
{
    public partial class ContactList : System.Web.UI.UserControl
    {
        private class Contact
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string ImageUrl { get; set; }
        }

        public int CurrentPageId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string baseUrl = string.Format("{0}", (Request.ApplicationPath.Equals("/")) ? string.Empty : Request.ApplicationPath);

                Node currentPage = new Node(CurrentPageId);
                List<Contact> contactList = new List<Contact>();
                foreach (Node node in currentPage.Children)
                {
                    if (node.NodeTypeAlias == "Contact")
                    {
                        Contact contact = new Contact();
                        contact.Name = node.Name;
                        contact.Position = node.GetProperty("position").Value ?? "";
                        contact.Email = node.GetProperty("email").Value ?? "";
                        contact.Phone = node.GetProperty("phone").Value ?? "";

                        int imageId = 0;
                        if (int.TryParse(node.GetProperty("image").Value ?? "", out imageId))
                        {
                            var mediaService = ServiceLocater.Instance.Locate<IMediaService>();
                            var media = mediaService.GetById(imageId);
                            contact.ImageUrl = media.GetValue<string>("umbracoFile").Replace("~", baseUrl);
                        }

                        contactList.Add(contact);
                    }
                }

                contacts.DataSource = contactList;
                contacts.DataBind();
            }
        }
    }
}
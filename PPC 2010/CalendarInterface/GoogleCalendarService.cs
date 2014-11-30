using System.Security.Cryptography.X509Certificates;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System.Web.Hosting;
using System;

namespace PPC_2010.CalendarInterface
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        public CalendarService Service { get; private set; }
        public byte[] FileBytes { get; set; }

        public GoogleCalendarService(string serviceAccountEmail, string serviceAcountCertFilePath)
        {
            // We need to add the X509KeyStorageFlags.MachineKey to work in shared hosting were we
            // don't have access to the user account
            // http://stackoverflow.com/questions/1345262/an-internal-error-occurred-when-loading-pfx-file-with-x509certificate2
            // On Arvixe the site would lockup and kill the AppPool
            X509Certificate2 certificate = new X509Certificate2(HostingEnvironment.MapPath(serviceAcountCertFilePath),
                "notasecret",
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);

            var credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = new[] { CalendarService.Scope.Calendar }
            }.FromCertificate(certificate));


            Service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "www.providence-pca.net",
            });
        }

        public void Dispose()
        {
            if (Service != null) Service.Dispose();
        }
    }
}
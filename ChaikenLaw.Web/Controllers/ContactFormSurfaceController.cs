using ChaikenLaw.Web.Models;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace ChaikenLaw.Web.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {

        public async Task<ActionResult> Index()
        {

            return PartialView("ContactForm", new ContactFormViewModel());
        }

        [HttpPost]
        public JsonResult Index(ContactFormViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var msg = new SendGridMessage();
                    msg.SetFrom(new EmailAddress("info@chaikenlaw.com", "Chaiken & Chaiken"));

                    var recipients = new List<EmailAddress>
                    {
                        new EmailAddress(viewModel.Email, viewModel.Name)
                    };
                    msg.AddTos(recipients);
                    msg.SetTemplateId("d-3194f200f65745e0a3b70c758f2f6d7c");
                    msg.SetTemplateData(new ContactFormEmail
                    {
                        Name = viewModel.Name
                    });

                    var apiKey = ConfigurationManager.AppSettings["SENDGRID_APIKEY"]; // System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                    var client = new SendGridClient(apiKey);
                    var response = client.SendEmailAsync(msg);


                    var infoMsg = new SendGridMessage();
                    infoMsg.SetFrom(new EmailAddress("info@chaikenlaw.com", "Chaiken & Chaiken"));

                    var infoRecipients = new List<EmailAddress>
                    {
                        new EmailAddress("rchaiken@chaikenlaw.com", "Robert Chaiken"),
                        new EmailAddress("kchaiken@chaikenlaw.com", "Kenneth Chaiken")
                    };
                    infoMsg.AddTos(infoRecipients);
                    infoMsg.AddBcc("sapplegate86@gmail.com", "Steven Applegate");
                    infoMsg.SetTemplateId("d-cbfb7d31b57b4a2e9f85554850944847");
                    infoMsg.SetTemplateData(new ContactFormEmail
                    {
                        Name = viewModel.Name,
                        Email = viewModel.Email,
                        Phone = viewModel.Phone,
                        Message = viewModel.Message
                    });
                    response = client.SendEmailAsync(infoMsg);


                    return Json(new { success = true, id = 123 });
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }



        private class ContactFormEmail
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("phone")]
            public string Phone { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
        }
    }
}
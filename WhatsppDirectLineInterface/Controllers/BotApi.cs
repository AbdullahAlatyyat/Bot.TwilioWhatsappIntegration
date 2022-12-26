using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace WhatsppDirectLineInterface.Controllers
{
    [Route("api/[controller]")]
    public class BotApiController : Controller
    {
        DirectLineRepo _directLineRepo;
        public BotApiController(DirectLineRepo directLineRepo)
        {
            this._directLineRepo = directLineRepo;
        }

        [HttpGet]
        public string Get()
        {
            return "Done!";
        }

        [HttpPost]
        public async Task<IActionResult> Post(string From, string To, string Body, string MediaUrl0, string Latitude, string Longitude)
        {
            Console.WriteLine($"{Body} {From} {To}");

            try
            {
                // Get the user connection 
                var userId = From;
                var directlineConnector = _directLineRepo.GetClientConversation(userId);
                // Send Activity and wait messages
                if (!string.IsNullOrWhiteSpace(Latitude) && !string.IsNullOrWhiteSpace(Longitude))
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
                    {
                        {"Latitude", Latitude},
                        {"Longitude",Longitude }
                    };
                    Body = JsonConvert.SerializeObject(keyValuePairs);
                }
                await directlineConnector.SendText(From, Body);
                var activites = await directlineConnector.ReadBotMessages(userId);
                // Create WhatsApp Response
                var messagingResponse = new MessagingResponse();
                foreach (var item in activites)
                {
                    var msg = item.Text.Trim();
                    var message = new Message();
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        bool isUri = Uri.IsWellFormedUriString(msg, UriKind.RelativeOrAbsolute);
                        if (isUri)
                        {
                            message.Media(new Uri(msg));
                        }
                        else
                        {
                            message.AddText(msg);
                        }
                        messagingResponse.Append(message);
                    }
                }
                // Serialize XML Response
                Response.ContentType = "application/xml";
                return new ContentResult
                {
                    Content = new TwiMLResult(messagingResponse).Data.ToString(),
                    ContentType = "text/xml",
                    StatusCode = 200,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}, {ex.StackTrace}");
                var messagingResponse = new MessagingResponse();
                messagingResponse.Message("Cannot Send Messages to Bot");

                return new ContentResult
                {
                    Content = new TwiMLResult(messagingResponse).Data.ToString(),
                    ContentType = "text/xml",
                    StatusCode = 200,
                };
            }
        }
    }
}

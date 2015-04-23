using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Contacts;
using Google.GData.Client;
using Google.GData.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code;

namespace WebApplication1.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            // Get the tokens from the FileDataStore
            var token = new FileDataStore("Google.Apis.Auth")
                .GetAsync<TokenResponse>("user");


            //UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //    new ClientSecrets
            //    {
            //        ClientId = "68942521982-f9h411ncn58sg54s75jctuckes2ibevo.apps.googleusercontent.com",
            //        ClientSecret = "9wyNRGLHjm_Iv_8oUtz0Eojk",
            //    },
            //    new[] { CalendarService.Scope.Calendar, "https://www.google.com/m8/feeds/" }, // This will ask the client for concent on calendar and contatcs
            //    "user", 
            //    CancellationToken.None).Result;
            //// Create the calendar service.
            //CalendarService cal_service = new CalendarService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "EventManagement",
            //});

            // How do I use the found credential to create a ContactsService????
            ContactsService service = new ContactsService("EventManagement");

              OAuth2Parameters parameters = new OAuth2Parameters()
            {
                ClientId = "68942521982-f9h411ncn58sg54s75jctuckes2ibevo.apps.googleusercontent.com",
                ClientSecret = "9wyNRGLHjm_Iv_8oUtz0Eojk",
                RedirectUri = "https://localhost:44300/signin-google",
                Scope = "https://docs.google.com/feeds/ ",
                State = "documents",
                AccessType = "offline"
            };
                parameters.AccessCode = Request.QueryString["code"];
                // it gets accesstoken from google
                Google.GData.Client.OAuthUtil.GetAccessToken(parameters);
                GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "EventManagement", parameters);
                service.RequestFactory = requestFactory;
            //OAuth2Parameters parameters = new OAuth2Parameters
            //{
            //    ClientId = "68942521982-f9h411ncn58sg54s75jctuckes2ibevo.apps.googleusercontent.com",
            //    ClientSecret = "9wyNRGLHjm_Iv_8oUtz0Eojk",
            //    // Note: AccessToken is valid only for 60 minutes
            //   // AccessToken = token.Result.AccessToken,
            //   // RefreshToken = token.Result.RefreshToken
            //};
            RequestSettings settings = new RequestSettings("EventManagement",parameters);

            ContactsRequest cr = new ContactsRequest(settings);
            GContactData gdata = new GContactData();
            gdata.PrintDateMinQueryResults(cr);
            return View();
        }
    }
}
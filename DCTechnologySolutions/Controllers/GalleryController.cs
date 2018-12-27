using DCTechnologySolutions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DCTechnologySolutions.Controllers
{
    public partial class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            return View("CAB");
        }

        public ActionResult CAB()
        {
            return View();
        }
        public ActionResult OJewelry()
        {
            return View();
        }

        public ActionResult Equity()
        {
            return View();
        }

        public ActionResult ArcGISSamples()
        {
            return View();
        }

        public ActionResult FitbitSamples()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FitbitActivity()
        {
            string fbID = ConfigurationManager.AppSettings["fitbit-clientId"];
            string encURL =  HttpUtility.UrlEncode(Url.Action("ReturnFromFitbit", null, null, Request.Url.Scheme));
            return Redirect("https://www.fitbit.com/oauth2/authorize?client_id=" + fbID + "&response_type=code&scope=activity&redirect_uri=" + encURL);
        }

        public async Task<ActionResult> ReturnFromFitbit(string code)
        {
            ViewBag.Message = "Your Return From Fitbit page.";
            System.Net.Http.HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.fitbit.com/");
            client.DefaultRequestHeaders.Accept.Clear();

            FitbitResponseModel model = new FitbitResponseModel();
            string responseString;
            // Get token from authorization code
            string fbID = ConfigurationManager.AppSettings["fitbit-clientId"];
            string fbPW = ConfigurationManager.AppSettings["fitbit-Secret"];
            var payload = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                //{ "redirect_uri", "https://localhost:44330/Home/ReturnFromFitbit"  }, // ~/Home/ReturnFromFitbit"
                { "redirect_uri", Url.Action("ReturnFromFitbit", null, null, Request.Url.Scheme)  }, // ~/Home/ReturnFromFitbit"
                { "code",  code },
                { "client_idx", fbID }
            };

            var authenticationData = Encoding.ASCII.GetBytes(fbID + ":" + fbPW);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://api.fitbit.com/oauth2/token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationData));
            request.Content = new FormUrlEncodedContent(payload);//, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await client.SendAsync(request);
            responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                model.isSuccessStatsCode = true;
                model.successPayload = JsonConvert.DeserializeObject<authSuccessPayload>(responseString);
            }
            else
            {
                model.payloadEerror = JsonConvert.DeserializeObject<authErrorPayload>(responseString);
                model.errorMessage = response.ReasonPhrase;
                model.statusCode = response.StatusCode;
                return View(model);
            }
            // Make Get Activity call
            client.DefaultRequestHeaders.Accept.Clear();
            request = new HttpRequestMessage(HttpMethod.Get, "https://api.fitbit.com/1/user/-/activities.json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.successPayload.access_token);
            response = await client.SendAsync(request);
            responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                model.activityLifetimePayload = JsonConvert.DeserializeObject<activityLifetimePayload>(responseString);
            }
            else
            {
                model.isSuccessStatsCode = false;
                model.apiError = JsonConvert.DeserializeObject<apiErrorPayload>(responseString);
                model.errorMessage = response.ReasonPhrase;
                model.statusCode = response.StatusCode;
                return View(model);
            }
            // Make Get Device call
            client.DefaultRequestHeaders.Accept.Clear();
            request = new HttpRequestMessage(HttpMethod.Get, "https://api.fitbit.com/1/user/-/devices.json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.successPayload.access_token);
            response = await client.SendAsync(request);
            responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                // Did not ask for access to get devices so this should never happen!
            }
            else
            {
                // model.isSuccessStatsCode = false; since is is expected, let's not mark it false until (if) I refactor
                model.apiError = JsonConvert.DeserializeObject<apiErrorPayload>(responseString);
                model.errorMessage = response.ReasonPhrase;
                model.statusCode = response.StatusCode;
                return View(model);
            }
            return View(model);
        }

        public ActionResult GoogleMapSamples()
        {
            GoogleMapSamplesModel gm = new GoogleMapSamplesModel();
            return View(gm);
        }

        public ActionResult TalkingBaseball()
        {
            return View();
        }
    }
}
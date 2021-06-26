using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using DevelopersApplication.Models;
using System.Web.Script.Serialization;
using DevelopersApplication.Models.ViewModels;

namespace DevelopersApplication.Controllers
{
    public class CoderController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CoderController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44384/api/");
        }
        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Coder/List
        public ActionResult List()
        {
            //Objective:Communicate with the coder web api to recieve a list of coders
            //curl :https://localhost:44384/api/coderdata/listcoders


            string url = "coderdata/listcoders";

            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The Response status code is ");
            //Debug.WriteLine(responseMessage.StatusCode);

            IEnumerable<CoderDto> coders = response.Content.ReadAsAsync<IEnumerable<CoderDto>>().Result;

            //Debug.WriteLine("The number of coders in the system: " + coders.Count());


            return View(coders);
        }

        // GET: Coder/Details/5
        public ActionResult Details(int id)
        {
            DetailsCoder ViewModel = new DetailsCoder();

            //Objective:Communicate with the coder web api to recieve the details of a coder provided the id 
            //curl :https://localhost:44384/api/coderdata/findcoder{id}


            string url = "coderdata/findcoder/" +id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The Response status code is ");
            Debug.WriteLine(response.StatusCode);

            CoderDto selectedcoder = response.Content.ReadAsAsync<CoderDto>().Result;

            Debug.WriteLine("Coder received:" + selectedcoder.Name);

            ViewModel.SelectedCoder = selectedcoder;

            //show associated programming languages with this coder
            url = "programminglanguagedata/listprogramminglanguagesforcoder/" + id;

            response = client.GetAsync(url).Result;

            IEnumerable<ProgrammingLanguageDto> ProgrammingLanguages = response.Content.ReadAsAsync<IEnumerable<ProgrammingLanguageDto>>().Result;

            ViewModel.ProgrammingLanguages = ProgrammingLanguages;

            url = "programminglanguagedata/listprogramminglanguagesnotassociatedwithcoder/" + id;

            response = client.GetAsync(url).Result;

            IEnumerable<ProgrammingLanguageDto> AvailableProgrammingLanguages = response.Content.ReadAsAsync<IEnumerable<ProgrammingLanguageDto>>().Result;

            ViewModel.AvailableProgrammingLanguages = AvailableProgrammingLanguages;



            return View(ViewModel);
        }


        //POST: Coder/Associate/{coderid}
        [HttpPost]
        [Authorize]
        public ActionResult Associate(int id, int LanguageId)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("Attempting to associate coder :" + id + " with programming language " + LanguageId);

            //call our api to associate animal with keeper
            string url = "coderdata/associatecoderwithprogramminglanguage/" + id + "/" + LanguageId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //GET: Coder/UnAssociate/{id}?LanguageId={languageid}
        [HttpGet]
        [Authorize]
        //[Route ("Coder/unassociate/{id}?languageid={languageid}")]
        public ActionResult UnAssociate(int id, int LanguageId)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("Attempting to unassociate animal :" + id + " with programminglanguage: " + LanguageId);

            //call our api to unassociate coder with programming language
            string url = "coderdata/unassociatecoderwithprogramminglanguage/" + id + "/" + LanguageId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // for error message
        public ActionResult Error()
        {

            return View();
        }

        // GET: Coder/New
        [Authorize]
        public ActionResult New()
        {
            //information about all careers in the system.
            //GET api/careerdata/listcareers

            string url = "careerdata/listcareers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CareerDto> CareerOptions = response.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;
            return View(CareerOptions);
        }

        // POST: Coder/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Coder coder)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(coder.Name);
            //objective: add a new coder into our system using the API
            //curl -H "Content-Type:application/json" -d @Coder.json https://localhost:44384/api/coderdata/addcoder 
            string url = "coderdata/addcoder";


            string jsonpayload = jss.Serialize(coder);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Coder/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateCoder ViewModel = new UpdateCoder();

            //the existing coder information
            string url = "coderdata/findcoder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CoderDto SelectedCoder = response.Content.ReadAsAsync<CoderDto>().Result;
            ViewModel.SelectedCoder = SelectedCoder;

            // all careers to choose from when updating this coder
            //the existing coder information
            url = "careerdata/listcareers/";
            response = client.GetAsync(url).Result;
            IEnumerable<CareerDto> CareerOptions = response.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;

            ViewModel.CareerOptions = CareerOptions;

            return View(ViewModel);
        }

        // POST: Coder/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Coder coder, HttpPostedFileBase CoderPic)
        {
            GetApplicationCookie();//get token credentials
            string url = "coderdata/updatecoder/" + id;
            string jsonpayload = jss.Serialize(coder);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            //update request sucessfull and the image has been uploaded
            if (response.IsSuccessStatusCode && CoderPic != null)
            {
                //Updating the coder picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for coder
                url = "CoderData/UploadCoderPic/" + id;
                //Debug.WriteLine("Received Coder Picture "+CoderPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(CoderPic.InputStream);
                requestcontent.Add(imagecontent, "CoderPic", CoderPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Coder/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "coderdata/findcoder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CoderDto selectedcoder = response.Content.ReadAsAsync<CoderDto>().Result;
            return View(selectedcoder);
        }

        // POST: Coder/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "coderdata/deletecoder/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}

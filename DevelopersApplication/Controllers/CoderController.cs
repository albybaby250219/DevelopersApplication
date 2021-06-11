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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44384/api/");
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


        //POST: Code/Associate/{coderid}
        [HttpPost]
        public ActionResult Associate(int id, int LanguageId)
        {
            Debug.WriteLine("Attempting to associate coder :" + id + " with programming language " + LanguageId);

            //call our api to associate animal with keeper
            string url = "coderdata/associatecoderwithprogramminglanguage/" + id + "/" + LanguageId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Coder/UnAssociate/{id}?LanguageId={languageid}
        [HttpGet]
        public ActionResult UnAssociate(int id, int LangaugeId)
        {
            Debug.WriteLine("Attempting to unassociate animal :" + id + " with programminglanguage: " + LangaugeId);

            //call our api to associate animal with keeper
            string url = "coderdata/unassociatecoderwithprogramminglanguage/" + id + "/" + LangaugeId;
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
        public ActionResult Create(Coder coder)
        {
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
        public ActionResult Update(int id, Coder coder )
        {
            string url = "coderdata/updatecoder/" + id;
            string jsonpayload = jss.Serialize(coder);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Coder/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "coderdata/findcoder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CoderDto selectedcoder = response.Content.ReadAsAsync<CoderDto>().Result;
            return View(selectedcoder);
        }

        // POST: Coder/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using DevelopersApplication.Models;
using DevelopersApplication.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace DevelopersApplication.Controllers
{
    public class ProgrammingLanguageController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProgrammingLanguageController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44384/api/");
        }
        // GET: ProgrammingLanguage/List
        public ActionResult List()
        {
            //Objective:Communicate with the programming language web api to recieve a list of programming languages
            //curl :https://localhost:44384/api/programminglanguagedata/listprogramminglanguages


            string url = "programminglanguagedata/listprogramminglanguages";

            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The Response status code is ");
            //Debug.WriteLine(responseMessage.StatusCode);

            IEnumerable<ProgrammingLanguageDto> programminglanguages = response.Content.ReadAsAsync<IEnumerable<ProgrammingLanguageDto>>().Result;

            //Debug.WriteLine("The number of programminglanguages in the system: " + programminglanguages.Count());


            return View(programminglanguages);
        }

        // GET: ProgrammingLanguage/Details/5
        public ActionResult Details(int id)
        {
            //Objective:Communicate with the programminglanguage web api to recieve the details of a programminglanguage provided the id 
            //curl :https://localhost:44384/api/programminglanguagedata/findprogramminglanguage{id}
            DetailsProgrammingLanguage ViewModel = new DetailsProgrammingLanguage();

            string url = "programminglanguagedata/findprogramminglanguage/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The Response status code is ");
            Debug.WriteLine(response.StatusCode);

            ProgrammingLanguageDto selectedprogramminglanguage = response.Content.ReadAsAsync<ProgrammingLanguageDto>().Result;

            Debug.WriteLine("programming language received:" + selectedprogramminglanguage.Language);

            ViewModel.SelectedProgrammingLanguage = selectedprogramminglanguage;

            //show all coders who knows this programming language
            url = "coderdata/listcodersforprogramminglanguage/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CoderDto> ExpertCoders = response.Content.ReadAsAsync<IEnumerable<CoderDto>>().Result;

            ViewModel.ExpertCoders = ExpertCoders;

            return View(ViewModel);
        }
        // for error message
        public ActionResult Error()
        {

            return View();
        }


        // GET: ProgrammingLanguage/New
        public ActionResult New()
        {
            return View();
        }

        // POST: ProgrammingLanguage/Create
        [HttpPost]
        public ActionResult Create(ProgrammingLanguage programminglanguage)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(programminglanguage.Language);
            //objective: add a new programming language into our system using the API
            //curl -H "Content-Type:application/json" -d @ProgrammingLanguage.json https://localhost:44384/api/programminglanguagedata/addprogramminglanguage
            string url = "programminglanguagedata/addprogramminglanguage";


            string jsonpayload = jss.Serialize(programminglanguage);

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

        // GET: ProgrammingLanguage/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "programminglanguagedata/findprogramminglanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProgrammingLanguageDto selectedProgrammingLanguage = response.Content.ReadAsAsync<ProgrammingLanguageDto>().Result;
            return View(selectedProgrammingLanguage);
        }

        // POST: ProgrammingLanguage/Update/5
        [HttpPost]
        public ActionResult Update(int id, ProgrammingLanguage programmingLanguage)
        {
            string url = "programminglanguagedata/updateprogramminglanguage/" + id;
            string jsonpayload = jss.Serialize(programmingLanguage);
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

        // GET: ProgrammingLanguage/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "programminglanguagedata/findprogramminglanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProgrammingLanguageDto selectedProgrammingLanguage = response.Content.ReadAsAsync<ProgrammingLanguageDto>().Result;
            return View(selectedProgrammingLanguage);
        }

        // POST: ProgrammingLanguage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "programminglanguagedata/deleteprogramminglanguage/" + id;
            HttpContent content = new StringContent("");
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
    }
}

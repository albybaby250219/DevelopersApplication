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
    public class CareerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CareerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44384/api/");
        }


        // GET: Career/List
        public ActionResult List()
        {
            //Objective:Communicate with the career web api to recieve a list of careers
            //curl :https://localhost:44384/api/careerdata/listcareers


            string url = "careerdata/listcareers";

            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The Response status code is ");
            //Debug.WriteLine(responseMessage.StatusCode);

            IEnumerable<CareerDto> careers = response.Content.ReadAsAsync<IEnumerable<CareerDto>>().Result;

            //Debug.WriteLine("The number of careers in the system: " + careers.Count());


            return View(careers);
        }

        // GET: Career/Details/5
        public ActionResult Details(int id)
        {
            //Objective:Communicate with the career web api to recieve the details of a career provided the id 
            //curl :https://localhost:44384/api/careerdata/findcareer{id}

            DetailsCareer ViewModel = new DetailsCareer();
            string url = "careerdata/findcareer/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The Response status code is ");
            Debug.WriteLine(response.StatusCode);

            CareerDto selectedcareer = response.Content.ReadAsAsync<CareerDto>().Result;

            Debug.WriteLine("Career received:" + selectedcareer.CareerName);

            ViewModel.SelectedCareer = selectedcareer;

            //showcase information about coders related to this career
            //send a request to gather information about coder related to a particular career ID
            url = "coderdata/listcodersforcareer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CoderDto> RelatedCoders = response.Content.ReadAsAsync<IEnumerable<CoderDto>>().Result;

            ViewModel.RelatedCoders = RelatedCoders;


            return View(ViewModel);
        }
        // for error message
        public ActionResult Error()
        {

            return View();
        }
        // GET: Career/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Career/Create
        [HttpPost]
        public ActionResult Create(Career career)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(career.CareerName);
            //objective: add a new career into our system using the API
            //curl -H "Content-Type:application/json" -d @Career.json https://localhost:44384/api/careerdata/addcareer 
            string url = "careerdata/addcareer";


            string jsonpayload = jss.Serialize(career);

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

        // GET: Career/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "careerdata/findcareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto selectedCareer = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(selectedCareer);
        }

        // POST: Career/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Career career)
        {
            string url = "careerdata/updatecareer/" + id;
            string jsonpayload = jss.Serialize(career);
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

        // GET: Career/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "careerdata/findcareer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareerDto selectedCareer = response.Content.ReadAsAsync<CareerDto>().Result;
            return View(selectedCareer);
        }

        // POST: Career/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "careerdata/deletecareer/" + id;
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

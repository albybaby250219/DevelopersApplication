using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DevelopersApplication.Models;
using System.Diagnostics;

namespace DevelopersApplication.Controllers
{
    public class CareerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Careers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Careers in the database, including their associated coders.
        /// </returns>
        /// <example>
        /// GET: api/CareerData/ListCareers
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(CareerDto))]
        public IHttpActionResult ListCareers()
        {

            List<Career> Careers = db.Careers.ToList();
            List<CareerDto> CareerDtos = new List<CareerDto>();
            Careers.ForEach(career => CareerDtos.Add(new CareerDto()
            {
                CareerId = career.CareerId,
                CareerName = career.CareerName,
                CareerDesc = career.CareerDesc

            }));

            return Ok(CareerDtos);
        }

        /// <summary>
        /// Find A career based on the careerid
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Career in the system matching up to the Career ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Careers</param>
        /// <example>
        /// GET: api/CareerData/FindCareer/2
        /// </example>

        [ResponseType(typeof(CareerDto))]
        [HttpGet]
        public IHttpActionResult FindCareer(int id)
        {
            Career Career = db.Careers.Find(id);
            CareerDto CareerDto = new CareerDto()
            {
                CareerId = Career.CareerId,
                CareerName = Career.CareerName,
                CareerDesc = Career.CareerDesc
            };
            if (Career == null)
            {
                return NotFound();
            }

            return Ok(CareerDto);
        }

        /// <summary>
        /// Updates a particular Career in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Career ID primary key</param>
        /// <param name="Career">JSON FORM DATA of a Career</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CareerData/UpdateCareer/3
        /// FORM DATA: Career JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCareer(int id, Career Career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Career.CareerId)
            {
                return BadRequest();
            }

            db.Entry(Career).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CareerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a Career to the system
        /// </summary>
        /// <param name="Career">JSON FORM DATA of a Career</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Career Id, Career Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CareerData/AddCareer
        /// FORM DATA: Career JSON Object
        /// </example>
       
        [ResponseType(typeof(Career))]
        [HttpPost]
        public IHttpActionResult AddCareer(Career Career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Careers.Add(Career);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Career.CareerId }, Career);
        }

        /// <summary>
        /// Deletes a Career from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Career</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CareerData/DeleteCareer/2
        /// FORM DATA: (empty)
        /// </example>
      
        [HttpPost]
        [ResponseType(typeof(Career))]
        public IHttpActionResult DeleteCareer(int id)
        {
            Career Career = db.Careers.Find(id);
            if (Career == null)
            {
                return NotFound();
            }

            db.Careers.Remove(Career);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CareerExists(int id)
        {
            return db.Careers.Count(e => e.CareerId == id) > 0;
        }
    }
}
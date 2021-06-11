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
    public class CoderDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all the coders in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all coders in the database, including their associated career.
        /// </returns>
        /// <example
        //  GET: api/CoderData/ListCoders
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CoderDto))]
        public IHttpActionResult ListCoders()
        {
            List<Coder>Coders =  db.Coders.ToList();
            List<CoderDto> CoderDtos = new List<CoderDto>();
            Coders.ForEach(c => CoderDtos.Add(new CoderDto(){
                CoderId = c.CoderId,
                Name = c.Name,
                Bio = c.Bio,
                Company = c.Company,
                CareerId = c.Careers.CareerId,
                CareerName = c.Careers.CareerName

            }));

            return Ok(CoderDtos);
        }

        /// <summary>
        /// Gathers information about all coders related to a particular career Id
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all coders in the database, including their associated career matched with a particular career Id
        /// </returns>
        /// <param name="id">Career Id.</param>
        /// <example>
        /// GET: api/CoderData/ListCodersForCareer/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CoderDto))]
        public IHttpActionResult ListCodersForCareer(int id)
        {
            List<Coder> Coders = db.Coders.Where(c => c.CareerId == id).ToList();
            List<CoderDto> CoderDtos = new List<CoderDto>();

            Coders.ForEach(c => CoderDtos.Add(new CoderDto()
            {
                CoderId = c.CoderId,
                Name = c.Name,
                Bio = c.Bio,
                Company = c.Company,
                CareerId = c.Careers.CareerId,
                CareerName = c.Careers.CareerName
            }));

            return Ok(CoderDtos);
        }

        /// <summary>
        /// Gathers information about coders related to a particular programming language
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all coders in the database, including their associated programming languges that match to a particular langauge id
        /// </returns>
        /// <param name="id">Language Id.</param>
        /// <example>
        /// GET: api/CoderData/ListCodersForProgrammingLanguage/2
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CoderDto))]
        public IHttpActionResult ListCodersForProgrammingLanguage(int id)
        {
            //all coderss that have programming language which match with our ID
            List<Coder> Coders = db.Coders.Where(
                c => c.ProgrammingLanguages.Any(
                    p => p.LanguageId == id
                )).ToList();
            List<CoderDto> CoderDtos = new List<CoderDto>();

            Coders.ForEach(c => CoderDtos.Add(new CoderDto()
            {
                CoderId = c.CoderId,
                Name = c.Name,
                Bio = c.Bio,
                Company = c.Company,
                CareerId = c.Careers.CareerId,
                CareerName = c.Careers.CareerName
            }));

            return Ok(CoderDtos);
        }


        /// <summary>
        /// Associates a particular programming language with a particular coder
        /// </summary>
        /// <param name="coderid">The Coder Id primary key</param>
        /// <param name="languageid">The Language Id primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/CoderData/AssociateCoderWithProgrammingLanguage/3/2
        /// </example>
        [HttpPost]
        [Route("api/CoderData/AssociateCoderWithProgrammingLanguage/{coderid}/{languageid}")]
        public IHttpActionResult AssociateCoderWithProgrammingLanguage(int coderid, int languageid)
        {

            Coder SelectedCoder = db.Coders.Include(c => c.ProgrammingLanguages).Where(c => c.CoderId == coderid).FirstOrDefault();
            ProgrammingLanguage SelectedProgrammingLanguage = db.ProgrammingLanguages.Find(languageid);

            if (SelectedCoder == null || SelectedProgrammingLanguage == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input coder id is: " + coderid);
            Debug.WriteLine("selected coder name is: " + SelectedCoder.Name);
            Debug.WriteLine("input language id is: " + languageid);
            Debug.WriteLine("selected programming language  is: " + SelectedProgrammingLanguage.Language);


            SelectedCoder.ProgrammingLanguages.Add(SelectedProgrammingLanguage);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular coder and a particular programming language
        /// </summary>
        /// <param name="coderid">The coder ID primary key</param>
        /// <param name="languageid">The programming language ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/CoderData/UnAssociateCoderWithProgrammingLanguage/3/1
        /// </example>
        [HttpPost]
        [Route("api/CoderData/UnAssociateCoderWithProgrammingLanguage/{coderid}/{languageid}")]
        public IHttpActionResult UnAssociateCoderWithProgrammingLanguage(int coderid, int languageid)
        {

            Coder SelectedCoder = db.Coders.Include(c => c.ProgrammingLanguages).Where(c => c.CoderId == coderid).FirstOrDefault();
            ProgrammingLanguage SelectedProgrammingLanguage = db.ProgrammingLanguages.Find(languageid);

            if (SelectedCoder == null || SelectedProgrammingLanguage == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input coder id is: " + coderid);
            Debug.WriteLine("selected coder name is: " + SelectedCoder.Name);
            Debug.WriteLine("input language id is: " + languageid);
            Debug.WriteLine("selected programming language  is: " + SelectedProgrammingLanguage.Language);


            SelectedCoder.ProgrammingLanguages.Remove(SelectedProgrammingLanguage);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Returns coder details of a particular coder in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A coder in the system matching up to the coder ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the coder</param>
        /// <example>
        /// GET: api/CoderData/Findcoder/2
        /// </example>

        [ResponseType(typeof(Coder))]
        [HttpGet]
        public IHttpActionResult FindCoder(int id)
        {
            Coder Coder = db.Coders.Find(id);
            CoderDto CoderDto = new CoderDto()
            {
                CoderId = Coder.CoderId,
                Name = Coder.Name,
                Bio = Coder.Bio,
                Company = Coder.Company,
                CareerId = Coder.CareerId,
                CareerName =Coder.Careers.CareerName
            };
            if (Coder == null)
            {
                return NotFound();
            }

            return Ok(CoderDto);
        }

        /// <summary>
        /// Updates a particular coder in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the coder ID primary key</param>
        /// <param name="Coder">JSON FORM DATA of a coder</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CoderData/UpdateCoder/3
        /// FORM DATA: Coder JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCoder(int id, Coder Coder)
        {
            Debug.WriteLine("i AM IN UPDATE METOD");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Coder.CoderId)
            {
                Debug.WriteLine("POST Parameter Coderid:" + Coder.CoderId);
                return BadRequest();
            }

            db.Entry(Coder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("SUCESS");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a coder to the system
        /// </summary>
        /// <param name="Coder">JSON FORM DATA of a coder</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Coder Id, Coder Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CoderData/AddCoder
        /// FORM DATA: Coder JSON Object
        /// </example>
 
        [ResponseType(typeof(Coder))]
        [HttpPost]
        public IHttpActionResult AddCoder(Coder Coder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Coders.Add(Coder);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Coder.CoderId }, Coder);
        }

        /// <summary>
        /// Deletes a coder from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the coder</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CoderData/DeleteCoder/5
        /// FORM DATA: (empty)
        /// </example>
    
        [ResponseType(typeof(Coder))]
        [HttpPost]
        public IHttpActionResult DeleteCoder(int id)
        {
            Coder Coder = db.Coders.Find(id);
            if (Coder == null)
            {
                return NotFound();
            }

            db.Coders.Remove(Coder);
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

        private bool CoderExists(int id)
        {
            return db.Coders.Count(e => e.CoderId == id) > 0;
        }
    }
}
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
    public class ProgrammingLanguageDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Programming language in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all programminglanguage in the database, including their associated careers.
        /// </returns>
        /// <example>
        /// GET: api/ProgrammingLanguageData/ListProgrammingLanguages
        /// </example>
        [ResponseType(typeof(ProgrammingLanguageDto))]
        [HttpGet]
        public IHttpActionResult ListProgrammingLanguages()
        {
            List<ProgrammingLanguage> ProgrammingLanguages = db.ProgrammingLanguages.ToList();
            List<ProgrammingLanguageDto> ProgrammingLanguageDtos = new List<ProgrammingLanguageDto>();
            ProgrammingLanguages.ForEach(p => ProgrammingLanguageDtos.Add(new ProgrammingLanguageDto()
            {
                LanguageId = p.LanguageId,
                Language = p.Language,
                LanguageInfo =p.LanguageInfo,
                IDEUsed = p.IDEUsed

            }));
            return Ok(ProgrammingLanguageDtos);
        }

        /// <summary>
        /// Returns all programming language in the system associated with a particular coder.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all programming languages in the database known by  a particular coder
        /// </returns>
        /// <param name="id">Coder Primary Key</param>
        /// <example>
        /// GET: api/ProgrammingLanguageData/ListProgrammingLanguagesForCoder/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProgrammingLanguageDto))]
        public IHttpActionResult ListProgrammingLanguagesForCoder(int id)
        {
            List<ProgrammingLanguage> ProgrammingLanguages = db.ProgrammingLanguages.Where(
                p => p.Coders.Any(
                    c => c.CoderId == id)
                ).ToList();
            List<ProgrammingLanguageDto> ProgrammingLanguageDtos = new List<ProgrammingLanguageDto>();

            ProgrammingLanguages.ForEach(p => ProgrammingLanguageDtos.Add(new ProgrammingLanguageDto()
            {
                LanguageId = p.LanguageId,
                Language = p.Language,
                LanguageInfo = p.LanguageInfo,
                IDEUsed = p.IDEUsed
            }));

            return Ok(ProgrammingLanguageDtos);
        }


        /// <summary>
        /// Returns programming languages in the system available to master for a particular coder.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all programming languages in the database not associated with a particular coder
        /// </returns>
        /// <param name="id">Coder Primary Key</param>
        /// <example>
        /// GET: api/ProgrammingLanguageData/ListProgrammingLanguagesNotAssociatedWithCoder/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProgrammingLanguageDto))]
        public IHttpActionResult ListProgrammingLanguagesNotAssociatedWithCoder(int id)
        {
            List<ProgrammingLanguage> ProgrammingLanguages = db.ProgrammingLanguages.Where(
                p => !p.Coders.Any(
                    c => c.CoderId == id)
                ).ToList();
            List<ProgrammingLanguageDto> ProgrammingLanguageDtos = new List<ProgrammingLanguageDto>();

            ProgrammingLanguages.ForEach(p => ProgrammingLanguageDtos.Add(new ProgrammingLanguageDto()
            {
                LanguageId = p.LanguageId,
                Language = p.Language,
                LanguageInfo = p.LanguageInfo,
                IDEUsed = p.IDEUsed
            }));

            return Ok(ProgrammingLanguageDtos);
        }

        /// <summary>
        /// Find a particular programming language using the  id
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A programming language in the system matching up to the language ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the programminglanguages</param>
        /// <example>
        /// GET: api/ProgrammingLanguageData/findprogramminglanguage/2
        /// </example>
       
        [HttpGet]
        [ResponseType(typeof(ProgrammingLanguageDto))]
        public IHttpActionResult FindProgrammingLanguage(int id)
        {
            ProgrammingLanguage ProgrammingLanguage = db.ProgrammingLanguages.Find(id);
            ProgrammingLanguageDto ProgrammingLanguageDto = new ProgrammingLanguageDto()
            {
                LanguageId = ProgrammingLanguage.LanguageId,
                Language = ProgrammingLanguage.Language,
                LanguageInfo = ProgrammingLanguage.LanguageInfo,
                IDEUsed = ProgrammingLanguage.IDEUsed
            };
            if (ProgrammingLanguage == null)
            {
                return NotFound();
            }

            return Ok(ProgrammingLanguageDto);
        }

        /// <summary>
        /// Updates a particular programming language in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the langauge ID primary key</param>
        /// <param name="ProgrammingLanguage">JSON FORM DATA of a programming language</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        ///  POST: api/ProgrammingLanguageData/updateprogramminglanguage/3
        /// FORM DATA: programminglanguage JSON Object
        /// </example>
        //
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateProgrammingLanguage(int id, ProgrammingLanguage ProgrammingLanguage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ProgrammingLanguage.LanguageId)
            {
                return BadRequest();
            }

            db.Entry(ProgrammingLanguage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgrammingLanguageExists(id))
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
        /// Adds a programming language to the system
        /// </summary>
        /// <param name="ProgrammingLanguage">JSON FORM DATA of a PROGRAMMING LANGUAGE</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Language ID, Programming Language Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ProgrammingLanguageData/addprogramminglanguage
        /// FORM DATA: programming language JSON Object
        /// </example>
        // 
        [ResponseType(typeof(ProgrammingLanguage))]
        [HttpPost]
        public IHttpActionResult AddProgrammingLanguage(ProgrammingLanguage ProgrammingLanguage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProgrammingLanguages.Add(ProgrammingLanguage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ProgrammingLanguage.LanguageId }, ProgrammingLanguage);
        }

        /// <summary>
        /// Deletes a programming language from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the programming language</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ProgrammingLanguageData/DeleteProgrammingLanguage/2
        /// FORM DATA: (empty)
        /// </example>
        
        [ResponseType(typeof(ProgrammingLanguage))]
        [HttpPost]
        public IHttpActionResult DeleteProgrammingLanguage(int id)
        {
            ProgrammingLanguage ProgrammingLanguage = db.ProgrammingLanguages.Find(id);
            if (ProgrammingLanguage == null)
            {
                return NotFound();
            }

            db.ProgrammingLanguages.Remove(ProgrammingLanguage);
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

        private bool ProgrammingLanguageExists(int id)
        {
            return db.ProgrammingLanguages.Count(e => e.LanguageId == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DevelopersApplication.Models
{
    public class Coder

    {
        [Key]
        public int CoderId { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }
        public string Company { get; set; }

       
        //data needed for keeping track of profile pic uploaded
        //images deposited into /Content/Images/Coders/{id}.{extension}
        public bool CoderHasPic { get; set; }
        public string PicExtension { get; set; }

        // A coder can have a career
        // a career can have many coders

        [ForeignKey("Careers")]
        public int CareerId { get; set; }
        public virtual Career Careers { get; set; }

        //A coder may know many programming languages
        public ICollection<ProgrammingLanguage> ProgrammingLanguages { get; set; }
    }
    public class CoderDto
    {
        public int CoderId { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public string Company { get; set; }
        public bool CoderHasPic { get; set; }
        public string PicExtension { get; set; }

        public string CareerName { get; set; }

        public int CareerId { get; set; }

    }
}
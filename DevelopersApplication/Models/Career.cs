using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DevelopersApplication.Models
{
    public class Career
    {
        [Key]
        public int CareerId { get; set; }
        public string CareerName { get; set; }

        public string CareerDesc { get; set; }

        //A career may require  multiple programming languages

        public ICollection<ProgrammingLanguage> ProgrammingLanguages { get; set; }

    }
    public class CareerDto
    {
        public int CareerId { get; set; }

        public string CareerName { get; set; }

        public string CareerDesc { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DevelopersApplication.Models
{
    public class ProgrammingLanguage
    {
        [Key]
        public int LanguageId { get; set; }

        public string Language { get; set; }
        public string LanguageInfo { get; set; }
        public string IDEUsed { get; set; }

        //A programming language has many coders

        public ICollection<Coder> Coders { get; set; }

        //A programming language may be used by many careers

        public ICollection<Career> Careers { get; set; }


    }
    public class ProgrammingLanguageDto
    {
        public int LanguageId { get; set; }

        public string Language { get; set; }

        public string LanguageInfo { get; set; }

        public string IDEUsed { get; set; }

    }
}
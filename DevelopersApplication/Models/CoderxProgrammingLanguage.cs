using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevelopersApplication.Models
{
    public class CoderxProgrammingLanguage
    {
        [Key]
        public int CoderxPLId { get; set; }
        [ForeignKey("Coder")]
        public int CoderId { get; set; }
        public virtual Coder Coder { get; set; }
        [ForeignKey("ProgrammingLanguage")]
        public int LanguageId { get; set; }
        public virtual ProgrammingLanguage ProgrammingLanguage { get; set; }
        public Boolean FavLang { get; set; }
    }

    public class CoderxPLDto
    {
        public int CoderId { get; set; }
        public int LanguageId { get; set; }
        public Boolean FavLang { get; set; }
        public string Language { get; set; }
    }
}
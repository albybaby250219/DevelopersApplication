using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevelopersApplication.Models.ViewModels
{
    public class DetailsCoder
    {
      
        public CoderDto SelectedCoder { get; set; }
        public IEnumerable<ProgrammingLanguageDto> ProgrammingLanguages { get; set; }

        public IEnumerable<ProgrammingLanguageDto> AvailableProgrammingLanguages { get; set; }

        public IEnumerable<CoderxPLDto> FavLanguages { get; set; }
    }
}


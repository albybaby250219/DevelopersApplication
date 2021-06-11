using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevelopersApplication.Models.ViewModels
{
    public class DetailsProgrammingLanguage
    {
        public ProgrammingLanguageDto SelectedProgrammingLanguage { get; set; }
        public IEnumerable<CoderDto> ExpertCoders { get; set; }
    }
}
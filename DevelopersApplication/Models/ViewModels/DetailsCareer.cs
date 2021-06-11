using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevelopersApplication.Models.ViewModels
{
    public class DetailsCareer
    {
        //the career itself that we want to display
        public CareerDto SelectedCareer { get; set; }

        //all of the related coders to that particular career
        public IEnumerable<CoderDto> RelatedCoders { get; set; }
    }
}
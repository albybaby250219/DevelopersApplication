using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevelopersApplication.Models.ViewModels
{
    public class UpdateCoder
    {
        //This viewmodel is a class which stores information that we need to present to /Coder/Update/{}

        //the existing coder information

        public CoderDto SelectedCoder { get; set; }

        // all species to choose from when updating this animal

        public IEnumerable<CareerDto> CareerOptions { get; set; }
    }
}
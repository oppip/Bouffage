using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Models;

namespace Bouffage.ViewModels
{
    public class SingleRecipeViewModel
    {
        public string SearchString { get; set; }

        public Recipe Recipe { get; set; }
    }
}

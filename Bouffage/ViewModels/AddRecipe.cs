using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bouffage.ViewModels
{
    public class AddRecipe
    {
        public Recipe Recipe { get; set; }

        public IEnumerable<int> SelectedCategories { get; set; } 
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public int NumberOfIngredients { get; set; }

        public IList<Ingredient> Ingredients { get; set; }


    }
}

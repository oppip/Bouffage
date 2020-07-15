using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Models;

namespace Bouffage.ViewModels
{
    public class RecipeAndCommentsViewModel
    {
        public string SearchString { get; set; }

        public IList<Recipe> Recipes { get; set; }

        public IList<Comment> Comments { get; set; }
         
        public IList<User> Users { get; set; }

        public IList<Ingredient> Ingredients { get; set; }
    }
}

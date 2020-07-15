using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Models;

namespace Bouffage.ViewModels
{
    public class UserRecipesViewModel
    {
        public string SearchString { get; set; }

        public IList<Recipe> Recipes { get; set; }

        public IList<Comment> Comments { get; set; }

        public IList<Ingredient> Ingredients { get; set; }

        public User User { get; set; }
    }
}

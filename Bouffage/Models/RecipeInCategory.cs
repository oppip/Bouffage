using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.Models
{
    public class RecipeInCategory
    {
        [Key]
        public int RecipeInCategoryId { get; set; }

        public int RecipeId { get;set; } 

        public Recipe Recipe { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}

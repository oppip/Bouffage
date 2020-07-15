using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        [Required]
        [StringLength(70)]
        public string IngredientUsed { get; set; } //what is used

        public int? RecipeId { set; get; }  //Which recipe has this ingredient

        public Recipe Recipe { get; set; }


    }
}

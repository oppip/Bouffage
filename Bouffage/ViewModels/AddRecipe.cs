using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bouffage.ViewModels
{
    public class AddRecipe
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [DisplayName("Is there a story behind this recipe?")]
        [Required]
        [StringLength(700)]
        public string Essay { get; set; }
        [DisplayName("What type of food is it")]
        [Required]
        public Recipe.CuisineFood CuisineFood { get; set; }
        [DisplayName("How easy is it to make")]
        [Required]
        public Recipe.Complex Complex { get; set; }
        [Required]
        public string Preparation { get; set; }
        [DisplayName("How much preparation is needed")]
        [Required]
        public int PrepTime { get; set; }
        [DisplayName("How long is the cooking process")]
        [Required]
        public int CookTime { get; set; }
        [DisplayName("How many dishes does the recipe yield")]
        [Required]
        public int Servings { get; set; }
        [DisplayName("List all the special equipment used")]
        public string SpecialEquipment { get; set; }
        [Required(ErrorMessage = "Please upload images of the case!")]
        [Display(Name = "Слика од јадењето")]
        public IFormFile UploadPicture { get; set; }
        public IList<int> SelectedCategories { get; set; } 
        public SelectList Categories { get; set; }
        public string[] Ingredients { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.Models
{
    public class Recipe
    {

        [Key]
        public int RecipeId { get; set; }   //The primary key for the Recipe table

        [Required]
        [StringLength(50)]
        public string Title { get; set; }   //Title of the recipe

        public enum CuisineFood
        {
            Chinese,
            Mexican,
            Italian,
            Japanese,
            French,
            Greek,
            Thai,
            Spanish,
            Indian,
            Mediterranean,
            Vietnamese,
            Cuban,
            American,
            Taiwanese,
            Indonesian,
            Moroccan,
            Lebanese,
            Brazilian,
            Swedish,
            Argentinian,
            Danish,
            Estonian,
            Portuguese,
            Korean,
            German,
            Filipino,
            Peruvian,
            Cajun,
            Pakistani,
            Macedonian,
            Other
        }       //	What type of cooking is it

        [Required]
        public virtual int CuisineId
        {
            get
            {
                return (int)this.Cuisine;
            }
            set
            {
                Cuisine = (CuisineFood)value;
            }
        }
        [EnumDataType(typeof(CuisineFood))]
        public CuisineFood Cuisine { get; set; }

        /*[DataType(DataType.MultilineText)]*/
        [StringLength(700)]
        public string Essay { get; set; }       //Wall of text explaining the origin of the recipe and how good it really is and how it goes great in *insert season* or with wine and on and on and on...

        public string Preparation { get; set; }     //how does one do the deed
       
        public int PrepTime { get; set; }

        public int CookTime { get; set; }

        public int Servings { get; set; }

        public enum Complex     //How hard is it to make
        {
            Easy,
            Medium,
            Hard
        }

        [Required]
        public virtual int ComplexityId
        {
            get
            {
                return (int)this.Complexity;
            }
            set
            {
                Complexity = (Complex)value;
            }
        }
        [EnumDataType(typeof(Complex))]
        public Complex Complexity { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public DateTime? PostingDate { get; set; }

        public string Picture { get; set; } //picture of the food

        [StringLength(100)]
        public string SpecialEquipment { get; set; }      //Is any special equipment needed for making this recipe

        public int UserPostedRecipeId { set; get; }   //which user posted the recipe

        public User User { get; set; }

        public ICollection<Comment> Users { get; set; } //All the users that have commented on the recipe

        public ICollection<RecipeInCategory> Categories { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public ICollection<Comment> CommentsOnThisRecipe { get; set; }

        public ICollection<VoteRecipe> UsersThatVotedRecipe { get; set; }

    }
}

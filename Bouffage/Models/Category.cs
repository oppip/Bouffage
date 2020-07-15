using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public enum Tags    //What does the food consist of or what kind of food is it
        {
            Pasta,
            Rice,
            Vegetarian,
            Vegan
        }

        [Required]
        public virtual int TypeId
        {
            get
            {
                return (int)this.Tag;
            }
            set
            {
                Tag = (Tags)value;
            }
        }
        [EnumDataType(typeof(Tags))] 
        public Tags Tag { get; set; }

        public ICollection<RecipeInCategory> RecipesInThisCategory { get; set; }

    }
}

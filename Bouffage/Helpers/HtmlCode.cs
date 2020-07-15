using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Models;

namespace Bouffage.Helpers
{
    public class HtmlCode
    {
        public string CardRecipeAndComments(Recipe recipe)
        {
            var searchtag = "<span id = ' " + recipe.RecipeId.ToString() + "' > </span>";
            Console.WriteLine(searchtag);
            var html1 = @"  

<div id = 'recipe-card'>
<p class='title'>" + recipe.Title + @"</p>


</div>                                                        
                        ";

            return searchtag + html1;

        }
    }
}

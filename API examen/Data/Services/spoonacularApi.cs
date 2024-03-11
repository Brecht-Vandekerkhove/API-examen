using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_examen.Data.Services
{
    internal class spoonacularApi
    {
        public List<Recipe> Recipes { get; set; }

        public class Recipe
        {
            public string Title { get; set; }
            public string Instructions { get; set; }
            public List<Ingredient> ExtendedIngredients { get; set; }
        }

        public class Ingredient
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Amount { get; set; }
            public string Unit { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_examen.Data.Services
{
    internal class spoonacularApi
    {
        //public List<Recipe> Recipes { get; set; }

        //public class Recipe
        //{
        //    public string Title { get; set; }
        //    public string Instructions { get; set; }
        //    public List<Ingredient> ExtendedIngredients { get; set; }
        //}

        //public class Ingredient
        //{
        //    public string Id { get; set; }
        //    public string Name { get; set; }
        //    public string Amount { get; set; }
        //    public string Unit { get; set; }
        //}

        [JsonPropertyName("results")]
        public List<Recipe> Recipes { get; set; }

        public class Recipe
        {
            [JsonPropertyName("id")]
            public int Id { get; set; } // Zorg ervoor da de type matcht met het id-format (int?)

            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("instructions")]
            public string Instructions { get; set; }

            [JsonPropertyName("extendedIngredients")]
            public List<Ingredient> ExtendedIngredients { get; set; }
        }

        public class Ingredient
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("amount")]
            public double Amount { get; set; } // amount double?

            [JsonPropertyName("unit")]
            public string Unit { get; set; }
        }
    }
}

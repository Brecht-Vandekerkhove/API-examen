using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_examen.Data.Services
{
    internal class DetailedRecipeResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("instructions")]
        public string Instructions { get; set; }

        [JsonPropertyName("extendedIngredients")]
        public List<Ingredient> ExtendedIngredients { get; set; }

        public class Ingredient
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("amount")]
            public double Amount { get; set; }

            [JsonPropertyName("unit")]
            public string Unit { get; set; }
        }
    }
}

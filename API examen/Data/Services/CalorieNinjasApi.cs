using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_examen
{
    class CalorieNinjasApi
    {
        [JsonPropertyName("items")]
        public List<NutritieItems> Items { get; set; }

        public class NutritieItems
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("calories")]
            public double Calories { get; set; }

            [JsonPropertyName("serving_size_g")]
            public double ServingSizeG { get; set; }

            [JsonPropertyName("fat_total_g")]
            public double FatTotalG { get; set; }

            [JsonPropertyName("fat_saturated_g")]
            public double FatSaturatedG { get; set; }

            [JsonPropertyName("protein_g")]
            public double ProteinG { get; set; }

            [JsonPropertyName("sodium_mg")]
            public double SodiumMg { get; set; }

            [JsonPropertyName("potassium_mg")]
            public double PotassiumMg { get; set; }

            [JsonPropertyName("cholesterol_mg")]
            public double CholesterolMg { get; set; }

            [JsonPropertyName("carbohydrates_total_g")]
            public double CarbohydratesTotalG { get; set; }

            [JsonPropertyName("fiber_g")]
            public double FiberG { get; set; }

            [JsonPropertyName("sugar_g")]
            public double SugarG { get; set; }
        }

        //Sample response:
        //        {
        //          "items": [
        //          {
        //              "name": "almond milk",
        //              "calories": 54.8,
        //              "serving_size_g": 248.8,
        //              "fat_total_g": 2.5,
        //              "fat_saturated_g": 0.2,
        //              "protein_g": 1.1,
        //              "sodium_mg": 9,
        //              "potassium_mg": 24,
        //              "cholesterol_mg": 0,
        //              "carbohydrates_total_g": 8.1,
        //              "fiber_g": 0.6,
        //              "sugar_g": 7.1
        //          }
        //                  ]
        //      }
    }
}

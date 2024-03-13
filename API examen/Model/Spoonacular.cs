using API_examen.Data.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using static API_examen.Data.Services.spoonacularApi;

namespace API_examen.Model
{

    internal class Spoonacular
    {
        //public void recept(string zoek)
        //{
        //    var test = GetData("random recept", zoek);
        //}

        string complexpartUrl = "";

        public async Task<(List<string> Ingredients, string Recipe)> ComplexSearchAsync(string zoek, bool isVegan, bool geenLactose, bool geenGluten, bool geenVis)
        {
            complexpartUrl = "";

            if (isVegan)
                complexpartUrl += "&diet=vegan";

            string intolerances = "";
            if (geenLactose)
                intolerances += "dairy,";
            if (geenGluten)
                intolerances += "gluten,";
            if (geenVis)
                intolerances += "seafood,";

            if (intolerances != "")
            {
                intolerances = intolerances.TrimEnd(','); // Remove the last comma
                complexpartUrl += "&intolerances=" + intolerances;
            }

            Debug.WriteLine($"Search string: {zoek}, bools {isVegan} {geenLactose} {geenGluten} {geenVis}");
            Debug.WriteLine($"complexpartUrl: {complexpartUrl}");

            // Perform the complex search
            spoonacularApi complexResult = await GetData("complex", zoek);

            List<string> ingredients = new List<string>();
            string recipeDescription = string.Empty;


            Debug.WriteLine($"Begin ComplexSearchAsync if-else structure");
            if (complexResult != null && complexResult.Recipes != null && complexResult.Recipes.Any())
            {
                Debug.WriteLine($"Begint de eerste if-route");

                // This example assumes you are interested in details for the first recipe
                var firstRecipeId = complexResult.Recipes.First().Id;
                Debug.WriteLine($"ReceptID: {firstRecipeId}");
                DetailedRecipeResponse detailedRecipe = await GetRecipeDetailsAsync(firstRecipeId);

                if (detailedRecipe != null)
                {
                    Debug.WriteLine($"Begint de tweede if-route");

                    ingredients.AddRange(detailedRecipe.ExtendedIngredients.Select(i => $"{i.Name} - {i.Amount} {i.Unit}"));
                    recipeDescription = $"{detailedRecipe.Title}\n\nInstructions:\n{detailedRecipe.Instructions}";
                }
                else
                {
                    Debug.WriteLine($"Tweede else-route");
                }
            }
            else
            {
                Debug.WriteLine($"Eerste else-route");

                MessageBox.Show($"Geen recipe found.\r\nChange the input!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                ingredients = new List<string> { "No ingredients found." };
                recipeDescription = "No recipe found.";
            }

            return (ingredients, recipeDescription);
        }

        public async Task<DetailedRecipeResponse> GetRecipeDetailsAsync(int recipeId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.spoonacular.com/recipes/{recipeId}/information?apiKey={ApiKey2}";
                HttpResponseMessage response = await client.GetAsync(url);
                Debug.WriteLine($"Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"JSON Response: {json}");

                    var detailedRecipe = JsonSerializer.Deserialize<DetailedRecipeResponse>(json);

                    return detailedRecipe;
                }
                else
                {
                    Debug.WriteLine($"Error: {response.ReasonPhrase}");
                }
            }
            return null;
        }


        private const string ApiKey = "1c312f036b1c45e7b5a00ec292622ca3";
        private const string ApiKey2 = "355943b82e6c444a8803d0b07ac85a84";

        public async Task<spoonacularApi> GetData(string type, string zoekopdracht)
        {
            try
            {
                //HttpClient client = new HttpClient();
                using (HttpClient client = new HttpClient())
                {
                    string urlApi = string.Empty;

                    switch (type)
                    {
                        case "random recept":
                            urlApi = $"https://api.spoonacular.com/recipes/random?apiKey={ApiKey}&tags={zoekopdracht}&number=3";
                            break;
                        case "complex":
                            urlApi = $"https://api.spoonacular.com/recipes/complexSearch?query={zoekopdracht}{complexpartUrl}&apiKey={ApiKey}";
                            break;
                        default:
                            // If the type is not recognized, return null
                            return null;
                    }

                    Debug.WriteLine(urlApi);
                    HttpResponseMessage response = await client.GetAsync(urlApi);


                    if (response.IsSuccessStatusCode)
                    {
                        // Lees api response
                        var json = await response.Content.ReadAsStringAsync();
                        var SpoonacularApi = JsonSerializer.Deserialize<spoonacularApi>(json);

                        // Als alles goed gaat, genereer output
                        //GenerateOutput(SpoonacularApi);
                        return SpoonacularApi;
                    }
                    //Als de API geen overeenkomstig recept vindt,
                    //retourneert het een 404 (Not Found) HTTP-statuscode.
                    //In dit geval zou EnsureSuccessStatusCode() een HttpRequestException veroorzaken.
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Recept niet gevonden
                        Console.WriteLine("Recept niet gevonden.");
                    }
                    else
                    {
                        // Andere fout bij het ophalen van gegevens, toon foutmelding en voer reset uit
                        Console.WriteLine($"Fout: {response.ReasonPhrase}");
                        MessageBox.Show($"Fout: {response.ReasonPhrase}", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Fout: kon geen verbinding maken met api.");
                MessageBox.Show("Fout: kon geen verbinding maken met api.", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                Console.WriteLine("Fout: unkown error.");
                MessageBox.Show("Fout: unkown error.", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }
    }
}

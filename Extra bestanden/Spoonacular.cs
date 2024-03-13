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
    //internal class Spoonacular
    //{
    //    private const string ApiKey = "1c312f036b1c45e7b5a00ec292622ca3";

    //    public async Task<(List<string> Ingredients, string Recipe)> ComplexSearchAsync(string zoek, bool isVegan, bool geenLactose, bool geenGluten, bool geenVis)
    //    {
    //        string complexpartUrl = "";

    //        if (isVegan) complexpartUrl += "&diet=vegan";
    //        string intolerances = "";
    //        if (geenLactose) intolerances += "dairy,";
    //        if (geenGluten) intolerances += "gluten,";
    //        if (geenVis) intolerances += "seafood,";

    //        if (intolerances != "")
    //        {
    //            intolerances = intolerances.TrimEnd(',');
    //            complexpartUrl += "&intolerances=" + intolerances;
    //        }

    //        spoonacularApi complexResult = await GetData("complex", zoek, complexpartUrl);

    //        List<string> ingredients = new List<string>();
    //        string recipe = string.Empty;

    //        if (complexResult != null && complexResult.Results != null && complexResult.Results.Any())
    //        {
    //            // Fetch detailed information for the first result
    //            var firstRecipeId = complexResult.Results.First().Id;
    //            var detailedRecipe = await GetRecipeDetailsAsync(firstRecipeId);

    //            if (detailedRecipe != null && detailedRecipe.Recipes != null && detailedRecipe.Recipes.Any())
    //            {
    //                var recipeDetails = detailedRecipe.Recipes.First();
    //                ingredients.AddRange(recipeDetails.ExtendedIngredients.Select(i => $"{i.Name} - {i.Amount} {i.Unit}"));
    //                recipe = $"{recipeDetails.Title}\n\nInstructions:\n{recipeDetails.Instructions}";
    //            }
    //        }
    //        else
    //        {
    //            ingredients = new List<string> { "No ingredients found." };
    //            recipe = "No recipe found.";
    //        }

    //        return (ingredients, recipe);
    //    }

    //    private async Task<spoonacularApi> GetData(string type, string zoekopdracht, string complexpartUrl = "")
    //    {
    //        using (HttpClient client = new HttpClient())
    //        {
    //            string url = type switch
    //            {
    //                "random recept" => $"https://api.spoonacular.com/recipes/random?apiKey={ApiKey}&tags={zoekopdracht}&number=3",
    //                "complex" => $"https://api.spoonacular.com/recipes/complexSearch?query={zoekopdracht}{complexpartUrl}&apiKey={ApiKey}",
    //                _ => null,
    //            };

    //            if (url == null) return null;

    //            HttpResponseMessage response = await client.GetAsync(url);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var json = await response.Content.ReadAsStringAsync();
    //                return JsonSerializer.Deserialize<spoonacularApi>(json);
    //            }
    //        }
    //        return null;
    //    }

    //    public async Task<spoonacularApi> GetRecipeDetailsAsync(int recipeId)
    //    {
    //        using (HttpClient client = new HttpClient())
    //        {
    //            string url = $"https://api.spoonacular.com/recipes/{recipeId}/information?apiKey={ApiKey}";
    //            HttpResponseMessage response = await client.GetAsync(url);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                string json = await response.Content.ReadAsStringAsync();
    //                return JsonSerializer.Deserialize<spoonacularApi>(json);
    //            }
    //        }
    //        return null;
    //    }
    //}

    internal class Spoonacular
    {
        //public void recept(string zoek)
        //{
        //    var test = GetData("random recept", zoek);
        //}

        string complexpartUrl = "";
        //public async Task<(List<string> Ingredients, string Recipe)> ComplexSearchAsync(string zoek, bool isVegan, bool geenLactose, bool geenGluten, bool geenVis)
        //{
        //    complexpartUrl = "";

        //    if (isVegan)
        //        complexpartUrl += "&diet=vegan";

        //    string intolerances = "";
        //    if (geenLactose)
        //        intolerances += "dairy,";
        //    if (geenGluten)
        //        intolerances += "gluten,";
        //    if (geenVis)
        //        intolerances += "seafood,";

        //    if (intolerances != "")
        //    {
        //        intolerances = intolerances.TrimEnd(','); //Laatste komma weghalen
        //        complexpartUrl += "&intolerances=" + intolerances;
        //    }

        //    Debug.WriteLine($"Zoek string: {zoek}, bools {isVegan} {geenLactose} {geenGluten} {geenVis}");
        //    Debug.WriteLine($"complexpartUrl: {complexpartUrl}");


        //    // Use await to asynchronously get the data
        //    spoonacularApi complexResult = await GetData("complex", zoek);

        //    //spoonacularApi complexResult = GetData("complex", zoek).Result; // If GetData is async, you need to await it or use .Result in a non-async context

        //    List<string> ingrediënten = new List<string>();
        //    string Recept = string.Empty;

        //    if (complexResult != null && complexResult.Recipes != null && complexResult.Recipes.Any())
        //    {
        //        var firstRecipe = complexResult.Recipes.First();

        //        // Extract ingredients
        //        foreach (var ingredient in firstRecipe.ExtendedIngredients)
        //        {
        //            ingrediënten.Add($"{ingredient.Name} - {ingredient.Amount} {ingredient.Unit}");
        //        }

        //        // Construct the recipe description
        //        Recept = $"{firstRecipe.Title}\n\nInstructions:\n{firstRecipe.Instructions}";
        //    }
        //    else
        //    {
        //        // Handle the case where no recipes are found or the result is null
        //        ingrediënten = new List<string> { "No ingredients found." };
        //        Recept = "No recipe found.";
        //    }

        //    return (ingrediënten, Recept);
        //}

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

            if (complexResult != null && complexResult.Recipes != null && complexResult.Recipes.Any())
            {
                // This example assumes you are interested in details for the first recipe
                var firstRecipeId = complexResult.Recipes.First().Id;
                spoonacularApi detailedRecipe = await GetRecipeDetailsAsync(firstRecipeId);

                if (detailedRecipe != null && detailedRecipe.Recipes != null && detailedRecipe.Recipes.Any())
                {
                    var recipeDetails = detailedRecipe.Recipes.First();
                    ingredients.AddRange(recipeDetails.ExtendedIngredients.Select(i => $"{i.Name} - {i.Amount} {i.Unit}"));
                    recipeDescription = $"{recipeDetails.Title}\n\nInstructions:\n{recipeDetails.Instructions}";
                }
            }
            else
            {
                ingredients = new List<string> { "No ingredients found." };
                recipeDescription = "No recipe found.";
            }

            return (ingredients, recipeDescription);
        }

        public async Task<spoonacularApi> GetRecipeDetailsAsync(int recipeId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.spoonacular.com/recipes/{recipeId}/information?apiKey={ApiKey}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<spoonacularApi>(json);
                }
            }
            return null;
        }

        public void ComplexSearch(string zoek, bool isVegan, bool geenLactose, bool geenGluten, bool geenVis, out List<string> ingrediënten, out string Recept)
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
                intolerances = intolerances.TrimEnd(','); //Laatste komma weghalen
                complexpartUrl += "&intolerances=" + intolerances;
            }

            spoonacularApi complexResult = GetData("complex", zoek).Result; // If GetData is async, you need to await it or use .Result in a non-async context

            ingrediënten = new List<string>();
            Recept = string.Empty;

            if (complexResult != null && complexResult.Recipes != null && complexResult.Recipes.Any())
            {
                var firstRecipe = complexResult.Recipes.First();

                // Extract ingredients
                foreach (var ingredient in firstRecipe.ExtendedIngredients)
                {
                    ingrediënten.Add($"{ingredient.Name} - {ingredient.Amount} {ingredient.Unit}");
                }

                // Construct the recipe description
                Recept = $"{firstRecipe.Title}\n\nInstructions:\n{firstRecipe.Instructions}";
            }
            else
            {
                // Handle the case where no recipes are found or the result is null
                ingrediënten = new List<string> { "No ingredients found." };
                Recept = "No recipe found.";
            }
        }


        private const string ApiKey = "1c312f036b1c45e7b5a00ec292622ca3";

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

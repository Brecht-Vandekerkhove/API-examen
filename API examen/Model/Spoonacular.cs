using API_examen.Data.Services;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;

namespace API_examen.Model
{

    internal class Spoonacular
    {
        #region initialisatiestatements
        string complexpartUrl = "";
        private const string ApiKey = "1c312f036b1c45e7b5a00ec292622ca3";
        //2de ApiKey want rate-limit per ApiKey van 1 request per seconde
        private const string ApiKey2 = "355943b82e6c444a8803d0b07ac85a84";
        #endregion
        #region Methoden exclusief api-calls
        public async Task<(Dictionary<string, int>, List<string>)> ComplexSearchDictionary(
            string zoek,
            bool isVegan,
            bool isVegetarian,
            bool isKetogenic,
            bool isPrimal,
            bool dairyIntolerance,
            bool glutenIntolerance,
            bool seafoodIntolerance,
            bool peanutIntolerance
            )

        {
            complexpartUrl = "";

            // Dieet
            List<string> dietReq = new List<string>();
            if (isVegan) dietReq.Add("vegan");
            if (isVegetarian) dietReq.Add("vegetarian");
            if (isKetogenic) dietReq.Add("ketogenic");
            if (isPrimal) dietReq.Add("primal");

            if (dietReq.Any())
            {
                complexpartUrl += "&diet=" + string.Join(",", dietReq);
            }

            // Intoleranties
            List<string> intoleranties = new List<string>();
            if (dairyIntolerance) intoleranties.Add("dairy");
            if (glutenIntolerance) intoleranties.Add("gluten");
            if (seafoodIntolerance) intoleranties.Add("seafood");
            if (peanutIntolerance) intoleranties.Add("peanut");

            if (intoleranties.Any())
            {
                complexpartUrl += "&intolerances=" + string.Join(",", intoleranties);
            }

            Debug.WriteLine($"Search string: {zoek}, diet and intolerances {complexpartUrl}");

            // Data ophalen met GetData (api-call)
            spoonacularApi complexResult = await GetData(zoek);
            Dictionary<string, int> recepten = new Dictionary<string, int>();
            List<string> receptenTitels = new List<string>();

            Debug.WriteLine($"Start ComplexSearchDictionary if-else structure");
            if (complexResult != null && complexResult.Recipes != null && complexResult.Recipes.Any())
            {
                MessageBox.Show($"Some recipes have been found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Stel lijst en dictionary op met titels + id van relevante recepten
                foreach (var recipe in complexResult.Recipes)
                {
                    recepten.Add(recipe.Title, recipe.Id);
                    receptenTitels.Add(recipe.Title);
                }
            }
            else
            {
                Debug.WriteLine($"Else-route");
                MessageBox.Show($"Geen recipe found.\r\nChange the input!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return (recepten, receptenTitels);
        }

        public async Task<(List<string> Ingredients, string Recipe, string RecipeTitel)> RecipeData(int id)
        {
            List<string> ingredients = new List<string>();
            string recipeDescription = string.Empty;
            string receptTitel = string.Empty;

            //Receptgegevens opvragen op basis van id
            DetailedRecipeResponse detailedRecipe = await GetRecipeDetailsAsync(id);
            if (detailedRecipe != null)
            {
                receptTitel = detailedRecipe.Title;
                Debug.WriteLine($"Found {receptTitel}");

                //receptbeschrijving soms in html (<li> ect) -> html elementen verwijderen)
                recipeDescription = $"Instructions:\n{CleanHtmlContent(detailedRecipe.Instructions)}";

                //Voor elk ingrediënt een string opstellen in de vorm van "100 g brood"
                foreach (var i in detailedRecipe.ExtendedIngredients)
                {
                    string ingredientString = $"{i.Amount} {i.Unit} {i.Name}";
                    if (!ingredients.Contains(ingredientString))
                    {
                        ingredients.Add(ingredientString);
                    }
                }
                MessageBox.Show($"U can now view the ingredients and recipe for {receptTitel}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Debug.WriteLine($"DetailedRecipe = null in RecipeData()");
                ingredients = new List<string> { "No ingredients found." };
                recipeDescription = "No recipe found.";
            }
            return (ingredients, recipeDescription, receptTitel);
        }
        private string CleanHtmlContent(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            // <li> aanpassen
            string withBullets = Regex.Replace(html, @"<li>(.*?)</li>", m => $"- {m.Groups[1].Value}\n");

            // Overige HTML tags verwijderen
            string withoutHtml = Regex.Replace(withBullets, @"<[^>]+>", string.Empty);

            // Decoderen HTML
            string decoded = System.Net.WebUtility.HtmlDecode(withoutHtml);

            return decoded.Trim();
        }
        #endregion
        #region methoden met api-calls
        public async Task<spoonacularApi> GetData(string zoekopdracht)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string urlApi = $"https://api.spoonacular.com/recipes/complexSearch?query={zoekopdracht}{complexpartUrl}&apiKey={ApiKey}";

                    HttpResponseMessage response = await client.GetAsync(urlApi);
                    if (response.IsSuccessStatusCode)
                    {
                        // Lees api response
                        var json = await response.Content.ReadAsStringAsync();
                        var SpoonacularApi = JsonSerializer.Deserialize<spoonacularApi>(json);

                        // Als alles goed gaat, retourneer gedeserialiseerde json
                        return SpoonacularApi;
                    }
                    //Als de API geen overeenkomstig recept vindt,
                    //retourneert het een 404 (Not Found) HTTP-statuscode.
                    //In dit geval zou EnsureSuccessStatusCode() een HttpRequestException veroorzaken.
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Debug.WriteLine("Recept niet gevonden.");
                    }
                    else
                    {
                        // Andere fout bij het ophalen van gegevens, toon foutmelding
                        Debug.WriteLine($"Error while retrieving data: {response.ReasonPhrase}");
                        MessageBox.Show($"Error while retrieving data: {response.ReasonPhrase}", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (HttpRequestException)
            {
                Debug.WriteLine("Error: couldn't get a connection with api.");
                MessageBox.Show("Fout: kon geen verbinding maken met api.", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error GetIngredientInfo: {ex.Message}");
                MessageBox.Show("Fout: unkown error.", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
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

        #endregion
    }
}

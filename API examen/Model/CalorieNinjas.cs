using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace API_examen
{
    class CalorieNinjas
    {
        private const string ApiKey = "g/23Fxfm97zN7S0LH/PoTQ==bNVX1aMlg1pxdj5I";
        private const string ApiUrl = "https://api.calorieninjas.com/v1/nutrition?query=";

        public async Task<string> GetIngredientInfo(string ingredient)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //string url = $"{ApiUrl}{ingredient}&api_key={ApiKey}";
                    string url = $"{ApiUrl}{ingredient}";

                    // Api toevoegen in request header
                    client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"JSON Response: {json}");

                        var ingredientInfo = JsonSerializer.Deserialize<CalorieNinjasApi>(json);

                        if (ingredientInfo?.Items != null && ingredientInfo.Items.Any())
                        {
                            var item = ingredientInfo.Items.First();
                            string result = $"Name: {item.Name}\n" +
                                            $"Estimated serving size: {item.ServingSizeG}g\n\n" +
                                            $"Calories: {item.Calories}\n" +
                                            "Macros:\n" +
                                            $"     - Protein: {item.ProteinG}g\n" +
                                            $"     - Carbs: {item.CarbohydratesTotalG}g\n" +
                                            $"     - Fibers: {item.FiberG} g\n" +
                                            $"     - Fats: {item.FatTotalG} g\n" +
                                            $"          - from which saturated: {item.FatSaturatedG}g\n" +
                                            "Others:\n" +
                                            $"     - Sugar: {item.SugarG}g\n" +
                                            $"     - Cholesterol: {item.CholesterolMg}mg\n" +
                                            $"     - Sodium: {item.SodiumMg}mg\n" +
                                            $"     - Potassium: {item.PotassiumMg}mg\n";

                            return result;
                        }
                        return "Ingredient information not found.";
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Debug.WriteLine("Ingredient niet gevonden.");
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error GetIngredientInfo: {ex.Message}");
            }
            return null;
        }
    }
}

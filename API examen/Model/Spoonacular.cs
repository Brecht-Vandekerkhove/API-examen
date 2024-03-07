using API_examen.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace API_examen.Model
{
    internal class Spoonacular
    {
        private const string ApiKey = "1c312f036b1c45e7b5a00ec292622ca3";

        public async Task<spoonacularApi> GetData(string type, string zoekopdracht)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = null;

                switch (type)
                {
                    case "random recept":
                        response = await client.GetAsync($"https://api.spoonacular.com/recipes/random?apiKey={ApiKey}&tags={zoekopdracht}&number=3");
                        break;
                    case "test":
                        // code block
                        response = await client.GetAsync($"https://api.spoonacular.com/recipes/random?apiKey={ApiKey}&tags={zoekopdracht}&number=3");
                        break;
                    default:
                        // If the type is not recognized, return null
                        return null;
                }

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

        public void recept(string zoek)
        {
            var test = GetData("random recept", zoek);
        }
        public void test(string zoek)
        {
            var test2 = GetData("test", zoek);
        }
    }
}

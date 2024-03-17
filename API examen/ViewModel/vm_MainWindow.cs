using API_examen.Model;
using API_examen.ViewModel.Commands;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace API_examen.ViewModel
{
    internal class vm_MainWindow : vmBase
    {
        #region Constructor en initialisatiestatements
        Dictionary<string, int> receptenDictionary;
        public ICommand zoekCommand { get; }
        public ICommand servingSizeCmd { get; }
        private Spoonacular spoonacular;
        private CalorieNinjas calorieNinjas;
        public vm_MainWindow()
        {
            receptenDictionary = new Dictionary<string, int>();
            zoekCommand = new RelayCommand(ZoekCmd);
            servingSizeCmd = new RelayCommand(ServingSizeCmd);
            spoonacular = new Spoonacular();
            calorieNinjas = new CalorieNinjas();
        }
        #endregion

        #region Binding Elementen
        #region Binding voor buttons
        private string _zoek;
        public string Zoek { get => _zoek; set { if (_zoek != value) { _zoek = value; OnPropertyChange(nameof(Zoek)); } } }

        private string _serving;
        public string Serving { get => _serving; set { if (_serving != value) { _serving = value; OnPropertyChange(nameof(Serving)); } } }
        #endregion
        #region Binding voor weergeven data
        private List<string> _ReceptenTitels;
        public List<string> ReceptenTitels
        {
            get => _ReceptenTitels;
            set
            {
                _ReceptenTitels = value;
                OnPropertyChange(nameof(ReceptenTitels));
            }
        }

        private string _selectedRecept;
        public string SelectedRecept
        {
            get => _selectedRecept;
            set
            {
                if (_selectedRecept != value)
                {
                    _selectedRecept = value;
                    OnPropertyChange(nameof(SelectedRecept));

                    GetRecipeData();
                }
            }
        }

        private List<string> _ingredients;
        public List<string> Ingredients
        {
            get => _ingredients;
            set
            {
                _ingredients = value;
                OnPropertyChange(nameof(Ingredients));
            }
        }

        private string _recept;
        public string Recipe
        {
            get => _recept;
            set
            {
                _recept = value;
                OnPropertyChange(nameof(Recipe));
            }
        }


        private string _receptTitel;
        public string ReceptTitel
        {
            get => _receptTitel;
            set
            {
                if (_receptTitel != value)
                {
                    _receptTitel = value;
                    OnPropertyChange(nameof(ReceptTitel));
                }
            }
        }
        #endregion
        #region Binding checkboxes
        private bool _isVegetarian;
        public bool isVegetarian
        {
            get => _isVegetarian;
            set
            {
                if (_isVegetarian != value)
                {
                    _isVegetarian = value;
                    OnPropertyChange(nameof(isVegetarian));
                }
            }
        }
        private bool _isVegan;
        public bool isVegan
        {
            get => _isVegan;
            set
            {
                if (_isVegan != value)
                {
                    _isVegan = value;
                    OnPropertyChange(nameof(isVegan));
                }
            }
        }

        private bool _isKetogenic;
        public bool isKetogenic
        {
            get => _isKetogenic;
            set
            {
                if (_isKetogenic != value)
                {
                    _isKetogenic = value;
                    OnPropertyChange(nameof(isKetogenic));
                }
            }
        }

        private bool _isPrimal;
        public bool isPrimal
        {
            get => _isPrimal;
            set
            {
                if (_isPrimal != value)
                {
                    _isPrimal = value;
                    OnPropertyChange(nameof(isPrimal));
                }
            }
        }

        private bool _dairyIntolerance;
        public bool dairyIntolerance
        {
            get => _dairyIntolerance;
            set
            {
                if (_dairyIntolerance != value)
                {
                    _dairyIntolerance = value;
                    OnPropertyChange(nameof(dairyIntolerance));
                }
            }
        }

        private bool _glutenIntolerance;
        public bool glutenIntolerance
        {
            get => _glutenIntolerance;
            set
            {
                if (_glutenIntolerance != value)
                {
                    _glutenIntolerance = value;
                    OnPropertyChange(nameof(glutenIntolerance));
                }
            }
        }

        private bool _seafoodIntolerance;
        public bool seafoodIntolerance
        {
            get => _seafoodIntolerance;
            set
            {
                if (_seafoodIntolerance != value)
                {
                    _seafoodIntolerance = value;
                    OnPropertyChange(nameof(seafoodIntolerance));
                }
            }
        }

        private bool _peanutIntolerance;
        public bool peanutIntolerance
        {
            get => _peanutIntolerance;
            set
            {
                if (_peanutIntolerance != value)
                {
                    _peanutIntolerance = value;
                    OnPropertyChange(nameof(peanutIntolerance));
                }
            }
        }
        #endregion
        #region Binding voor opzoeken ingrediënten
        private string _selectedIngredient;
        public string SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                if (_selectedIngredient != value)
                {
                    _selectedIngredient = value;
                    OnPropertyChange(nameof(SelectedIngredient));

                    GetIngredient();
                }
            }
        }

        private string _ingredientInfo;
        public string IngredientInfo
        {
            get => _ingredientInfo;
            set
            {
                if (_ingredientInfo != value)
                {
                    _ingredientInfo = value;
                    OnPropertyChange(nameof(IngredientInfo));

                    GetIngredient();
                }
            }
        }
        #endregion
        #endregion

        #region methodes voor commands
        private async void ZoekCmd(object parameter)
        {
            Debug.WriteLine($"Zoek gestart met {Zoek}");

            try
            {
                // Await de resultaten van de asynchronous method en sla ze op
                var (localReceptenDictionary, localReceptenTitels) = await spoonacular.ComplexSearchDictionary(
                    Zoek,
                    isVegan,
                    isVegetarian,
                    isKetogenic,
                    isPrimal,
                    dairyIntolerance,
                    glutenIntolerance,
                    seafoodIntolerance,
                    peanutIntolerance
                    );
                //dictionary om later makkelijker id op te halen
                receptenDictionary = localReceptenDictionary;
                //list om te weergeven in listbox
                ReceptenTitels = localReceptenTitels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ServingSizeCmd(object parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(SelectedIngredient))
                {
                    //Huidige eenheden (bvb 2 cups) weghalen
                    //Werkt ook voor bvb "3 tomatoes", omdat deze in de listbox weergeven worden met 2 spaties
                    string tempIngredient = String.Join(" ", SelectedIngredient.Split(' ').Skip(2));

                    //Nieuwe eenheden uit textbox toevoegen, 100g in geval van ongeldige/geen input
                    if (!string.IsNullOrEmpty(Serving) && double.TryParse(Serving, out double servingAmount))
                    {
                        SelectedIngredient = $"{servingAmount} g {tempIngredient}";
                    }
                    else if (string.IsNullOrEmpty(Serving)) SelectedIngredient = $"100 g {tempIngredient}";
                    else
                    {
                        SelectedIngredient = $"100 g {tempIngredient}";
                        Serving = string.Empty;
                        MessageBox.Show($"Incorrect input!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //Ingrediëntinfo opnieuw genereren
                    IngredientInfo = await calorieNinjas.GetIngredientInfo(SelectedIngredient);
                }
            }
            catch
            {
                Debug.WriteLine("Fout in ServingSizeCmd.");
            }
        }
        #endregion
        #region methodes voor listboxitem changed
        private async void GetRecipeData()
        {
            //Geselecteerde recept uit listbox ophalen
            if (!string.IsNullOrEmpty(SelectedRecept))
            {
                //Reset textbox met ingrediëntinfo
                IngredientInfo = string.Empty;

                Debug.WriteLine($"Opzoeken info: {SelectedRecept}");

                //Bijhorende id uit dictionary ophalen
                if (receptenDictionary.TryGetValue(SelectedRecept, out int id))
                {
                    Debug.WriteLine($"The ID for '{SelectedRecept}' is {id}.");

                    //Recept zoeken met id
                    var (localIngredients, localRecipe, localRecipeTitel) = await spoonacular.RecipeData(id);

                    //Resultaat weergeven dmv binding
                    Ingredients = localIngredients;
                    Recipe = localRecipe;
                    ReceptTitel = localRecipeTitel;
                }
                else
                {
                    Debug.WriteLine("Recipe title not found.");
                }
            }
        }

        private async void GetIngredient()
        {
            //Geselecteerde ingrediënt uit listbox ophalen
            if (!string.IsNullOrEmpty(SelectedIngredient))
            {
                Debug.WriteLine($"Opzoeken info: {SelectedIngredient}");

                //IngredientInfo weergeven
                IngredientInfo = await calorieNinjas.GetIngredientInfo(SelectedIngredient);
            }
        }
        #endregion
    }
}

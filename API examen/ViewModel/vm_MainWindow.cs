using API_examen.Model;
using API_examen.ViewModel.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static API_examen.Data.Services.spoonacularApi;

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

        private string _zoek;
        public string Zoek { get => _zoek; set { if (_zoek != value) { _zoek = value; OnPropertyChange(nameof(Zoek)); } } }

        private string _serving;
        public string Serving { get => _serving; set { if (_serving != value) { _serving = value; OnPropertyChange(nameof(Serving)); } } }
        
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

        //public string Recept
        //{
        //    get => _recept;
        //    set
        //    {
        //        if (_recept != value)
        //        {
        //            _recept = value;
        //            // Assume the title is the first line of the recipe description
        //            ReceptTitel = value.Split('\n').FirstOrDefault()?.Trim();
        //            OnPropertyChange(nameof(Recept));
        //        }
        //    }
        //}
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

        //Code voor opzoeken ingrediënten van de listbox
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

        private async void ZoekCmd(object parameter)
        {
            Debug.WriteLine($"Zoek gestart met {Zoek}");
            //spoonacular.recept(Zoek);
            //List<string> localIngredients;
            //string localRecipe;

            //spoonacular.ComplexSearch(Zoek, isVegan, geenLactose, geenGluten, geenVis, out localIngredients, out localRecipe);

            //Ingredients = localIngredients;
            //Recipe = localRecipe;

            try
            {
                //// Await the result from the asynchronous method and store it in local variables
                //var (localIngredients, localRecipe, localRecipeTitel) = await spoonacular.ComplexSearchAsync(
                //    Zoek,
                //    isVegan,
                //    isVegetarian,
                //    isKetogenic,
                //    isPrimal,
                //    dairyIntolerance,
                //    glutenIntolerance,
                //    seafoodIntolerance,
                //    peanutIntolerance
                //    );

                //// Assign the values to your ViewModel properties
                //Ingredients = localIngredients;
                //Recipe = localRecipe;
                //ReceptTitel = localRecipeTitel;

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
                receptenDictionary = localReceptenDictionary;
                ReceptenTitels = localReceptenTitels;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void GetRecipeData()
        {
            if (!string.IsNullOrEmpty(SelectedRecept))
            {
                Debug.WriteLine($"Opzoeken info: {SelectedRecept}");

                if (receptenDictionary.TryGetValue(SelectedRecept, out int id))
                {
                    Debug.WriteLine($"The ID for '{SelectedRecept}' is {id}.");

                    var (localIngredients, localRecipe, localRecipeTitel) = await spoonacular.RecipeData(id);

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

        private async void ServingSizeCmd(object parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(SelectedIngredient))
                {
                    string tempIngredient = String.Join(" ", SelectedIngredient.Split(' ').Skip(2));
                    if (!string.IsNullOrEmpty(Serving) && double.TryParse(Serving, out double servingAmount))
                    {
                        SelectedIngredient = $"{servingAmount} g {tempIngredient}";
                    }
                    else if (string.IsNullOrEmpty(Serving)) SelectedIngredient = $"100 g {tempIngredient}";
                    else MessageBox.Show($"Incorrect input!", "Foutmelding", MessageBoxButton.OK, MessageBoxImage.Error);

                    IngredientInfo = await calorieNinjas.GetIngredientInfo(SelectedIngredient);
                }
            }
            catch
            {

            }
        }

        private async void GetIngredient()
        {
            if (!string.IsNullOrEmpty(SelectedIngredient))
            {
                Debug.WriteLine($"Opzoeken info: {SelectedIngredient}");

                IngredientInfo = await calorieNinjas.GetIngredientInfo(SelectedIngredient);
            }
        }
    }
}

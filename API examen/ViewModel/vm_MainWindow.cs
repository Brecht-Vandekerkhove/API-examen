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

namespace API_examen.ViewModel
{
	internal class vm_MainWindow : vmBase
	{
        #region Constructor en initialisatiestatements
        public ICommand SearchCommand { get; }
        private Spoonacular spoonacular;
        private CalorieNinjas calorieNinjas;
        public vm_MainWindow()
        {
            SearchCommand = new RelayCommand(ZoekCmd);
            spoonacular = new Spoonacular();
            calorieNinjas = new CalorieNinjas();
        }
        #endregion

        #region Binding Elementen

        private string _zoek;
        public string Zoek { get => _zoek; set { if (_zoek != value) { _zoek = value; OnPropertyChange(nameof(Zoek)); } } }

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

        private bool _geenLactose;
        public bool geenLactose
        {
            get => _geenLactose;
            set
            {
                if (_geenLactose != value)
                {
                    _geenLactose = value;
                    OnPropertyChange(nameof(geenLactose));
                }
            }
        }

        private bool _geenGluten;
        public bool geenGluten
        {
            get => _geenGluten;
            set
            {
                if (_geenGluten != value)
                {
                    _geenGluten = value;
                    OnPropertyChange(nameof(geenGluten));
                }
            }
        }

        private bool _geenVis;
        public bool geenVis
        {
            get => _geenVis;
            set
            {
                if (_geenVis != value)
                {
                    _geenVis = value;
                    OnPropertyChange(nameof(geenVis));
                }
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
                // Await the result from the asynchronous method and store it in local variables
                var (localIngredients, localRecipe, localRecipeTitel) = await spoonacular.ComplexSearchAsync(Zoek, isVegan, _geenLactose, _geenGluten, _geenVis);

                // Assign the values to your ViewModel properties
                Ingredients = localIngredients;
                Recipe = localRecipe;
                ReceptTitel = localRecipeTitel;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

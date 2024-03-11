using API_examen.Model;
using API_examen.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace API_examen.ViewModel
{
	internal class vm_MainWindow : vmBase
	{
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

        private string _recipe;
        public string Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
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


        public ICommand SearchCommand { get; }
        private Spoonacular spoonacular;
        public vm_MainWindow()
        {
            SearchCommand = new RelayCommand(ZoekCmd);
            spoonacular = new Spoonacular();
        }


        private void ZoekCmd(object parameter)
        {
            //spoonacular.recept(Zoek);
            List<string> localIngredients;
            string localRecipe;

            spoonacular.ComplexSearch(Zoek, isVegan, geenLactose, geenGluten, geenVis, out localIngredients, out localRecipe);

            Ingredients = localIngredients;
            Recipe = localRecipe;
        }
    }
}

using API_examen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace API_examen.View
{
    /// <summary>
    /// Interaction logic for GUI_MainWindow.xaml
    /// </summary>
    public partial class GUI_MainWindow : Window
    {
        public GUI_MainWindow()
        {
            InitializeComponent();
            DataContext = new vm_MainWindow();
            //DataContext = vm_MainWindow;
        }
    }
}

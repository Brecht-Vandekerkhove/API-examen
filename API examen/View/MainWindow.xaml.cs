using API_examen.ViewModel;
using System.Windows;

namespace API_examen.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new vm_MainWindow();
        }
    }
}

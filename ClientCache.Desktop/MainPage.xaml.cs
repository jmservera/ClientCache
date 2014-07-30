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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientCache.Desktop
{
    public class Commands
    {
        public Commands()
        {

        }
        public ICommand NoCodeCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    NavigationService.GetNavigationService((DependencyObject)o)
                        .Navigate(new ImagesTest());
                });
            }
        }
    }
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();
        }
    }
}

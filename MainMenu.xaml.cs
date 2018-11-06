
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

namespace MessageBank
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        Validations menuValidation = new Validations();





        public MainMenu()
        {
            InitializeComponent();
        }






        #region Click Events

        private void btn_inputText_click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            Input inputPage = new Input();

            // Navigates to the input page
            NavigationService.Navigate(inputPage);
        }


        #endregion





        #region Exit

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            // Calls the method from validation to exit
            menuValidation.ExitAppValidation();
        }
    }

    #endregion
}
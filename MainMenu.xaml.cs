
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
        #region Object

        Validations menuValidation = new Validations();

        #endregion

        #region Constructor

        public MainMenu()
        {
            InitializeComponent();
        }

        #endregion

        #region Click Events

        private void manuallyInputButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            Input input = new Input();

            // Navigates to the InputManually page
            NavigationService.Navigate(input);
        }

        private void autoInputButton_Click(object sender, RoutedEventArgs e)
        {
            // later
        }

        private void viewMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the ViewMessages Page           
            ViewMessages viewMessages = new ViewMessages();

            // Navigates to the InputManually page
            NavigationService.Navigate(viewMessages);

        }

        private void exportJson_Click(object sender, RoutedEventArgs e)
        {

        }


        #endregion

        #region Exit Button

        // Method which handles the 'Exit Application' button being clicked
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            // Calls the method ExitApplicationValidation() from the Validation class.
            menuValidation.ExitAppValidation();
        }




        #endregion

    }
}
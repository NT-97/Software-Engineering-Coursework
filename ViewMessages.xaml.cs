
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
    /// Interaction logic for ViewMessagesPage.xaml
    /// </summary>
    public partial class ViewMessages : Page
    {
        Validations validation = new Validations();
        Json json = new Json();



        public ViewMessages()
        {
            InitializeComponent();
            DisplayNewMessage();
        }


        #region Click Events

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayNewMessage();
        }

        #endregion


        #region Navigation Buttons

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            MainMenu mainmenu = new MainMenu();

            // Navigates to the InputManually page
            NavigationService.Navigate(mainmenu);
        }

        // Method which handles the 'Exit Application' button being clicked
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {


            // Calls the method ExitApplicationValidation() from the Validation class.
            validation.ExitAppValidation();
        }

        #endregion


        #region Private Methods

        private void DisplayNewMessage()
        {
            //MessageClass message = json.Deserialize();            

            //messageHeaderTxt.Text = message.Header;
            //messageBodyTxt.Text = message.MessageText;    
        }

        #endregion

    }
}

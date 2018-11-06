
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
    /// Interaction logic for InputManuallyPage.xaml
    /// </summary>
    public partial class Input : Page
    {
        Validations validation = new Validations();


        public Input()
        {
            InitializeComponent();
        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

            if (validation.HeaderInputValidation(txt_Header.Text).Equals(false))
            {
                MessageBox.Show("You have entered the header incorrectly.");
                txt_Header.Focus();
                return;
            }

            if (validation.MessageBodyInputValidation(txt_Body.Text).Equals(false))
            {
                MessageBox.Show("You have entered the body incorrectly.");
                txt_Body.Focus();
                return;
            }



        }



        #region Navigation Buttons

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            MainMenu menuPage = new MainMenu();

            // Navigates to the InputManually page
            NavigationService.Navigate(menuPage);
        }

        // Method which handles the 'Exit Application' button being clicked
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {


            // Calls the method ExitApplicationValidation() from the Validation class.
            validation.ExitAppValidation();
        }

        #endregion

    }
}
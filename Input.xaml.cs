﻿
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
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : Page
    {
        Validations validation = new Validations();
        Json json = new Json();



        public Input()
        {
            InitializeComponent();
            validation.LoadStoredList();
        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {



            // Converts the whole list of messages into JSON and stores it
            json.Serialize(validation.messagesList);

        }

        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            // Validates the message header
            if (validation.InputMessageCheck(messageHeaderTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("The Header has been input incorrectly, please try again.");
                messageHeaderTxt.Focus();
                return;
            }

            // Validates the message body
            if (validation.InputBodyCheck(messageBodyTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("The Body has been input incorrectly, please try again");
                messageBodyTxt.Focus();
                return;
            }
        }



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
    }
}
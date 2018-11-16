
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
        Json json = new Json();
        DataProcess dataProcess = new DataProcess();

        
        





        public Input()
        {
            InitializeComponent();
            validation.RetrieveStoredList();
        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {


            string path = @".\MessageBank.json";

            validation.AddMessageToList();


            // Converts the whole list of messages into JSON and stores it
            json.Serialize(validation.listOfMessages, path);

            saveButton.IsEnabled = false;

           // validation.EndOfCycle();

            convertedMessageHeaderTxt.Text = string.Empty;
            convertedMessageSenderTxt.Text = string.Empty;
            convertedMessageSubjectTxt.Text = string.Empty;
            convertedMessageBodyTxt.Text = string.Empty;
            messageHeaderTxt.Text = string.Empty;
            messageBodyTxt.Text = string.Empty;


        }

        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            // Validates the message header
            if (validation.MessageHeaderInputValidation(messageHeaderTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the header incorrectly.");
                messageHeaderTxt.Focus();
                return;
            }

            // Validates the message body
            if (validation.MessageBodyInputValidation(messageBodyTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the body incorrectly.");
                messageBodyTxt.Focus();
                return;
            }

            saveButton.IsEnabled = true;


            string text = validation.Text;
            string header = validation.Header;


            dataProcess.MessageProcessing(header, ref text);


            convertedMessageHeaderTxt.Text = header;
            convertedMessageSenderTxt.Text = validation.Sender;
            convertedMessageSubjectTxt.Text = validation.Subject;
            convertedMessageBodyTxt.Text = text;


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
            validation.ExitApplicationValidation();
        }


        #endregion        
    }
}
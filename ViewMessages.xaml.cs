
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
    public partial class ViewMessages 
    {
        Validations validation = new Validations();
        Json json = new Json();

        List<Messages> messagesList = new List<Messages>();

        int messageCounter = 0;

        public ViewMessages()
        {
            InitializeComponent();
            RetrieveStoredList();
            DisplayFirstMessage();

        }


        #region Click Events

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            DisplayNextMessage();
        }

        private void btn_Previous_Click(object sender, RoutedEventArgs e)
        {
            DisplayPreviousMessage();
        }

        #endregion


        #region Navigation Buttons

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mainmenu = new MainMenu();
            
            NavigationService.Navigate(mainmenu);
        }

        // Method which handles the 'Exit Application' button being clicked
        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {


            // Calls the method ExitApplicationValidation() from the Validation class.
            validation.ExitApplicationValidation();
        }

        #endregion


        #region Private Methods

        private void DisplayFirstMessage()
        {
            if (messageCounter < (messagesList.Count - 1))
            {
                header_txt.Text = messagesList[messageCounter].Header;
                sender_txt.Text = messagesList[messageCounter].Sender;
                subject_txt.Text = messagesList[messageCounter].Subject;
                body_txt.Text = messagesList[messageCounter].Text;

            }
            else
            {
                MessageBox.Show("There are no more messages in the list to view.");
            }

        }


        private void DisplayNextMessage()
        {
            if (messageCounter < (messagesList.Count - 1))
            {
                messageCounter = messageCounter + 1;

                header_txt.Text = messagesList[messageCounter].Header;
                sender_txt.Text = messagesList[messageCounter].Sender;
                subject_txt.Text = messagesList[messageCounter].Subject;
                body_txt.Text = messagesList[messageCounter].Text;

            }
            else
            {
                MessageBox.Show("There are no more messages in the list to view.");
            }
        }

        private void DisplayPreviousMessage()
        {
            if (messageCounter > 0)
            {
                messageCounter = messageCounter - 1;

                header_txt.Text = messagesList[messageCounter].Header;
                sender_txt.Text = messagesList[messageCounter].Sender;
                subject_txt.Text = messagesList[messageCounter].Subject;
                body_txt.Text = messagesList[messageCounter].Text;

            }
            else
            {
                MessageBox.Show("You are at the start of the list.");
            }
        }


        public void RetrieveStoredList()
        {
            int counter = 0;

            try
            {
                // Returns the list that is stored as JSON             
                messagesList = json.Deserialize();

                counter = counter + 1;



            }
            catch (Exception ex)
            {
                if (counter > 0)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        #endregion


    }
}
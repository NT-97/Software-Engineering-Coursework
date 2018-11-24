
#region Usings


using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#endregion

namespace MessageBank
{
  
    public partial class Input
    {
        #region Objects / Data Structure / Variables

        Validations validation = new Validations();
        Json json = new Json();
        DataProcess dataProcess = new DataProcess();

        List<string> importList = new List<string>();

        bool IsTheDataImported = false;

        private string processedText = string.Empty;

        int importCounter = 0;

        #endregion

        #region Constructor

        // Constructor
        public Input()
        {
            InitializeComponent();
            validation.RetrieveStoredList();
            dataProcess.RetrieveTextWordsFromCSV();
            dataProcess.RetrieveHashtagsFromCSV();
            dataProcess.RetrieveMentionsFromCSV();
            dataProcess.RetrieveSIRFromCSV();
        }

        #endregion

        #region Click Events

      
        
        private void btn_Convert_Click(object sender, RoutedEventArgs e)
        {
            // Validates the message header
            if (validation.MessageHeaderValidation(header_txt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the header incorrectly.");
                header_txt.Focus();
                return;
            }

            // Validates the message body
            if (validation.MessageBodyValidation(body_txt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the body incorrectly.");
                body_txt.Focus();
                return;
            }

            // Enables the 'save button'
            btn_Save.IsEnabled = true;

            string text = validation.Text;
            string header = validation.Header;
            string subject = validation.Subject;


            dataProcess.MessageProcessing(header, ref text, subject);

            // Splits the text up into different textboxes for the user to preview the information
            // Before saving
            convertedMessageHeaderTxt.Text = validation.Header;
            convertedMessageSenderTxt.Text = validation.Sender;
            convertedMessageSubjectTxt.Text = validation.Subject;
            convertedMessageBodyTxt.Text = text.Trim();
            processedText = text.Trim();
        }

        
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Sets the path as a string
            string path = @".\MessageBank.json";

            // If the message is a Tweet, then the method SearchForHashTagsAndMentions() is called
            if (validation.Header.StartsWith("T"))
            {
                dataProcess.FindHashtagsAndMentions(processedText);
            }

            //If the message is an Email, then the method SearchForSIR() is called
            if (validation.Header.StartsWith("E") && validation.Subject.StartsWith("SIR"))
            {
                dataProcess.FindSIR(processedText);

            }

            // Adds the message to a list
            validation.AddMessageToList(processedText);

            // Converts the whole list of messages into JSON and stores it
            json.Serialize(validation.messagesList, path);

            // Sets the save button to IsEnabled = false
            btn_Save.IsEnabled = false;

            validation.ResetStringValues();

            // Clears the textboxes
            convertedMessageHeaderTxt.Text = string.Empty;
            convertedMessageSenderTxt.Text = string.Empty;
            convertedMessageSubjectTxt.Text = string.Empty;
            convertedMessageBodyTxt.Text = string.Empty;
            header_txt.Text = string.Empty;
            body_txt.Text = string.Empty;
        }


      
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            if (IsTheDataImported.Equals(false))
            {
                using (var reader = new StreamReader(@".\testdata.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        importList.Add(line.ToString());
                    }
                }

                IsTheDataImported = true;
            }

            // Once a file is uploaded the content of the button changes to next message
            btn_Import.Content = "Next Message";

            ImportDataSplitMethod();
        }

        #endregion

        #region Private Method

       
        private void ImportDataSplitMethod()
        {
            // This IF statement stop an out of range exception
            if (importCounter < importList.Count)
            {
                string lineString = importList[importCounter];

                int firstComma = lineString.Trim().IndexOf(",");
                string headerString = lineString.Substring(0, firstComma);
                string bodyString = lineString.Substring(firstComma + 1);

                header_txt.Text = headerString.Trim();
                body_txt.Text = bodyString.Trim();

                importCounter = importCounter + 1;
            }
            else
            {
                MessageBox.Show("There are no more messages to import.");
            }
        }
         
        #endregion

        #region Navigation Buttons

               
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menuPage = new MainMenu();
            
            NavigationService.Navigate(menuPage);
        }
       
        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            validation.ExitApplicationValidation();
        }

        #endregion

    }
}


#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#endregion

namespace MessageBank
{

    public  partial class MainMenu
    {
        
        Validations mainmenuValidation = new Validations();
        Json json = new Json();
        AccessLists accessLists = new AccessLists();

     
        
        public MainMenu()
        {
            InitializeComponent();
     
        }
        
        #region Click Events
        
        private void btn_Input_Click(object sender, RoutedEventArgs e)
        {
            Input input = new Input();
            
            NavigationService.Navigate(input);
        }

        
        private void btn_ViewMessages_Click(object sender, RoutedEventArgs e)
        {
            ViewMessages viewMessages = new ViewMessages();

            NavigationService.Navigate(viewMessages);
        }
 
        private void btn_ExportJson_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult YorN = MessageBox.Show("Once the JSON is exported, would you like to delete the stored messages?", "Exit Application", MessageBoxButton.YesNo);

            if (YorN == MessageBoxResult.Yes)
            {
                ExportJsonFile();

                File.Delete(@".\MessageBank.json");

                File.WriteAllText(@".\hashtags.csv", String.Empty);
                File.WriteAllText(@".\mentions.csv", String.Empty);
                File.WriteAllText(@".\sir.csv", String.Empty);

                // Clears the trending lists
                accessLists.trendingListBox.Items.Clear();
                accessLists.mentionsListBox.Items.Clear();
                accessLists.sirListBox.Items.Clear();
            }
            else
            {
                ExportJsonFile();
            }
        }

    
        private void ExportJsonFile()
        {
            try
            {
                mainmenuValidation.RetrieveStoredList();

                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MessageBank.json";

                json.Serialize(mainmenuValidation.messagesList, folderPath);

                MessageBox.Show("JSON file has been exported to the your documents folder");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
        
       
        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            mainmenuValidation.ExitApplicationValidation();
        }

    


        private void btn_accessLists_Click(object sender, RoutedEventArgs e)
        {

            AccessLists accessLists = new AccessLists();

            NavigationService.Navigate(accessLists);
        }
    }
}
#endregion

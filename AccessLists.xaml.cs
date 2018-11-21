using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MessageBank
{
    
    public partial class AccessLists : Page
    {
        #region Objects / Data Structures

        Validations menuValidation = new Validations();
        Json jsonClass = new Json();
       

        Dictionary<string, int> mentionsDictionary = new Dictionary<string, int>();
        Dictionary<string, int> hashtagDictionary = new Dictionary<string, int>();

        List<string> hashTagList = new List<string>();
        List<string> mentionsList = new List<string>();
        List<string> sirList = new List<string>();

        #endregion

        #region Constructor

        public AccessLists()
        {
            InitializeComponent();
            RetrieveHashTags();
            RetrieveMentions();
            RetrieveSIR();
        }

        #endregion

        #region Updating ListBoxes

        
        #region Trending / Hashtag ListBox

        
        private void RetrieveHashTags()
        {
            using (var reader = new StreamReader(@".\hashtags.csv"))
            {
                while (!reader.EndOfStream)
                {
                    // Reads each line in the .csv file
                    var line = reader.ReadLine();

                    if (line.Contains("#") && line.Count() < 50)
                    {
                        string lineString = line.ToString();

                        // Uses substrings to split the string into a key and a value
                        int firstSpaceIndex = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpaceIndex);
                        string valueString = lineString.Substring(firstSpaceIndex + 1);

                        Int32.TryParse(valueString, out int valueInt);

                        // Stores the key and value in a dictionary
                        hashtagDictionary.Add(keyString.Trim(), valueInt);
                    }
                }
            }

            UpdateHashTagListBox();
        }

      
        private void UpdateHashTagListBox()
        {
            foreach (KeyValuePair<string, int> hashtag in hashtagDictionary)
            {
                hashTagList.Add(String.Format("[{0}] - {1}", hashtag.Value.ToString(), hashtag.Key));
            }
            

            int hashtagCounter = hashTagList.Count;

            foreach (var entry in hashTagList)
            {
                if (hashtagCounter > 0)
                {

                    trendingListBox.Items.Add(hashTagList[(hashtagCounter - 1)]);


                    hashtagCounter = hashtagCounter - 1;
                }
            }
        }

        #endregion

        #region Mentions ListBox
        
        private void RetrieveMentions()
        {
            using (var reader = new StreamReader(@".\mentions.csv"))
            {
                while (!reader.EndOfStream)
                {
                    // Reads each line in the .csv file
                    var line = reader.ReadLine();

                    if (line.Contains("@") && line.Count() < 20)
                    {
                        string lineString = line.ToString();
                        
                        int firstSpaceIndex = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpaceIndex);
                        string valueString = lineString.Substring(firstSpaceIndex + 1);

                        Int32.TryParse(valueString, out int valueInt);

                        mentionsDictionary.Add(keyString.Trim(), valueInt);
                    }
                }
            }

            UpdateMentionsListBox();
        }
        
        private void UpdateMentionsListBox()
        {
            foreach (KeyValuePair<string, int> mention in mentionsDictionary)
            {
                mentionsList.Add(String.Format("[{0}] - {1}", mention.Value.ToString(), mention.Key));
            }


            int mentionsCounter = mentionsList.Count;

            foreach (var entry in mentionsList)
            {
                if (mentionsCounter > 0)
                {
                    mentionsListBox.Items.Add(mentionsList[(mentionsCounter - 1)]);

                    mentionsCounter = mentionsCounter - 1;
                }
            }
        }

        #endregion

        #region SIR ListBox
        
        private void RetrieveSIR()
        {
            using (var reader = new StreamReader(@".\sir.csv"))
            {
                while (!reader.EndOfStream)
                {
                    // Reads each line in the .csv file
                    var line = reader.ReadLine();

                    if (line.Contains("-") && line.Count() < 100)
                    {
                        sirList.Add(line.ToString());
                    }
                }
            }

            UpdateSIRListBox();
        }
        
        private void UpdateSIRListBox()
        {
            int sirCounter = sirList.Count;

            foreach (string entry in sirList)
            {
                if (sirCounter > 0)
                {
                    sirListBox.Items.Add(sirList[(sirCounter - 1)]);

                    sirCounter = sirCounter - 1;
                }
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menuPage = new MainMenu();

            NavigationService.Navigate(menuPage);
        }

  
}
}



    #endregion

#endregion


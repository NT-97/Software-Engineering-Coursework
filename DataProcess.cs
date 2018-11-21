#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

#endregion

namespace MessageBank
{
    public class DataProcess
    {
    
        Dictionary<string, string> DictionaryTextWords = new Dictionary<string, string>();
        Dictionary<string, int> DictionaryMentions = new Dictionary<string, int>();
        Dictionary<string, int> DictionaryHashtags = new Dictionary<string, int>();

        List<string> sirList = new List<string>();
        List<string> quarantineList = new List<string>();


        // Default Constructor
        public DataProcess()
        {

        }

       
        public void MessageProcessing(string header, ref string text, string subject)
        {
            string checkHeader = string.Empty;

            // Takes the first char of the string header, converts the char to a string then converts it to uppercase
            checkHeader = header[0].ToString().ToUpper();

            if (checkHeader.Equals("S"))
            {
                ProcessedSms(ref text);
            }
            else if (checkHeader.Equals("E"))
            {
                ProcessedEmail(ref text, subject);
            }
            else if (checkHeader.Equals("T"))
            {
                ProcessedTweet(ref text);
            }
            else
            {
                MessageBox.Show("The Header has been input incorrectly");
            }
        }

        public void RetrieveTextWordsFromCSV()
        {
            using (var textWordCSVReader = new StreamReader(@".\textwords.csv"))
            {
                while (!textWordCSVReader.EndOfStream)
                {
                    var readline = textWordCSVReader.ReadLine();
                    string lineString = readline.ToString();

                    int firstComma = lineString.Trim().IndexOf(",");
                    string keyString = lineString.Substring(0, firstComma);
                    string valueString = lineString.Substring(firstComma + 1);

                    DictionaryTextWords.Add(keyString.Trim(), valueString.Trim());
                }
            }
        }

       
        public void RetrieveHashtagsFromCSV()
        {
            using (var hastagsCSVReader = new StreamReader(@".\hashtags.csv"))
            {
                while (!hastagsCSVReader.EndOfStream)
                {
                    var readline = hastagsCSVReader.ReadLine();

                    if (readline.Contains("#") && readline.Count() < 50)
                    {
                        string lineString = readline.ToString();

                        int firstSpace = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpace);
                        string valueString = lineString.Substring(firstSpace + 1);

                        Int32.TryParse(valueString, out int IntValue);

                        DictionaryHashtags.Add(keyString.Trim(), IntValue);
                    }
                }
            }
        }

       
        public void RetrieveMentionsFromCSV()
        {
            using (var mentionsCSVReader = new StreamReader(@".\mentions.csv"))
            {
                while (!mentionsCSVReader.EndOfStream)
                {
                    var line = mentionsCSVReader.ReadLine();

                    if (line.Contains("@") && line.Count() < 16)
                    {
                        string lineString = line.ToString();

                        int firstSpaceIndex = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpaceIndex);
                        string valueString = lineString.Substring(firstSpaceIndex + 1);

                        Int32.TryParse(valueString, out int valueInt);

                        DictionaryMentions.Add(keyString.Trim(), valueInt);
                    }
                }
            }
        }

        /// <summary>
        /// This method retrieves sir values stored in sir.csv file as a string per line
        /// Checks that the sir code is formatted correctly then stores the string in a List
        /// </summary>
        public void RetrieveSIRFromCSV()
        {
            using (var sirCSVReader = new StreamReader(@".\sir.csv"))
            {
                while (!sirCSVReader.EndOfStream)
                {
                    var readline = sirCSVReader.ReadLine();

                    if (readline[3].ToString().Equals("-") && readline[7].ToString().Equals("-"))
                    {
                        sirList.Add(readline.ToString());
                    }
                }
            }
        }
        
        public void FindSIR(string processedText)
        {
            GetSortCodeAndIncident(processedText);
            AddSIRListToCSV();
        }

        
        public void FindHashtagsAndMentions(string processedText)
        {
            FindMentions(processedText);
            FindHashTags(processedText);
            AddDictionaryOfHashtagsToCSV();
            AddDictionaryOfMentionsToCSV();
        }

       
     
        private void ProcessedSms(ref string processedText)
        {
            FindAndReplaceTextSpeakAbbreviations(ref processedText);
        }

      

       
        private void ProcessedEmail(ref string processedText, string subject)
        {
            FindURL(ref processedText);

            if (subject.Trim().StartsWith("SIR"))
            {
                ConfigureSIR(ref processedText);
            }
        }

    
        private void ConfigureSIR(ref string processedText)
        {
            // Splits the string into a string array
            string[] splitProcessedText = processedText.Trim().Split(' ');

            // Finds the word Nature in the string then replaces it with \nNature so that a new line is created before Nature
            if (splitProcessedText[3].Equals("Nature"))
            {
                splitProcessedText[3] = $"\n{splitProcessedText[3]}";
            }

            // Formats for Nature of Incident with 1 words
            if (splitProcessedText[6].Equals("Raid") || splitProcessedText[6].Equals("Terrorism") || splitProcessedText[6].Equals("Intelligence") || splitProcessedText[6].Equals("Theft")) 
            {
                if (splitProcessedText.Length > 7)
                {
                    splitProcessedText[7] = $"\n{splitProcessedText[7]}";
                }
            }

            // Formats for Nature of Incident with 2 words
            if (splitProcessedText[6].Equals("Staff") || splitProcessedText[6].Equals("Customer") || splitProcessedText[6].Equals("ATM") ||
                splitProcessedText[6].Equals("Bomb") || splitProcessedText[6].Equals("Suspicious") || splitProcessedText[6].Equals("Cash"))  
            {
                if (splitProcessedText.Length > 8)
                {
                    splitProcessedText[8] = $"\n{splitProcessedText[8]}";
                }
            }

            

            // Appends all the values stored in the string array
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string value in splitProcessedText)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(' ');
            }

            processedText = stringBuilder.ToString();
        }

   
        private void FindURL(ref string processedText)
        {
            UrlAttribute url = new UrlAttribute();

            string[] splitProcessedText = processedText.Split(' ');

            for (int i = 0; i < splitProcessedText.Length; i++)
            {
                if (url.IsValid(splitProcessedText[i]) || splitProcessedText[i].StartsWith("www."))
                {
                    quarantineList.Add(splitProcessedText[i]);
                    splitProcessedText[i] = "'<URL Quarantined>'";
                }
            }

            // Appends all the values stored in the string array
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string value in splitProcessedText)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(' ');
            }

            processedText = stringBuilder.ToString();
        }

     
        private void GetSortCodeAndIncident(string processedText)
        {
            string[] splitProcessedText = processedText.Trim().Split(' ');
            string[] natureOfIncident = {"Staff Attack",  "Raid","Theft" , "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Cash Loss", "Intelligence", "ATM Theft" };

            string incident = string.Empty;
            string code = string.Empty;

            // Checks the array index 3 to confirm the length of the value stored is 8 characters long
            if (!splitProcessedText[2].ToString().Trim().Count().Equals(8))
            {
                MessageBox.Show("The Sort Code you have entered is incorrect.");
                return;
            }
            else
            {
                code = splitProcessedText[2];
            }


            if (splitProcessedText[7].Equals("Staff"))
            {
                if (splitProcessedText[8].Equals("Attack"))
                {
                    incident = "Staff Attack";
                }
                else
                {
                    incident = "Staff Abuse";
                }
            }
            else
            {
                foreach (string str in natureOfIncident)
                {
                    if (str.StartsWith(splitProcessedText[6]))
                    {
                        incident = str;
                        break;
                    }
                }
            }
            
            string stringInput = ($"[{code}] - [{incident}]");

            // Adds the string to the sirList, only if the string does not already exist on the list
            if (!sirList.Contains(stringInput))
            {
                sirList.Add(stringInput);
            }
            else
            {
                MessageBox.Show("The Sort Code and Nature of Incident already exist on the SIR list.");
                return;
            }
        }

  

      
        private void ProcessedTweet(ref string processedText)
        {
            FindAndReplaceTextSpeakAbbreviations(ref processedText);
        }

       
        private void FindHashTags(string processedText)
        {
            string[] splitProcessedText = processedText.Split(' ');
            bool hashtagOccurs = false;

            for (int i = 0; i < splitProcessedText.Length; i++)
            {
                hashtagOccurs = false;

                if (splitProcessedText[i].StartsWith("#"))
                {
                    for (int q = 0; q < DictionaryHashtags.Count; q++)
                    {
                        var hashtagElement = DictionaryHashtags.ElementAt(q);

                        if (hashtagElement.Key.Equals(splitProcessedText[i]))
                        {
                            DictionaryHashtags[splitProcessedText[i]] = DictionaryHashtags[splitProcessedText[i]] + 1;
                            MessageBox.Show("Updated a hashtag entry");
                            hashtagOccurs= true;
                        }
                    }

                    if (hashtagOccurs.Equals(false))
                    {
                        DictionaryHashtags.Add(splitProcessedText[i], 1);
                        MessageBox.Show("Found a hashtag");
                    }
                }
            }
        }

        
        private void FindMentions(string processedText)
        {
            string[] splitProcessedText = processedText.Split(' ');
            bool mentionOccurs = false;


            for (int i = 0; i < splitProcessedText.Length; i++)
            {
                mentionOccurs = false;

                if (splitProcessedText[i].StartsWith("@"))
                {

                    for (int w = 0; w < DictionaryMentions.Count; w++)
                    {
                        var mentionElement = DictionaryMentions.ElementAt(w);

                        if (mentionElement.Key.Equals(splitProcessedText[i]))
                        {
                            DictionaryMentions[splitProcessedText[i]] = DictionaryMentions[splitProcessedText[i]] + 1;
                            MessageBox.Show("Updated a mention entry");
                            mentionOccurs = true;
                        }
                    }

                    if (mentionOccurs.Equals(false))
                    {
                        DictionaryMentions.Add(splitProcessedText[i], 1);
                        MessageBox.Show("Found a mention");
                    }
                }
            }
        }


    


        private void FindAndReplaceTextSpeakAbbreviations(ref string processedText)
        {
            string[] splitProcessedText = processedText.Trim().Split(' ');
            bool set = false;

            for (int i = 0; i < splitProcessedText.Length; i++)
            {
                foreach (KeyValuePair<string, string> abbrev in DictionaryTextWords)
                {
                    if (splitProcessedText[i].Equals(abbrev.Key))
                    {
                        splitProcessedText[i] = ($"{abbrev.Key.Trim()} <{abbrev.Value.Trim()}>");
                        set = true;
                    }

                    if (splitProcessedText[i].StartsWith(abbrev.Key) && set.Equals(false))
                    {
                        if (!splitProcessedText[i].All(char.IsLetter))
                        {
                            string miscChar = splitProcessedText[i][(splitProcessedText[i].Count() - 1)].ToString();
                            splitProcessedText[i] = ($"{abbrev.Key.Trim()} <{abbrev.Value.Trim()}>{miscChar}");
                        }
                    }
                }
            }

            // Appends all the values stored in the string array
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string value in splitProcessedText)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(' ');
            }

            processedText = stringBuilder.ToString();
        }



       
        private void AddDictionaryOfHashtagsToCSV()
        {
            using (var write = new StreamWriter(@".\hashtags.csv"))
            {
                foreach (var pair in DictionaryHashtags)
                {
                    write.WriteLine($"{pair.Key},{pair.Value}");
                }
            }
        }


      
        private void AddDictionaryOfMentionsToCSV()
        {
            using (var write = new StreamWriter(@".\mentions.csv"))
            {
                foreach (var pair in DictionaryMentions)
                {
                    write.WriteLine($"{pair.Key},{pair.Value}");
                }
            }
        }

        
        private void AddSIRListToCSV()
        {
            using (var write = new StreamWriter(@".\sir.csv"))
            {
                foreach (var entry in sirList)
                {
                    write.WriteLine($"{entry}");
                }
            }
        }

  
    }
}
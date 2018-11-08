using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace MessageBank
{
    public class Validations
    {
        bool sms = false;
        bool email = false;
        bool tweet = false;

        string header;

        public List<Messages> messagesList = new List<Messages>();

        Json json = new Json();

        





        public bool InputMessageCheck(string inputText)
        {
            // Stores the first character of inputText as the string _inputText
            string inputFirstChar = inputText[0].ToString();

            // Trims the input then stores it as the string numeric
            header = inputText.Trim();

            // Stores a substring of header as the string subheader
            string subHeader = header.Substring(1, 9);

            // Checks the length of the input
            if (!(header.Length).Equals(10))
            {
                return false;
            }

            // Checks the substring is all numbers
            if (subHeader.All(char.IsDigit).Equals(false))
            {
                return false;
            }




            if (inputFirstChar.ToUpper().Equals("S"))
            {
                sms = true;
                return true;
            }

            if (inputFirstChar.ToUpper().Equals("T"))
            {
                tweet = true;
                return true;
            }

            if (inputFirstChar.ToUpper().Equals("E"))
            {
                email = true;
                return true;
            }

            

            return false;
        }

        public bool InputBodyCheck(string inputText)
        {
            string sender = string.Empty;

            if (sms.Equals(true))
            {
                string smsID = @"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$";

                Regex myRegex = new Regex(smsID, RegexOptions.None);

                sender = myRegex.Replace(inputText, string.Empty);

                MessageBox.Show(sender);

                Messages message = new Messages()
                {
                    Header = header,
                    Sender = sender,
                    Text = inputText
                };


                // Adds sms to the list
                AddMessageMethod(header, sender, inputText);

                MessageBox.Show("SMS Saved");

                return true;
            }

            if (email.Equals(true))
            {
                string emailID = @"[A-Za-z0-9_\-\+]+@[A-Za-z0-9\-]+\.([A-Za-z]{2,3})(?:\.[a-z]{2})?";
                Regex myRegex = new Regex(emailID, RegexOptions.None);

                Match myMatch = myRegex.Match(inputText);

                if (myMatch.Success)
                {
                    sender = myMatch.Value;
                }

                // Removes the email address from the string and replaces it with a '-'
                string newInputText = myRegex.Replace(inputText, "-");

                // Splits the string in 2 based on the delimiter '-'
                string[] splitText = newInputText.Split('-');

                // Checks the email doesn't exceed the maximum length
                if (splitText[1].Length > 1048)
                {
                    MessageBox.Show("This email is longer than 1028 max characters");
                    return false;
                }

                // Creates the string name from the part of splitText before the '-'
                string eName = splitText[0];

                // Creates substrings from newInputText
                string eSubject = splitText[1].Substring(0, 21);
                string eText = splitText[1].Substring(21);


                //Adds email to the list
                AddMessageMethodEmail(header, sender, eSubject, eText);

                MessageBox.Show("Email Saved");

                return true;
            }


            // TWEET
            if (tweet.Equals(true))
            {
                string tweetID = @"^((@\w+)\s)+";

                Regex regex = new Regex(tweetID, RegexOptions.None);

                Match match = regex.Match(inputText);

                if (match.Value.Length > 17)
                {
                    return false;
                }

                if (match.Success)
                {
                    sender = match.Value;
                }


                // Removes the Twitter ID  from the string, leaving the tweet.
                string tweetText = regex.Replace(inputText, string.Empty);


                if (tweetText.Length > 140)
                {
                    return false;
                }

                // Adds tweet to the list
                AddMessageMethod(header, sender, tweetText);

                MessageBox.Show("Tweet Saved");


                return true;

            }

            return false;

        }

        // Adds SMS and Tweets to the list
        private void AddMessageMethod(string header, string sender, string text)
        {
            Messages message = new Messages()
            {
                Header = header,
                Sender = sender,
                Text = text
            };

            messagesList.Add(message);

            MessageBox.Show("File added to the list.");


        }


        // Adds Email to the list
        private void AddMessageMethodEmail(string header, string sender, string subject, string text)
        {
            Messages message = new Messages()
            {
                Header = header,
                Sender = sender,
                Subject = subject,
                Text = text
            };

            messagesList.Add(message);

            MessageBox.Show("File added to the list.");
        

        }


        public void LoadStoredList()
        {
            try
            {
                // Returns the list that is stored as JSON             
                messagesList = json.Deserialize();
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.ToString());
            }

        }




        #region User Selection Validations

        //Do you want to exit the application
        public void ExitAppValidation()
        {
            MessageBoxResult yesOrNo = MessageBox.Show("Do you want to Exit?", "Exit Application", MessageBoxButton.YesNo);

            if (yesOrNo == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }

            else
            {
                return;
            }
        }

        #endregion
    }
}
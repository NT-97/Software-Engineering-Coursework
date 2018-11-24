

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel.DataAnnotations;


#endregion

namespace MessageBank
{
    public class Validations
    {
        #region Objects / Data Structure / Variables

        Json json = new Json();

        public List<Messages> messagesList = new List<Messages>();

        bool smsMessage = false;
        bool emailMessage = false;
        bool tweetMessage = false;

        string header = string.Empty;
        string sender = string.Empty;
        string text = string.Empty;
        string subject = string.Empty;
       

        public string Header = string.Empty;
        public string Sender = string.Empty;
        public string Subject = string.Empty;
        public string Text = string.Empty;

        #endregion

        #region Constructor

        // Default Constructor
        public Validations()
        {

        }

        #endregion

        #region Public Methods


        public bool MessageHeaderValidation(string textInput)
        {
            // Trims the input then stores it as the string header
            header = textInput.Trim();

            // Checks the length of the input
            if (!(header.Length).Equals(10))
            {
                return false;
            }

            // Stores a substring of numeric as the string subStringNumeric
            string numericSubString = header.Substring(1, 9);

            // Checks the substring is all numbers
            if (numericSubString.All(char.IsDigit).Equals(false))
            {
                return false;
            }

            // Stores the first character of inputText as the string _inputText
            string _textInput = textInput[0].ToString();


            if (_textInput.ToUpper().Equals("S"))
            {
                smsMessage = true;
                return true;
            }
            else if (_textInput.ToUpper().Equals("E"))
            {
                emailMessage = true;
                return true;
            }
            else if (_textInput.ToUpper().Equals("T"))
            {
                tweetMessage = true;
                return true;
            }
            else
            {
                MessageBox.Show("The header is incorrect. Please check and try again.");
            }

            return false;
        }

        
        public bool MessageBodyValidation(string textInput)
        {
            EmailAddressAttribute emailCheck = new EmailAddressAttribute();
            PhoneAttribute PhoneCheck = new PhoneAttribute();
            
            
            //SMS
            if (smsMessage.Equals(true))
            {
                bool smsCheck = true;
                smsMessage = false;
                
                while (smsCheck)
                {
                    if (textInput.Length > 20)
                    {
                        for (int i = 20; i > 7; i--)
                        {
                            sender = textInput.Trim().Substring(0, i);
                            
                            if (PhoneCheck.IsValid(sender))
                            {
                                text = textInput.Trim().Substring(i);
                                smsCheck = false;
                                break;
                            }
                        }
                    }
                    else if (textInput.Length > 7 && textInput.Length < 21)
                    {
                        for (int i = textInput.Length; i > 7; i--)
                        {
                            sender = textInput.Trim().Substring(0, i);
                            
                            if (PhoneCheck.IsValid(sender))
                            {
                                text = textInput.Trim().Substring(i);
                                smsCheck = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("The phone number entered, is not a valid phone number.");
                        return false;
                    }
                }

                // Checks the length of the SMS
                if (text.Length > 140)
                {
                    MessageBox.Show("The SMS is too long. It can only be upto a maximum of 140 characters");
                    return false;
                }

                SetGlobalVariable();
                return true;
            }

            //Email
            if (emailMessage.Equals(true))
            {
                emailMessage = false;

                string[] splitProcessedtext = textInput.Trim().Split(' ');

                
                string[] SubjectAndTextArray = { };

                string SubjectAndText = string.Empty;
                bool emailAddressSpotted = false;

                for (int i = 0; i < splitProcessedtext.Length; i++)
                {
                    if (splitProcessedtext[i].Contains("@"))
                    {
                        // Checks if the email is a valid email address
                        if (emailCheck.IsValid(splitProcessedtext[i]))
                        {
                            emailAddressSpotted = true;
                            sender = splitProcessedtext[i];
                            splitProcessedtext[i] = "";

                            
                            SubjectAndTextArray = splitProcessedtext.Skip(i).ToArray();

                            break;
                        }
                    }
                }

                if (emailAddressSpotted.Equals(false))
                {
                    MessageBox.Show("You have entered an incorrect email address.");
                    return false;
                }

                

                // Concatenate all the elements into a StringBuilder.
                StringBuilder subjectAndTextBuilder = new StringBuilder();
                foreach (string value in SubjectAndTextArray)
                {
                    subjectAndTextBuilder.Append(value);
                    subjectAndTextBuilder.Append(' ');
                }










                SubjectAndText = subjectAndTextBuilder.ToString().Trim();

                if (SubjectAndText.ToUpper().StartsWith("SIR"))
                {
                    // Creates substrings from newInputText
                    subject = SubjectAndText.Trim().Substring(0, 12);
                    text = SubjectAndText.Trim().Substring(12);
                }
                else
                {
                    // Creates substrings from newInputText
                    subject = SubjectAndText.Trim().Substring(0, 20);
                    text = SubjectAndText.Trim().Substring(20);
                }

                // Checks the email doesn't exceed the maximum length
                if (text.Length > 1048)
                {
                    MessageBox.Show("This email is longer than 1028 max characters");
                    return false;
                }

                SetGlobalVariable();
                return true;
            }

            //Tweet
            if (tweetMessage.Equals(true))
            {
                tweetMessage = false;

                string[] splitProcessedText = textInput.Trim().Split(' ');

                if (splitProcessedText[0].StartsWith("@") && splitProcessedText[0].Length < 16)
                {
                    sender = splitProcessedText[0];
                    splitProcessedText[0] = string.Empty;
                }
                else
                {
                    MessageBox.Show("You have entered an incorrect Twitter ID.\nPlease check and try again.");
                    return false;
                }

                // Concatenate all the elements into a StringBuilder.
                StringBuilder stringBuilder = new StringBuilder();
                foreach (string value in splitProcessedText)
                {
                    stringBuilder.Append(value);
                    stringBuilder.Append(' ');
                }

                text = stringBuilder.ToString().Trim();

                // Checks that the tweet text length is less than 140 characters
                if (text.Length > 140)
                {
                    MessageBox.Show("The tweet text is more than 140 characters in length.");
                    return false;
                }

                SetGlobalVariable();
                return true;
            }

            return false;
        }

        
        public void ResetStringValues()
        {
            header = string.Empty;
            sender = string.Empty;
            text = string.Empty;
            subject = string.Empty;
          

            Header = string.Empty;
            Sender = string.Empty;
            Subject = string.Empty;
            Text = string.Empty;
        }

     
        public void AddMessageToList(string inputText)
        {
            Messages message = new Messages()
            {
                Header = header,
                Sender = sender,
                Subject = subject,
                Text = inputText
            };

            messagesList.Add(message);
            header = sender = subject = text = string.Empty;
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

        #region Private Method

        
        private void SetGlobalVariable()
        {
            Header = header;
            Sender = sender;
            Subject = subject;
            Text = text;
        }

        #endregion

        #region User's Choice Validation Methods      

        public void ExitApplicationValidation()
        {
            MessageBoxResult yesOrNo = MessageBox.Show("Are you sure you want to exit the application?", "Exit Application", MessageBoxButton.YesNo);

            if (yesOrNo == MessageBoxResult.Yes)
            {
                File.WriteAllText(@".\hashtags.csv", String.Empty);
                File.WriteAllText(@".\mentions.csv", String.Empty);
                File.WriteAllText(@".\sir.csv", String.Empty);

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

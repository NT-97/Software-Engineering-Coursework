using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessageBank
{
    public class Validations
    {
        bool sms = false;
        bool email = false;
        bool tweet = false;


        // Default Constructor
        public Validations()
        {

        }






        public bool HeaderInputValidation(string textInput)
        {
            // Stores the first character of inputText as the string _inputText
            string text_Input = textInput[0].ToString();

            // Trims the input then stores it as the string numeric
            string numeric = textInput.Trim();

            // Stores a substring of numeric as the string subStringNumeric
            string subStringNumeric = numeric.Substring(1, 9);

            // Checks the length of the input
            if (!(numeric.Length).Equals(10))
            {
                return false;
            }

            // Checks the substring is all numbers
            if (subStringNumeric.All(char.IsDigit).Equals(false))
            {
                return false;
            }




            if (text_Input.ToUpper().Equals("S"))
            {
                sms = true;
                return true;
            }

            if (text_Input.ToUpper().Equals("E"))
            {
                email = true;
                return true;
            }

            if (text_Input.ToUpper().Equals("T"))
            {
                tweet = true;
                return true;
            }

            return false;
        }

        public bool MessageBodyInputValidation(string textInput)
        {
            if (sms.Equals(true))
            {

            }

            if (email.Equals(true))
            {

            }

            if (tweet.Equals(true))
            {

            }

            return false;
        }




        #region User Selection Validations

        //Do you want to exit the application
        public void ExitAppValidation()
        {
            MessageBoxResult yesOrNo = MessageBox.Show("Are you sure you want to exit the application?", "Exit Application", MessageBoxButton.YesNo);

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
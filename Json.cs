using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessageBank
{
    public class Json
    {

        public void Serialize(List<Messages> list)
        {
            try
            {
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(@".\MessageBank.json", JsonConvert.SerializeObject(list, Formatting.Indented));

                // Message informing the user that the file has been saved successfully
                MessageBox.Show("JSON File saved.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public List<Messages> Deserialize()
        {
            // read file into a string and deserialize JSON to a type
            List<Messages> storedListOfMessages = JsonConvert.DeserializeObject<List<Messages>>(File.ReadAllText(@".\MessageBank.json"));

            return storedListOfMessages;

        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace C_sharp_Course.Chapter_3
{
    class Chapter_3_1
    {
        public static void Run()
        {
           /* Console.WriteLine("Create a simple, single object in JSON:");
            CreateOneJSON();

            Console.WriteLine("\n\nCreate multiple objects object in JSON:");
           // CreateMultipleJSON();

            Console.WriteLine("\n\nDeserialize JSON back to C#");
            //DeserializeMultipleJSON();*/

            //Console.WriteLine("\n\nSerialize a list of objects in XML");
            //SerializeMultipleXML();

            //Console.WriteLine("\n\nValidate JSON (catch invalid format)");
            //CatchJSONError();

            //Console.WriteLine("\n\nProvide copies of objects");
            //ProvideCopyOfObject();

            //Console.WriteLine("\n\nReplace characters using Regex");
            //RegexReplace();

            //Console.WriteLine("\n\nCheck if input matches regex");
            //RegexMatchMultiple();

            //Console.WriteLine("\n\nShows 3 examples of value converting");
            ConvertingValues();
        }

        private static void CreateOneJSON()
        {
            MovieCharacter character = new MovieCharacter("Anakin Skywalker", "Star Trek", 8);
            string json = JsonConvert.SerializeObject(character);
            Console.WriteLine(json);
        }

        private static void CreateMultipleJSON()
        {
            List<MovieCharacter> characterList = new List<MovieCharacter>
            {
                new MovieCharacter("Batman", "Avengers: Infinity War", 8),
                new MovieCharacter("Dora the Explorer", "Final Destination", 6),
                new MovieCharacter("Uncle Ben", "Superman", 10)
            };

            string json = JsonConvert.SerializeObject(characterList);
            Console.WriteLine(json);
        }

        private static void DeserializeMultipleJSON()
        {
            List<MovieCharacter> characterList = new List<MovieCharacter>
            {
                new MovieCharacter("Batman", "Avengers: Infinity War", 8),
                new MovieCharacter("Dora the Explorer", "Final Destination", 6),
                new MovieCharacter("Uncle Ben", "Superman", 10)
            };

            string json = JsonConvert.SerializeObject(characterList);

            List<MovieCharacter> deserialized = JsonConvert.DeserializeObject<List<MovieCharacter>>(json);
            foreach (MovieCharacter character in deserialized)
            {
                Console.WriteLine(character.Name);
            }
        }

        private static void SerializeMultipleXML()
        {
            List<MovieCharacter> characterList = new List<MovieCharacter>
            {
                new MovieCharacter("Batman", "Avengers: Infinity War", 8),
                new MovieCharacter("Dora the Explorer", "Final Destination", 6),
                new MovieCharacter("Uncle Ben", "Superman", 10)
            };

            XmlSerializer serializer = new XmlSerializer(typeof(List<MovieCharacter>));
            TextWriter writer = new StringWriter();
            serializer.Serialize(writer, characterList);
            writer.Close();

            // For deserialization: Streamreader instead of Streamwriter, and call Deserialize(); -> https://stackoverflow.com/questions/364253/how-to-deserialize-xml-document

            string characterXML = writer.ToString();
            Console.WriteLine(characterXML);
        }

        private static void CatchJSONError()
        {
            // The 'Rating' is missing a double quotation
            string invalidJson = "{\"Name\":\"Mustafa\",\"Movie\":\"Peter Pan\",\"Rating\":5\"}";
            try
            {
                MovieCharacter character = JsonConvert.DeserializeObject<MovieCharacter>(invalidJson);
                Console.WriteLine(character.Name);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ProvideCopyOfObject()
        {
            MovieCharacter character = new MovieCharacter("Anakin Skywalker", "Star Trek", 8);
            MovieCharacter copiedCharacter = new MovieCharacter(character);
            copiedCharacter.Name = "Not Anakin Skywalker";
            Console.WriteLine(character.Name);
            Console.WriteLine(copiedCharacter.Name);
        }

        private static void RegexReplace()
        {
            string input = "A,,,,B,C,D,E,F,G,H,I,J,K,L";
            //string regexToMatch = ",";
            string regexToMatch = ",+"; // The '+' symbol means 'match 1 or more'
            string patternToReplace = " ";

            string replaced = Regex.Replace(input, regexToMatch, patternToReplace);
            Console.WriteLine(replaced);
        }

        private static void RegexMatchMultiple()
        {
            string input = "Jan:Peter:Balkenende";
            // Match 1 or more characters, 3 times
            string regexToMatch = ".+:.+:.";
            if (Regex.IsMatch(input, regexToMatch))
            {
                Console.WriteLine("Valid input");
            }

            string invalidInput = "Jan:Peter";
            if (!Regex.IsMatch(invalidInput, regexToMatch))
            {
                Console.WriteLine("Invalid input");
            }
        }

        private static void ConvertingValues()
        {
            // Parse -> throws exception when not possible
            try
            {
                string value = "99";
                int parsedIntValue = Int32.Parse(value);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            // TryParse -> returns false when not possible
            if (int.TryParse("10", out int result))
                Console.WriteLine($"{result} is a Valid Number");
            else
                Console.WriteLine("Invalid Number");

            // Convert.ToInt32 -> throws exception when not possible, but not when null
            string stringValue = "99";
            int intValue = Convert.ToInt32(stringValue);
            Console.WriteLine(intValue);

            string nullValue = null;
            int nullIntValue = Convert.ToInt32(nullValue);
            Console.WriteLine(nullIntValue);
        }
    }
}

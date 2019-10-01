using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace C_sharp_Course.Chapter_3
{
   public class MovieCharacter
    {
        public string Name { get; set; }
        [XmlElement(ElementName = "MovieTitle")]
        public string Movie { get; set; }
        [JsonProperty(PropertyName = "score")]
        public int Rating { get; set; }

        public MovieCharacter()
        {
            // Parameterless constructor is necessary for XML serialization.
        }

        public MovieCharacter(string name, string movie, int rating)
        {
            this.Name = name;
            this.Movie = movie;
            this.Rating = rating;
        }
        
        // Providers a copy of the data object to preserve data integrity.
        public MovieCharacter(MovieCharacter source)
        {
            Name = source.Name;
            Movie = source.Movie;
            Rating = source.Rating;
            
        }
    }
}

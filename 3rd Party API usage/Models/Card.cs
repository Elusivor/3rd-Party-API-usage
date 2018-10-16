using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rd_Party_API_usage.Models
{
    public class Card
    {
        public int ID { get; set; }

        public string Image { get; set; } // URL of image
        public string Value { get; set; } // the value. eg. king, queen, jack, 10, etc
        public string Suit { get; set; } // Hearts, Diamonds, Spades, Clubs
        public string Code { get; set; } // short code, Value and Suit. Eg: 5H, JC
    }
}
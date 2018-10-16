using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rd_Party_API_usage.Models
{
    public class Deck
    {
        public int ID { get; set; }

        public string Deck_ID { get; set; } //The Deck ID used with our API
        public int Remaining { get; set; } // the remaining amount of cards in our deck, updated trough API
        public int? LastCardID { get; set; } // An ID of our card that was last drawn
        public Card LastCard { get; set; } // OUr LAst card
    }

}
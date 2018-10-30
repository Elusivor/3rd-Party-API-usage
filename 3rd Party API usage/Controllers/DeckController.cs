using _3rd_Party_API_usage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace _3rd_Party_API_usage.Controllers
{
    public class DeckController : Controller
    {

        private DB _context;
        private HttpClient api;

        public DeckController()
        {
            _context = new DB();
            api = new HttpClient()
            {
                BaseAddress = new Uri("https://deckofcardsapi.com/api/deck/")
            };
        }
        // GET: Deck
        public ActionResult List()
        {
            var decks = _context.Decks.Include(x=>x.LastCard).ToList();
            return View(decks);
        }
        public async Task<ActionResult> New()
        {
            var response = await api.GetAsync("new/shuffle");
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (json["success"].ToObject<bool>())
            {
                Deck newDeck = new Deck()
                {
                    Remaining = int.Parse(json["remaining"].ToString()),
                    Deck_ID = json["deck_id"].ToString()
                };
                _context.Decks.Add(newDeck);
                _context.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                return Content("Error generating new deck, please try again");
            }
        }

        public ActionResult Deck(string deckID, string errMsg = null)
        {
            Deck Deku = _context.Decks.Include(x => x.LastCard).SingleOrDefault(x => x.Deck_ID == deckID);
            if (Deku != null)
            {
                DeckDisplayViewModel viewModel = new DeckDisplayViewModel()
                {
                    Deck = Deku,
                    Error = errMsg
                };
                return View(viewModel);
            }
            else
            {
                return Content("[Deck Not Found] Error loading deck, please try again later.");
            }
        }

        public async Task<ActionResult> Draw(string deckID)
        {
            Deck Deku = _context.Decks.Include(x => x.LastCard).SingleOrDefault(x => x.Deck_ID == deckID);
            HttpResponseMessage response = await api.GetAsync($"{deckID}/draw/"); //Example: "6sfain0sefd7/draw/"
            JObject jason = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (bool.Parse(jason["success"].ToString()) & jason["deck_id"].ToString() == deckID)
            { //Checks whether or not our API Request was successful and check that we have been returned our Deck ID
                string jasoncode = jason["cards"][0]["code"].ToString();
                Card card = _context.Cards.SingleOrDefault(x => x.Code == jasoncode);
                if (card == null) //If our card is not in our Database
                {
                    card = new Card //Create a new card
                    {
                        Code = jason["cards"][0]["code"].ToString(),
                        Value = jason["cards"][0]["value"].ToString(),
                        Suit = jason["cards"][0]["suit"].ToString(),
                        Image = jason["cards"][0]["images"]["png"].ToString()
                    };
                    _context.Cards.Add(card);
                    _context.SaveChanges();
                    _context.Entry(card).GetDatabaseValues();
                }
                Deku.Remaining = jason["remaining"].ToObject<int>();
                Deku.LastCard = card;
                _context.Decks.AddOrUpdate(Deku);
                _context.SaveChanges();

                return RedirectToAction("Deck", new { deckID });
            }
            else
            {
                if (jason["deck_id"].ToString() != deckID)
                {
                    return Content("[Deck ID Mismatch] Error loading this deck, please try again");
                }
                else if (bool.Parse(jason["success"].ToString()) == false)
                {
                    if (int.Parse(jason["remaining"].ToString()) <= 0)
                    {
                        return RedirectToAction("Deck", new { deckID, errMsg = jason["error"].ToString() });
                    }
                    else
                    {
                        return Content("[Not Successful] Error loading this deck, please try again");
                    }
                }
                else
                {
                    return Content("[Unknown Error] Error loading this deck, please try again");
                }
            }
        }

        public async Task<ActionResult> Reshuffle(string deckID)
        {
            Deck Deku = _context.Decks.Include(x => x.LastCard).SingleOrDefault(x => x.Deck_ID == deckID);
            HttpResponseMessage response = await api.GetAsync($"{deckID}/draw/"); //Example: "6sfain0sefd7/draw/"
            JObject jason = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (bool.Parse(jason["success"].ToString()) & jason["deck_id"].ToString() == deckID)
            { //Checks whether or not our API Request was successful and check that we have been returned our Deck ID
                Deku.Remaining = int.Parse(jason["remaining"].ToString());
                Deku.LastCard = null;
                Deku.LastCardID = null;
                _context.SaveChanges();
                return RedirectToAction("Deck", new { deckID });
            }
            else
            {
                return Content("[Not Successful] There was an error shuffling the deck please try again.");
            }
        }
    }
}
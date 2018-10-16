using _3rd_Party_API_usage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
            var decks = _context.Decks.ToList();
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
    }
}
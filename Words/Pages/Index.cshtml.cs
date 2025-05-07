using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Words.Services;

namespace Words.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICheckWords _checkWords;

        public IndexModel(ILogger<IndexModel> logger, ICheckWords checkWords)
        {
            _logger = logger;
            _checkWords = checkWords;
        }

        public string RandomWord
        {
            get => HttpContext.Session.GetString("RandomWord");
            set => HttpContext.Session.SetString("RandomWord", value);
        }
        public List<char> CorrectGuesses
        {
            get => HttpContext.Session.GetString("CorrectGuesses") != null
                ? JsonSerializer.Deserialize<List<char>>(HttpContext.Session.GetString("CorrectGuesses"))
                : new List<char>();
            set => HttpContext.Session.SetString("CorrectGuesses", JsonSerializer.Serialize(value));
        }
        public List<char> IncorrectGuesses
        {
            get => HttpContext.Session.GetString("IncorrectGuesses") != null
                ? JsonSerializer.Deserialize<List<char>>(HttpContext.Session.GetString("IncorrectGuesses"))
                : new List<char>();
            set => HttpContext.Session.SetString("IncorrectGuesses", JsonSerializer.Serialize(value));
        }
        [BindProperty]
        public bool GameStarted { get; set; }
        [BindProperty]
        public bool GuessMade { get; set; } = false;
        [BindProperty]
        public string Guess { get; set; }  = string.Empty;
        public int NumberOfLetters { get; set; }
        [TempData]
        public string? Message { get; set; }
        [TempData]
        public bool HasWon { get; set; }



        public void OnGet()
        {
            
        }

        public void OnPostStartGame()
        {
            RandomWord = _checkWords.GenerateRandomWord();
            CorrectGuesses = new List<char>();
            IncorrectGuesses = new List<char>();
            if (_checkWords.IsGuessed(RandomWord)) _checkWords.GenerateRandomWord();
            GameStarted = true;
        }

        public IActionResult OnPostMakeGuess()
        {
            GuessMade = true;
            var goodGuesses = CorrectGuesses;
            var badGuesses = IncorrectGuesses;
            var ord = RandomWord;
           

            if (!string.IsNullOrEmpty(Guess) && _checkWords.IsLetterSingle(Guess))
            {
                var guess = Guess.ToLower();
                var ch = guess[0];

                if (goodGuesses.Contains(ch) || badGuesses.Contains(ch))
                {
                    Message = $"Du har redan gissat på '{ch}'!";
                }
                else if (ord.Contains(ch))
                {
                    goodGuesses.Add(ch);
                    CorrectGuesses = goodGuesses;
                    Message = "Bra gissat!";
                    // Kolla om alla bokstäver är gissade
                    if (IsWordGuessed(ord, goodGuesses))
                    {
                        HasWon = true;
                        Message = $"Grattis! Du klarade ordet \"{ord}\". Vill du spela igen?";
                    }
                }
                else
                {
                    badGuesses.Add(ch);
                    IncorrectGuesses = badGuesses;
                    Message = "Fel gissat!";
                    _checkWords.AddIncorrectGuess(guess, ord);
                }
            }
            else
            {
                Message = "Skriv EN bokstav!";
            }

            return RedirectToPage();
        }
        private bool IsWordGuessed(string word, List<char> guesses)
        {
            return word.All(c => guesses.Contains(c));
        }
        public IActionResult OnPostQuitGame()
        {
            HttpContext.Session.Remove("RandomWord");
            HttpContext.Session.Remove("CorrectGuesses");
            HttpContext.Session.Remove("IncorrectGuesses");
            // Eventuellt visa ett tack-meddelande
            return RedirectToPage();
        }
    }
}

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
            get => HttpContext.Session.GetString("RandomWord") ?? "";
            set => HttpContext.Session.SetString("RandomWord", value);
        }
        public List<char> CorrectGuesses
        {
            get
            {
                var json = HttpContext.Session.GetString("CorrectGuesses");
                return json != null ? JsonSerializer.Deserialize<List<char>>(json) ?? new List<char>() : new List<char>();
            }
            set => HttpContext.Session.SetString("CorrectGuesses", JsonSerializer.Serialize(value));

        }
     
        public List<char> IncorrectGuesses
        {
            get
            {
                var json = HttpContext.Session.GetString("IncorrectGuesses");
                return json != null ? JsonSerializer.Deserialize<List<char>>(json) ?? new List<char>() : new List<char>();
            }
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

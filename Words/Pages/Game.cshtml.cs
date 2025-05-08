using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Words.Models;
using Words.Services;

namespace Words.Pages
{
    public class GameModel : PageModel
    {
        private readonly GameService _gameService;

        [BindProperty]
        public string Guess { get; set; }
        public GameViewModel GameVM { get; set; } = new GameViewModel();

        public GameModel(GameService gameService)
        {
            _gameService = gameService;
        }

        public void OnGet()
        {
            GameVM.Stats = _gameService.CheckGameState();
            if (GameVM.Stats == null) GameVM.Stats = _gameService.StartNewGame();
        }

        public IActionResult OnPostNewGame()
        {
            GameVM.Stats = _gameService.StartNewGame();
            return Page();
        }

        //Om användaren vill spela igen, starta ett nytt spel.
        //Om användaren inte vill spela igen, avsluta spelet.

        public IActionResult OnPostGuess()
        {
            GameVM.Stats = _gameService.CheckGameState();


            if (!_gameService.IsValidGuess(Guess[0]))
            {
                GameVM.FeedbackMessage = "Du måste gissa en bokstav!";
            }
            else if (_gameService.IsAlreadyGuessed(Guess[0], GameVM.Stats))
            {
                GameVM.FeedbackMessage = "Du har redan gissat på den bokstaven.";
            }
            else if (_gameService.IsCorrectGuess(Guess[0], GameVM.Stats.CurrentWord))
            {
                GameVM.Stats.CorrectGuesses.Add(char.ToLower(Guess[0]));
                GameVM.FeedbackMessage = "Rätt gissat!";
            }
            else
            {
                GameVM.Stats.WrongGuesses.Add(char.ToLower(Guess[0]));
                GameVM.FeedbackMessage = "Fel gissat!";
            }

            if (_gameService.IsGameWon(GameVM.Stats))
            {
                GameVM.FeedbackMessage = "Grattis, du vann!";
            }
            else if (_gameService.IsGameOver(GameVM.Stats))
            {
                GameVM.FeedbackMessage = $"Spelet är slut. Ordet var: {GameVM.Stats.CurrentWord}";
            }

            _gameService.SaveStatistics(GameVM.Stats);
            ModelState.Clear();
            return Page();
        }
    }
}

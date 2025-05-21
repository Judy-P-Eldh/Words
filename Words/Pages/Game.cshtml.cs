using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Words.Models;
using Words.Services;

namespace Words.Pages
{
    public class GameModel : PageModel
    {
        private readonly GameService _gameService;

        [BindProperty]
        public string Guess { get; set; }

        [BindProperty]
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

        public IActionResult OnPostGuess([FromForm] string action)
        {
            GameVM.Stats = _gameService.CheckGameState();

            // Om spelaren klickade p� "Visa ledtr�d"
            if (action == "hint")
            {
                GameVM.Stats.ShowHint = true;
                _gameService.SaveStatistics(GameVM.Stats);
                ModelState.Clear();
                return Page();
            }

            // Blockera gissning om spelet redan �r slut eller vunnet
            if (GameVM.Stats.IsGameOver)
            {
                GameVM.FeedbackMessage = $"Spelet �r slut. Ordet var: {GameVM.Stats.CurrentWord}";
                return Page();
            }
            if (GameVM.Stats.IsGameWon)
            {
                GameVM.FeedbackMessage = $"Grattis, du vann! Du klarade ordet: {GameVM.Stats.CurrentWord}";
                return Page();
            }

            if (string.IsNullOrEmpty(Guess) || !_gameService.IsValidGuess(Guess[0]))
            {
                GameVM.FeedbackMessage = "Du m�ste gissa en bokstav!";
            }
            else if (_gameService.IsAlreadyGuessed(Guess[0], GameVM.Stats))
            {
                GameVM.FeedbackMessage = "Du har redan gissat p� den bokstaven.";
            }
            else if (_gameService.IsCorrectGuess(Guess[0], GameVM.Stats.CurrentWord))
            {
                GameVM.Stats.CorrectGuesses.Add(char.ToUpper(Guess[0]));
                GameVM.FeedbackMessage = "R�tt gissat!";
            }
            else
            {
                GameVM.Stats.WrongGuesses.Add(char.ToUpper(Guess[0]));
                GameVM.FeedbackMessage = "Fel gissat!";
            }

            // Uppdatera feedback om spelet �r vunnet eller f�rlorat efter gissningen
            if (GameVM.Stats.IsGameWon)
            {
                GameVM.FeedbackMessage = $"Grattis, du vann! Du klarade ordet: {GameVM.Stats.CurrentWord}";
            }
            else if (GameVM.Stats.IsGameOver)
            {
                GameVM.FeedbackMessage = $"Tyv�rr, du f�rlorade.";
            }

            _gameService.SaveStatistics(GameVM.Stats);
            ModelState.Clear();
            GameVM.Guess = null;
            return Page();
        }

        public IActionResult OnPostShowWordGuess()
        {
            GameVM.Stats = _gameService.CheckGameState();
            GameVM.Stats.ShowWordGuess = true;
            _gameService.SaveStatistics(GameVM.Stats);
            return Page();
        }

        public IActionResult OnPostGuessWord()
        {
            GameVM = _gameService.GetCurrentGame(GameVM.WordGuess, null);

            if (_gameService.IsCorrectWordGuess(GameVM.WordGuess, GameVM.Stats.CurrentWord))
            {
                // Hantera vinst: l�gg till alla bokst�ver i CorrectGuesses
                GameVM.Stats.CorrectGuesses = GameVM.Stats.CurrentWord
                    .ToUpper()
                    .Where(char.IsLetter)
                    .Distinct()
                    .ToList();

                GameVM.Stats.ShowWordGuess = false;
                GameVM.FeedbackMessage = $"Grattis, du vann! Ordet var: {GameVM.Stats.CurrentWord}";
            }
            else
            {
                // Hantera endast fel h�r!
                GameVM.Stats.WrongWordGuesses++;
                GameVM.Stats.ShowWordGuess = false;
                GameVM.FeedbackMessage = "Fel ordgissning!";
            }

            _gameService.SaveStatistics(GameVM.Stats);

            return Page();
        }
    }
}

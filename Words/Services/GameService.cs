using System.Text.Json;
using Words.Models;

namespace Words.Services
{
    public class GameService
    {
        private readonly WordGenerator _wordGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameService(IHttpContextAccessor httpContextAccessor, ICheckWords checkWords, WordGenerator wordGenerator)
        {
            _httpContextAccessor = httpContextAccessor;
            _wordGenerator = wordGenerator;
        }

        // 1. Hämta state
        public Statistic? CheckGameState()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = session?.GetString("Statistics");
            if (!string.IsNullOrEmpty(json))
            {
                var stats = JsonSerializer.Deserialize<Statistic>(json);
                return stats;
            }
            return null; // Inget spel pågår
        }

        public GameViewModel GetCurrentGame(string? wordGuess, char? guess)
        {
            // 1. Hämta statistik från sessionen, eller skapa nytt om inget finns
            var stats = CheckGameState();
            if (stats == null)
            {
                StartNewGame();
            }

            // 3. Bygg och returnera din GameViewModel
            return new GameViewModel
            {
                Stats = stats,
                WordGuess = wordGuess,
                Guess = guess
                // GuessesLeft räknas automatiskt utifrån Stats
            };
        }

        public void SaveStatistics(Statistic stats)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString("Statistics", JsonSerializer.Serialize(stats));
        }

        public Statistic StartNewGame()
        {
            WordEntry randomWordEntry;
            do
            {
                randomWordEntry = _wordGenerator.GetRandomWord();
            } while (IsWordGuessed(randomWordEntry.Word));

            var randomWord = randomWordEntry.Word.ToUpper();
            var guesses = randomWord.Length < 5 ? 15 : 10;

            var stats = new Statistic
            {
                CurrentWord = randomWord.ToUpper(),
                Category = randomWordEntry.Category,
                CorrectGuesses = new List<char>(),
                WrongGuesses = new List<char>(),
                MaxGuesses = guesses,
            };

            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString("Statistics", JsonSerializer.Serialize(stats));

            return stats;
        }

        public bool IsWordGuessed(string word)
        {
            return _wordGenerator.GetWordsDone().Any(w => w.Word.Equals(word, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsValidGuess(char guess)
        {
            if (char.IsLetter(guess)) return true;
            else return false;
        }

        public bool IsLetterGuessed(char guess)
        {
            var stats = CheckGameState();
            if (stats.CorrectGuesses.Contains(guess) || stats.WrongGuesses.Contains(guess)) return true;
            else return false;
        }
        public bool IsAlreadyGuessed(char guess, Statistic stats)
        {
            char upper = char.ToUpper(guess);
            if (stats.CorrectGuesses.Contains(upper) || stats.WrongGuesses.Contains(upper)) return true;
            else return false;
        }

        public bool IsCorrectWordGuess(string wordGuess, string randomWord)
        {
            if (randomWord.Equals(wordGuess, StringComparison.OrdinalIgnoreCase)) return true;
            else return false;
        }

        public bool IsCorrectGuess(char guess, string randomWord)
        {
            if (randomWord.Contains(char.ToUpper(guess))) return true;
            else return false;
        }

        public bool IsGameWon(Statistic stats)
        {
            return stats.EntireWordSuccess;
        }

        public bool IsGameOver(Statistic stats)
        {
            return stats.IsGameOver;
        }
    }
}

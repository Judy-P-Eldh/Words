using System.Diagnostics.Eventing.Reader;
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
        public void SaveStatistics(Statistic stats)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString("Statistics", JsonSerializer.Serialize(stats));
        }

        public Statistic StartNewGame()
        {
            string randomWord;
            do
            {
                randomWord = _wordGenerator.GetRandomWord();
            } while (IsWordGuessed(randomWord));

            var guesses = randomWord.Length < 5 ? 15 : 10;

            var stats = new Statistic
            {
                CurrentWord = randomWord,
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
            if (_wordGenerator.GetWordsDone().Contains(word)) return true;
            else return false;
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
            char lower = char.ToLower(guess);
            if (stats.CorrectGuesses.Contains(lower) || stats.WrongGuesses.Contains(lower)) return true;
            else return false;
        }

        public bool IsCorrectGuess(char guess, string randomWord)
        {
            if (randomWord.Contains(char.ToLower(guess))) return true;
            else return false;
        }

        public bool IsGameWon(Statistic stats)
        {
            // Din logik för att avgöra om spelet är vunnet
            return stats.EntireWordSuccess;
        }

        public bool IsGameOver(Statistic stats)
        {
            return stats.IsGameOver;
        }
    }
}

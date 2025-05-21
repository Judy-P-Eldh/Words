namespace Words.Models
{
    public class GameViewModel
    {
        public Statistic? Stats { get; set; } = new Statistic();
        public string? FeedbackMessage { get; set; }
        public char? Guess { get; set; }
        public string? WordGuess { get; set; }
        public int GuessesLeft => Stats?.MaxGuesses - Stats?.GuessCount ?? 0;
    }
}

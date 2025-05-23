﻿namespace Words.Models
{
    public class Statistic
    {
        public string CurrentWord { get; set; } = string.Empty;
        public WordCategory Category { get; set; }
        public bool ShowHint { get; set; }
        public bool ShowWordGuess { get; set; }
        public List<char> CorrectGuesses { get; set; } = new();
        public List<char> WrongGuesses { get; set; } = new();
        public int WrongWordGuesses { get; set; }
        public int MaxGuesses { get; set; }
        public int GuessCount => CorrectGuesses.Count + WrongGuesses.Count + WrongWordGuesses;
        public bool EntireWordSuccess =>
            !string.IsNullOrEmpty(CurrentWord) &&
            CurrentWord
                .ToUpper()
                .Where(char.IsLetter)
                .Distinct()
                .All(c => CorrectGuesses.Contains(c));
        public bool IsGameOver => GuessCount >= MaxGuesses;
        public bool IsGameWon => EntireWordSuccess;

        //TODO: Lägg till lista för klarade ord
    }
}

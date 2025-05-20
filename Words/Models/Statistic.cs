namespace Words.Models
{
    public class Statistic
    {
        public string CurrentWord { get; set; } = string.Empty;
        public WordCategory Category { get; set; }
        public List<char> CorrectGuesses { get; set; } = new();
        public List<char> WrongGuesses { get; set; } = new();
        public int MaxGuesses { get; set; }
        public int GuessCount => CorrectGuesses.Count + WrongGuesses.Count;
        public bool EntireWordSuccess =>
            !string.IsNullOrEmpty(CurrentWord) &&
            CurrentWord
                .ToUpper()
                .Where(char.IsLetter)
                .Distinct()
                .All(c => CorrectGuesses.Contains(c));
        public bool IsGameOver => GuessCount >= MaxGuesses; // || EntireWordSuccess;
        public bool IsGameWon => EntireWordSuccess;

        //TODO: Lägg till lista för klarade ord
    }
}

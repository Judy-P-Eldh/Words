namespace Words.Models
{
    public class Statistic
    {
        public string CurrentWord { get; set; } = string.Empty;
        public List<char> CorrectGuesses { get; set; } = new();
        public List<char> WrongGuesses { get; set; } = new();
        public int MaxGuesses { get; set; }
        public int GuessCount => CorrectGuesses.Count + WrongGuesses.Count;
        public bool EntireWordSuccess { get; set; } = false;
        public bool IsGameOver => GuessCount >= MaxGuesses || EntireWordSuccess;
        public bool IsGameWon => EntireWordSuccess;

        //Lägg till lista för klarade ord
    }
}

namespace Words.Services
{
    public interface ICheckWords
    {
        public string GenerateRandomWord();
        public bool IsGuessed(string word);
        public bool IsLetterSingle(string input);
        public bool IsLetterCorrect(string input, string randomWord);
        public List<string> AddIncorrectGuess(string input, string randomWord);
    }
}
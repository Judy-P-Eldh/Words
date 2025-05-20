using Words.Models;

namespace Words.Services
{
    public interface ICheckWords
    {
        public WordEntry GenerateRandomWord();
        public bool IsGuessed(WordEntry word);
        public bool IsLetterSingle(string input);
        public bool IsLetterCorrect(string input, WordEntry randomWord);
        public List<string> AddIncorrectGuess(string input, WordEntry randomWord);
    }
}
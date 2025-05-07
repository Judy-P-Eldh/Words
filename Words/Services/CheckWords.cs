using Words.Models;

namespace Words.Services   
{
    public class CheckWords    : ICheckWords
    {
        private readonly WordList _wordList;
        public string randomWord { get; private set; }
        public CheckWords(WordList wordList)
        {
            _wordList = wordList ?? throw new ArgumentNullException(nameof(wordList));
        }
       
        public string GenerateRandomWord()
        {
            randomWord = _wordList.GetRandomWord();
            return randomWord;
        }
        public bool IsGuessed(string word)
        {
            if (_wordList.wordsDone.Contains(word)) return true;

            else return false;
        }

        public bool IsLetterSingle(string input)
        {
            if (input.Length == 1 && char.IsLetter(input[0])) return true;
            else return false;
        }
        public bool IsLetterCorrect(string input, string randomWord)
        {
            if (IsLetterSingle(input) && randomWord.Contains(input)) return true;
            else return false;
        }

        public List<string> IncorrectGuessing = new List<string>();
        public List<string> AddIncorrectGuess(string input, string randomWord)
        {
            if (!IsLetterCorrect(input, randomWord))
            {
                IncorrectGuessing.Add(input);
                return IncorrectGuessing;
            }
            return IncorrectGuessing;
        }
    }
}

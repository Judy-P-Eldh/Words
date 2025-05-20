using Words.Models;

namespace Words.Services   
{
    public class CheckWords    : ICheckWords
    {
        private readonly WordList _wordList;
        public WordEntry? randomWord { get; private set; }
        public CheckWords(WordList wordList)
        {
            _wordList = wordList;
        }
       
        public WordEntry  GenerateRandomWord()
        {
            randomWord = _wordList.GetRandomWord();
            return randomWord;
        }
        public bool IsGuessed(WordEntry word)
        {
            if (_wordList.wordsDone.Contains(word)) return true;

            else return false;
        }

        public bool IsLetterSingle(string input)
        {
            if (input.Length == 1 && char.IsLetter(input[0])) return true;
            else return false;
        }
        public bool IsLetterCorrect(string input, WordEntry randomWord)
        {
            if (!IsLetterSingle(input)) return false; //&& randomWord.Contains(input)) return true;
            char guess = char.ToUpper(input[0]);
            return randomWord.Word.ToUpper().Contains(guess);
            //else return false;
        }

        public List<string> IncorrectGuessing = new List<string>();
        public List<string> AddIncorrectGuess(string input, WordEntry randomWord)
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

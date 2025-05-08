using Words.Models;

namespace Words.Services
{
    public class WordGenerator
    {
        private readonly WordList _wordlist;

        public WordGenerator(WordList wordlist)
        {
            _wordlist = wordlist;
        }

        public List<string> GetAllWords()
        {
            return _wordlist.GetWords();
        }
        public string GetRandomWord()
        {
            return _wordlist.GetRandomWord();
        }
        public List<string> GetWordsDone()
        {
            return _wordlist.wordsDone;
        }
    }
}

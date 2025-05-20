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

        public List<WordEntry> GetAllWords()
        {
            return _wordlist.GetWords();
        }
        public WordEntry GetRandomWord()
        {
            return _wordlist.GetRandomWord();
        }
        public List<WordEntry> GetWordsDone()
        {
            return _wordlist.wordsDone;
        }
    }
}

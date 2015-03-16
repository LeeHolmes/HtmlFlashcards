using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlashCardsSupport
{
	/// <summary>
	/// A deck of Flash Cards.
	/// </summary>
	[Serializable]
    public class FlashDeck : IEnumerable
    {
        private Hashtable deck;
        private string[] deckIds;
        private Random randomGenerator;

        public FlashDeck()
        {
            randomGenerator = new Random();
        }

        public static void SaveSession(FlashDeck currentDeck, string filename)
        {
            using(Stream saveStream = File.OpenWrite(filename))
            {
                if (saveStream != null)
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(saveStream, currentDeck);
                }
            }
        }

        public static FlashDeck LoadSession(string filename)
        {
            using(Stream readStream = File.OpenRead(filename))
            {
                if (readStream != null)
                {
                    IFormatter formatter = new BinaryFormatter();
                    return (FlashDeck) formatter.Deserialize(readStream);
                }
                else
                    return null;
            }
        }

        public int Count { get { return deck.Count; } }

        public void LoadDeck(string directoryName)
        {
            if(! Directory.Exists(directoryName))
                throw new DirectoryNotFoundException();

            deck = new Hashtable();

            foreach(string currentFile in Directory.GetFiles(directoryName, "Q_*.html"))
            {
                string questionContents = "";
                string answerContents = "";
                
                string id = currentFile.ToUpper();
                id = id.Remove(0, id.LastIndexOf("Q_") + 2);
                id = id.Replace(".HTML", "");

                string answerFile = currentFile.ToUpper().Replace("Q_", "A_");

                StreamReader inFile = File.OpenText(currentFile);
                questionContents = inFile.ReadToEnd();
                inFile.Close();

                if(! File.Exists(answerFile))
                    continue;

                inFile = File.OpenText(answerFile);
                answerContents = inFile.ReadToEnd();
                inFile.Close();

                FlashCard nextCard = new FlashCard(id, questionContents, answerContents);
                deck[id] = nextCard;
            }

            CreateIdArray();
        }

        private void CreateIdArray()
        {
            deckIds = new string[deck.Count];
            int counter = 0;
            foreach(string keyName in deck.Keys)
            {
                deckIds[counter] = keyName;
                counter++;
            }
        }

        public FlashCard GetSpecific(string id)
        {
            if((deck == null) || (deck.Count == 0))
                throw new DeckEmptyException();

            return (FlashCard) deck[id];
        }

        public FlashCard GetRandom()
        {
            if((deck == null) || (deck.Count == 0))
                throw new DeckEmptyException();

            int nextCard = randomGenerator.Next(deck.Count);
            string id = deckIds[nextCard];
            return GetSpecific(id);
        }

        public FlashCard GetWeakest()
        {
            if((deck == null) || (deck.Count == 0))
                throw new DeckEmptyException();

            double currentScore, weakestScore = -1;
            string weakestId = null;

            foreach(string currentId in deck.Keys)
            {
                FlashCard currentCard = (FlashCard) deck[currentId];

                if((currentCard.CorrectCount + currentCard.IncorrectCount) == 0)
                    currentScore = 0;
                else
                    currentScore = 
                        ((double) currentCard.CorrectCount) / 
                        ((double) (currentCard.CorrectCount + currentCard.IncorrectCount));

                if((currentScore < weakestScore) || (weakestScore < 0))
                {
                    weakestId = currentId;
                    weakestScore = currentScore;
                }
            }

            return (FlashCard) deck[weakestId];
        }

        #region IEnumerable Members
        public IEnumerator GetEnumerator() 
        { 
            return deck.Values.GetEnumerator(); 
        }
        #endregion
    }
}

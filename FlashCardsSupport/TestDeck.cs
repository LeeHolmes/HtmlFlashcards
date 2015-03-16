using System;
using NUnit.Framework;

namespace FlashCardsSupport
{
	/// <summary>
	/// Test a deck of Flash Cards.
	/// </summary>
    [TestFixture]
	public class TestDeck
	{
        [Test]
        public void CreateDeck()
        {
            FlashDeck flashDeck = new FlashDeck();
        }

        [Test]
        [ExpectedException(typeof(System.IO.DirectoryNotFoundException))]
        public void LoadDeckBadDirectory()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\aoeushaoeu";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);
        }

        [Test]
        public void LoadDeckGoodDirectory()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\multiplication";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            Assertion.AssertEquals(0, flashDeck.Count);
        }

        [Test]
        public void LoadDeckSeveralQuestions()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            Assertion.AssertEquals(25, flashDeck.Count);
        }

        [Test]
        public void DoesntLoadBrokenQuestion()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            Assertion.AssertEquals(null, flashDeck.GetSpecific("NO-ANSWER"));
        }


        [Test]
        public void LoadEmpties()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";
            string deckDirectory2 = Environment.CurrentDirectory + "\\multiplication";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);
            Assertion.AssertEquals(25, flashDeck.Count);

            flashDeck.LoadDeck(deckDirectory2);
            Assertion.AssertEquals(0, flashDeck.Count);
        }

        [Test]
        [ExpectedException(typeof(DeckEmptyException))]
        public void LoadEmptiesNext()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";
            string deckDirectory2 = Environment.CurrentDirectory + "\\multiplication";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);
            Assertion.AssertEquals(25, flashDeck.Count);

            flashDeck.LoadDeck(deckDirectory2);
            Assertion.AssertEquals(0, flashDeck.Count);
            flashDeck.GetRandom();
        }

        [Test]
        public void GetSpecific()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            FlashCard currentCard = flashDeck.GetSpecific("8");
            Assertion.AssertNotNull(currentCard);
        }

        [Test]
        public void GetsQuestion()
        {
            string expectedQuestion = "4 4";
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            FlashCard currentCard = flashDeck.GetSpecific("4-4");
            Assertion.AssertEquals(expectedQuestion, currentCard.Question);
        }

        [Test]
        public void GetsAnswer()
        {
            string expectedAnswer = "Split on 5,6.  Otherwise hit.";
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            FlashCard currentCard = flashDeck.GetSpecific("4-4");
            Assertion.AssertEquals(expectedAnswer, currentCard.Answer);
        }

        [Test]
        public void GetsId()
        {
            string expectedId = "4-4";
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            FlashCard currentCard = flashDeck.GetSpecific("4-4");
            Assertion.AssertEquals(expectedId, currentCard.Id);
        }

        [Test]
        public void GetsRandom()
        {
            System.Collections.ArrayList seenList = new System.Collections.ArrayList();
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            for(int counter = 0; counter < flashDeck.Count * 10; counter++)
            {
                FlashCard currentCard = flashDeck.GetRandom();

                if(! seenList.Contains(currentCard.Id))
                    seenList.Add(currentCard.Id);
            }
            
            Assertion.Assert((flashDeck.Count - seenList.Count) < 5);
        }

        [Test]
        public void GetsWeakest()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            // Mark a bunch correct
            for(int counter = 0; counter < flashDeck.Count * 10; counter++)
            {
                FlashCard currentCard = flashDeck.GetRandom();
                currentCard.MarkCorrect();
            }

            FlashCard badCard = flashDeck.GetSpecific("5-5");
            for(int counter = 0; counter < 100; counter++)
                badCard.MarkIncorrect();

            FlashCard weakestCard = flashDeck.GetWeakest();
            Assertion.AssertEquals("5-5", weakestCard.Id);
        }

        [Test]
        public void GetsWeakestSparse()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            // Mark one correct
            FlashCard goodCard = flashDeck.GetWeakest();
            goodCard.MarkCorrect();

            FlashCard weakestCard = flashDeck.GetWeakest();
            Assertion.AssertEquals(0, weakestCard.CorrectCount);
        }

        [Test]
        public void WeakestChanges()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            // Mark a bunch correct
            for(int counter = 0; counter < flashDeck.Count * 10; counter++)
            {
                FlashCard currentCard = flashDeck.GetRandom();
                currentCard.MarkCorrect();
            }

            FlashCard badCard = flashDeck.GetSpecific("5-5");
            for(int counter = 0; counter < 100; counter++)
                badCard.MarkIncorrect();

            FlashCard weakestCard = flashDeck.GetWeakest();
            Assertion.AssertEquals("5-5", weakestCard.Id);
            
            badCard = flashDeck.GetSpecific("A-A");
            for(int counter = 0; counter < 500; counter++)
                badCard.MarkIncorrect();

            weakestCard = flashDeck.GetWeakest();
            Assertion.AssertEquals("A-A", weakestCard.Id);
        }

        [Test]
        public void SerializeAndDeserialize()
        {
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            // Mark one incorrect a lot
            FlashCard tempCard = flashDeck.GetSpecific("5-5");
            for(int counter = 0; counter < 100; counter++)
                tempCard.MarkIncorrect();

            // Save and load the state
            FlashDeck.SaveSession(flashDeck, Environment.CurrentDirectory + "\\blackjack\\statistics.fdk");
            FlashDeck secondDeck = FlashDeck.LoadSession(Environment.CurrentDirectory + "\\blackjack\\statistics.fdk");

            FlashCard fiveCard = secondDeck.GetSpecific("5-5");
            Assertion.AssertEquals(100, fiveCard.IncorrectCount);
        }

        [Test]
        [ExpectedException(typeof(DeckEmptyException))]
        public void GetWeakestException()
        {
            FlashDeck newDeck = new FlashDeck();
            newDeck.GetWeakest();
        }

        [Test]
        [ExpectedException(typeof(DeckEmptyException))]
        public void GetRandomException()
        {
            FlashDeck newDeck = new FlashDeck();
            newDeck.GetRandom();
        }

        [Test]
        [ExpectedException(typeof(DeckEmptyException))]
        public void GetSpecificException()
        {
            FlashDeck newDeck = new FlashDeck();
            newDeck.GetSpecific("5-5");
        }

        [Test]
        public void IsEnumerable()
        {
            int cardCount = 0;
            string deckDirectory = Environment.CurrentDirectory + "\\blackjack";

            FlashDeck flashDeck = new FlashDeck();
            flashDeck.LoadDeck(deckDirectory);

            foreach(FlashCard currentCard in flashDeck)
            {
                string id = currentCard.Id;
                cardCount += 1;
            }

            Assertion.AssertEquals(25, cardCount);
        }
    }
}

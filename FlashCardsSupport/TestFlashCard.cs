using System;
using NUnit.Framework;

namespace FlashCardsSupport
{
	/// <summary>
	/// Test the FlashCard class.
	/// </summary>
	[TestFixture]
	public class TestFlashCard
	{
        [Test]
		public void FlashCardQuestion()
		{
            string question = "How many are there?";
            string answer = "";

            FlashCard flashCard = new FlashCard("", question, answer);
            Assertion.AssertEquals(question, flashCard.Question);
		}

        [Test]
        public void FlashCardAnswer()
        {
            string question = "";
            string answer = "52";

            FlashCard flashCard = new FlashCard("", question, answer);
            Assertion.AssertEquals(answer, flashCard.Answer);
        }

        [Test]
        public void FlashCardMarkRight()
        {
            string question = "";
            string answer = "52";

            FlashCard flashCard = new FlashCard("", question, answer);
            flashCard.MarkCorrect();

            Assertion.AssertEquals(1, flashCard.CorrectCount);
        }

        [Test]
        public void FlashCardMark2Right()
        {
            string question = "";
            string answer = "52";

            FlashCard flashCard = new FlashCard("", question, answer);
            flashCard.MarkCorrect();
            flashCard.MarkCorrect();

            Assertion.AssertEquals(2, flashCard.CorrectCount);
        }

        [Test]
        public void FlashCardMarkWrong()
        {
            string question = "";
            string answer = "52";

            FlashCard flashCard = new FlashCard("", question, answer);
            flashCard.MarkIncorrect();

            Assertion.AssertEquals(1, flashCard.IncorrectCount);
        }

        [Test]
        public void FlashCardMark2Wrong()
        {
            string question = "";
            string answer = "52";

            FlashCard flashCard = new FlashCard("", question, answer);
            flashCard.MarkIncorrect();
            flashCard.MarkIncorrect();

            Assertion.AssertEquals(2, flashCard.IncorrectCount);
        }

        [Test]
        public void FlashCardMarkRightAndWrong()
        {
            string question = "";
            string answer = "52";

            FlashCard flashCard = new FlashCard("", question, answer);
            flashCard.MarkIncorrect();
            flashCard.MarkIncorrect();
            flashCard.MarkCorrect();

            Assertion.AssertEquals(2, flashCard.IncorrectCount);
            Assertion.AssertEquals(1, flashCard.CorrectCount);
        }
    }
}


using System;

namespace FlashCardsSupport
{
	/// <summary>
	/// A Flash Card.
	/// </summary>
	[Serializable]
	public class FlashCard
	{
        private string question;
        private string answer;
        private string id;

        private int correctCount;
        private int incorrectCount;

		public FlashCard(string id, string question, string answer)
        {
            this.id = id;
            this.question = question;
            this.answer = answer;
            this.correctCount = 0;
            this.incorrectCount = 0;
		}

        public string Id { get { return id; } }
        public string Question { get { return question; } }
        public string Answer { get { return answer; } }
        public int CorrectCount { get { return correctCount; } }
        public int IncorrectCount { get { return incorrectCount; } }

        public void MarkCorrect() { correctCount++; }
        public void MarkIncorrect() { incorrectCount++; }
	}
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using FlashCardsSupport;

namespace FlashcardTrainer
{
	/// <summary>
	/// Flash card trainer.
	/// </summary>
	public class FlashcardTrainer : System.Windows.Forms.Form
	{
        private AxSHDocVw.AxWebBrowser axWebBrowser1;
        private System.Windows.Forms.Button flipButton;
        private System.Windows.Forms.Button wrongButton;
        private System.Windows.Forms.Button skipButton;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Label rightWrongText;
        private System.Windows.Forms.Label rightText;
        private System.Windows.Forms.Label wrongText;
        private System.Windows.Forms.Label percentText;

        private System.Windows.Forms.GroupBox statsGroup;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItemFile;
        private System.Windows.Forms.MenuItem menuItemOpen;

        private FlashDeck flashDeck;
        private FlashCard currentCard;
        private bool viewingQuestion;
        private string deckDirectory;
        private bool didWork;
        private bool loadAndUpdate;
        private string sessionFile;

        private Random generator;
        private System.Windows.Forms.MenuItem menuItemOpenSession;
        private System.Windows.Forms.MenuItem menuItemSave;

        /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public FlashcardTrainer() : this(null, false)
        {}

		public FlashcardTrainer(string filename, bool loadAndUpdate)
		{
			InitializeComponent();

            this.Icon = new Icon(this.GetType(), "App.ico");

            viewingQuestion = true;
            UpdateValues();

            flashDeck = new FlashDeck();
            skipButton.Enabled = false;
            flipButton.Enabled = false;
            didWork = false;
            this.loadAndUpdate = loadAndUpdate;

            generator = new Random();

            this.Closing += new CancelEventHandler(FlashcardTrainer_Closing);

            if(filename != null)
                LoadSession(filename);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FlashcardTrainer));
            this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
            this.flipButton = new System.Windows.Forms.Button();
            this.wrongButton = new System.Windows.Forms.Button();
            this.skipButton = new System.Windows.Forms.Button();
            this.rightButton = new System.Windows.Forms.Button();
            this.rightWrongText = new System.Windows.Forms.Label();
            this.rightText = new System.Windows.Forms.Label();
            this.wrongText = new System.Windows.Forms.Label();
            this.percentText = new System.Windows.Forms.Label();
            this.statsGroup = new System.Windows.Forms.GroupBox();
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemOpenSession = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
            this.SuspendLayout();
            // 
            // axWebBrowser1
            // 
            this.axWebBrowser1.Enabled = true;
            this.axWebBrowser1.Location = new System.Drawing.Point(8, 8);
            this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
            this.axWebBrowser1.Size = new System.Drawing.Size(352, 224);
            this.axWebBrowser1.TabIndex = 0;
            // 
            // flipButton
            // 
            this.flipButton.Location = new System.Drawing.Point(368, 8);
            this.flipButton.Name = "flipButton";
            this.flipButton.Size = new System.Drawing.Size(104, 23);
            this.flipButton.TabIndex = 1;
            this.flipButton.Text = "F&lip";
            this.flipButton.Click += new System.EventHandler(this.flipButton_Click);
            // 
            // wrongButton
            // 
            this.wrongButton.Enabled = false;
            this.wrongButton.Location = new System.Drawing.Point(368, 112);
            this.wrongButton.Name = "wrongButton";
            this.wrongButton.Size = new System.Drawing.Size(104, 23);
            this.wrongButton.TabIndex = 4;
            this.wrongButton.Text = "&Wrong";
            this.wrongButton.Click += new System.EventHandler(this.wrongButton_Click);
            // 
            // skipButton
            // 
            this.skipButton.Location = new System.Drawing.Point(368, 32);
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size(104, 23);
            this.skipButton.TabIndex = 2;
            this.skipButton.Text = "&Skip";
            this.skipButton.Click += new System.EventHandler(this.skipButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Enabled = false;
            this.rightButton.Location = new System.Drawing.Point(368, 88);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(104, 23);
            this.rightButton.TabIndex = 3;
            this.rightButton.Text = "&Right";
            this.rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // rightWrongText
            // 
            this.rightWrongText.Location = new System.Drawing.Point(368, 72);
            this.rightWrongText.Name = "rightWrongText";
            this.rightWrongText.Size = new System.Drawing.Size(40, 16);
            this.rightWrongText.TabIndex = 5;
            this.rightWrongText.Text = "I was:";
            // 
            // rightText
            // 
            this.rightText.Location = new System.Drawing.Point(376, 176);
            this.rightText.Name = "rightText";
            this.rightText.Size = new System.Drawing.Size(88, 16);
            this.rightText.TabIndex = 7;
            this.rightText.Text = "Right: 0";
            // 
            // wrongText
            // 
            this.wrongText.Location = new System.Drawing.Point(376, 192);
            this.wrongText.Name = "wrongText";
            this.wrongText.Size = new System.Drawing.Size(88, 16);
            this.wrongText.TabIndex = 8;
            this.wrongText.Text = "Wrong: 0";
            // 
            // percentText
            // 
            this.percentText.Location = new System.Drawing.Point(376, 208);
            this.percentText.Name = "percentText";
            this.percentText.Size = new System.Drawing.Size(88, 16);
            this.percentText.TabIndex = 9;
            this.percentText.Text = "100%";
            // 
            // statsGroup
            // 
            this.statsGroup.Location = new System.Drawing.Point(368, 152);
            this.statsGroup.Name = "statsGroup";
            this.statsGroup.Size = new System.Drawing.Size(104, 80);
            this.statsGroup.TabIndex = 10;
            this.statsGroup.TabStop = false;
            this.statsGroup.Text = "Score:";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                     this.menuItemFile});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                         this.menuItemOpen,
                                                                                         this.menuItemOpenSession,
                                                                                         this.menuItemSave});
            this.menuItemFile.Text = "&File";
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 0;
            this.menuItemOpen.Text = "Open &Deck";
            this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
            // 
            // menuItemOpenSession
            // 
            this.menuItemOpenSession.Index = 1;
            this.menuItemOpenSession.Text = "&Open Session";
            this.menuItemOpenSession.Click += new System.EventHandler(this.menuItemOpenSession_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Enabled = false;
            this.menuItemSave.Index = 2;
            this.menuItemSave.Text = "&Save Session";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // FlashcardTrainer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(480, 238);
            this.Controls.Add(this.percentText);
            this.Controls.Add(this.wrongText);
            this.Controls.Add(this.rightText);
            this.Controls.Add(this.rightWrongText);
            this.Controls.Add(this.rightButton);
            this.Controls.Add(this.skipButton);
            this.Controls.Add(this.wrongButton);
            this.Controls.Add(this.flipButton);
            this.Controls.Add(this.axWebBrowser1);
            this.Controls.Add(this.statsGroup);
            this.Menu = this.mainMenu;
            this.Name = "FlashcardTrainer";
            this.Text = "Flash Card Trainer";
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
            if(args.Length == 0)
                Application.Run(new FlashcardTrainer());
            else
            {
                if((args[0].ToUpper() == "/?") || (args[0].ToUpper() == "?") || (args[0].ToUpper() == "--HELP"))
                {
                    PrintUsage();
                    return;
                }

                string filename = args[0];
                bool loadAndUpdate = false;

                filename = System.IO.Path.GetFullPath(filename);

                if((args.Length > 1) && (args[1].ToUpper().IndexOf("TRUE") >= 0))
                    loadAndUpdate = true;

                Application.Run(new FlashcardTrainer(filename, loadAndUpdate));
            }
		}

        private static void PrintUsage()
        {
            string outputMessage = 
                "Usage: FlashcardTrainer.exe [.fdk file to open] [loadAndUpdate=true]\n\n" +
                "If you specify [loadAndUpdate=true], FlashcardTrainer will load the specified\n" +
                ".fdk file (Flash Deck Session,) test you on 1 question, save the results back\n" +
                "to that file, and exit.";

            MessageBox.Show(outputMessage);
        }

        private void UpdateValues()
        {
            int rightCount = GetRightCount();
            int wrongCount = GetWrongCount();

            rightText.Text = "Right: " + rightCount;
            wrongText.Text = "Wrong: " + wrongCount;

            int percent = 100;
            if((rightCount + wrongCount) > 0)
                percent = (int) ((double) rightCount / (double) (rightCount + wrongCount) * 100);

            percentText.Text = percent + " %";
        }

        private int GetRightCount()
        {
            int rightCount = 0;

            if(flashDeck != null)
            {
                foreach(FlashCard currentCard in flashDeck)
                    rightCount += currentCard.CorrectCount;
            }

            return rightCount;
        }

        private int GetWrongCount()
        {
            int wrongCount = 0;

            if(flashDeck != null)
            {
                foreach(FlashCard currentCard in flashDeck)
                    wrongCount += currentCard.IncorrectCount;
            }

            return wrongCount;
        }

        private void LoadNext()
        {
            // 30% of the time, load the weakest card.
            if(generator.Next(100) < 30)
                currentCard = flashDeck.GetWeakest();
            else
                currentCard = flashDeck.GetRandom();

            string questionFile = deckDirectory + "\\Q_" + currentCard.Id + ".html";

            DisplayFile(questionFile);
            rightButton.Enabled = false;
            wrongButton.Enabled = false;
            viewingQuestion = true;
        }

        private void Flip()
        {
            string displayFile = "";

            if(viewingQuestion)
                displayFile = deckDirectory + "\\A_" + currentCard.Id + ".html";
            else
                displayFile = deckDirectory + "\\Q_" + currentCard.Id + ".html";

            DisplayFile(displayFile);
            
            rightButton.Enabled = true;
            wrongButton.Enabled = true;

            viewingQuestion = ! viewingQuestion;
        }

        private void DisplayFile(string inputFile)
        {
            object oZero = 0;
            string emptyString = "";
            object oEmptyString = emptyString;

            axWebBrowser1.Navigate(inputFile, 
                ref oZero,
                ref oEmptyString,
                ref oEmptyString,
                ref oEmptyString);
        }

        private void skipButton_Click(object sender, System.EventArgs e)
        {
            LoadNext();
        }

        private void flipButton_Click(object sender, System.EventArgs e)
        {
            Flip();
        }

        private void rightButton_Click(object sender, System.EventArgs e)
        {
            didWork = true;
            currentCard.MarkCorrect();
            UpdateValues();
            LoadNext();

            if(loadAndUpdate)
            {
                SaveSession(sessionFile);
                this.Close();
            }
        }

        private void wrongButton_Click(object sender, System.EventArgs e)
        {
            didWork = true;
            currentCard.MarkIncorrect();
            UpdateValues();
            LoadNext();

            if(loadAndUpdate)
            {
                SaveSession(sessionFile);
                this.Close();
            }
        }

        private void menuItemOpen_Click(object sender, System.EventArgs e)
        {
            if(UserNeedsToSave())
                return;

            FolderBrowserDialog fileOpen = new FolderBrowserDialog();
            fileOpen.Description = "Browse to a Flash Pack Directory";
            fileOpen.SelectedPath = System.Environment.CurrentDirectory;
            
            DialogResult result = fileOpen.ShowDialog();

            if(result == DialogResult.OK)
            {
                string tempDirectory = fileOpen.SelectedPath;

                try
                {
                    flashDeck.LoadDeck(tempDirectory);

                    if(flashDeck.Count == 0)
                    {
                        MessageBox.Show("Error: There are no flash cards in this directory.");
                        return;
                    }

                    deckDirectory = tempDirectory;

                    flipButton.Enabled = true;
                    skipButton.Enabled = true;
                    menuItemSave.Enabled = true;
                    didWork = false;

                    viewingQuestion = true;
                    UpdateValues();

                    LoadNext();
                }
                catch
                {
                    MessageBox.Show("Error: Could not load files from " + tempDirectory);
                }
            }
        }

        private void menuItemOpenSession_Click(object sender, System.EventArgs e)
        {
            if(UserNeedsToSave())
                return;

            FileDialog fileOpen = new OpenFileDialog();
            fileOpen.Title = "Open a Flash Deck session";
            fileOpen.ValidateNames = true;
            fileOpen.CheckFileExists = true;
            fileOpen.Filter = "Flash Deck Session (*.fdk)|*.fdk"; 
            fileOpen.InitialDirectory = System.Environment.CurrentDirectory;
            
            DialogResult result = fileOpen.ShowDialog();

            if(result == DialogResult.OK)
            {
                string filename = fileOpen.FileName;

                try
                {
                    LoadSession(filename);
                }
                catch
                {
                    MessageBox.Show("Error: There was an error loading the session.");
                }
            }
        }

        private void LoadSession(string filename)
        {
            if(! System.IO.File.Exists(filename))
            {
                MessageBox.Show("Error: " + filename + " does not exist.", "File does not exist.");
                loadAndUpdate = false;
                return;
            }

            flashDeck = FlashDeck.LoadSession(filename);

            if((flashDeck == null) || (flashDeck.Count == 0))
            {
                MessageBox.Show("Error: There was an error loading the session.");
                return;
            }

            sessionFile = filename;
            deckDirectory = System.IO.Directory.GetParent(filename).ToString();

            flipButton.Enabled = true;
            skipButton.Enabled = true;
            menuItemSave.Enabled = true;
            didWork = false;

            viewingQuestion = true;
            UpdateValues();
            LoadNext();
        }

        private void menuItemSave_Click(object sender, System.EventArgs e)
        {
            FileDialog fileSave = new SaveFileDialog();
            fileSave.Title = "Save a Flash Deck session";
            fileSave.ValidateNames = true;
            fileSave.Filter = "Flash Deck Session (*.fdk)|*.fdk"; 
            fileSave.InitialDirectory = System.Environment.CurrentDirectory;
            
            DialogResult result = fileSave.ShowDialog();

            if(result == DialogResult.OK)
            {
                string filename = fileSave.FileName;
                string newDirectory = System.IO.Directory.GetParent(filename).ToString();

                if(newDirectory.ToUpper() != deckDirectory.ToUpper())
                {
                    result = MessageBox.Show(
                        "Warning: You are saving this session to a directory that doesn't " +
                        "contain your deck of cards.  Unless you move this file into the " +
                        "directory that contains your deck of cards, FlashTrainer will be " +
                        "unable to load your cards.", 
                        "Directory Warning",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);
                    
                    if(result == DialogResult.Cancel)
                        return;
                }

                try
                {
                    SaveSession(filename);
                }
                catch
                {
                    MessageBox.Show("Error: There was an error saving the session.");
                }
            }
        }

        private void SaveSession(string filename)
        {
            FlashDeck.SaveSession(flashDeck, filename);
            didWork = false;
        }

        private bool UserNeedsToSave()
        {
            if(! didWork)
                return false;

            DialogResult result = MessageBox.Show("You have unsaved changes.  Continue?", 
                "You have unsaved changes.",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            return (result == DialogResult.No);
        }

        private void FlashcardTrainer_Closing(object sender, CancelEventArgs e)
        {
            if(UserNeedsToSave())
                e.Cancel = true;;
        }
    }
}

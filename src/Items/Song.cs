namespace ShowBuddy.Items
{
    using System.IO;
    using System.Media;
    using System.Threading.Tasks;

    using ShowBuddy.Core;

    internal class Song
    {
        internal static int Index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Delay">The delay.</param>
        /// <param name="LyricDelay">The lyric delay.</param>
        internal Song(string Name, int Delay, int LyricDelay)
        {
            this.Name = Name;
            this.Delay = Delay;
            this.LyricDelay = LyricDelay;
        }

        /// <summary>
        /// Plays this instance.
        /// </summary>
        internal void Play()
        {
            if (Song.Index < MainUI.SetList.Items.Count)
            {
                this.PlaySong();
                this.PrintLyrics();
                
                Song.Index++;
            }
        }

        /// <summary>
        /// Plays the song.
        /// </summary>
        internal void PlaySong()
        {
            if (File.Exists($@"Sets/Songs/{this.Name}.wav"))
            {
                var Player = new SoundPlayer($@"Sets/Songs/{this.Name}.wav");
                Player.Play();

                MainUI.Teleprompter.Items.Clear();
            }
            else
            {
                MainUI.Teleprompter.Items.Add("Failed to find the specified song!");
            }
        }

        /// <summary>
        /// Prints the lyrics.
        /// </summary>
        internal async void PrintLyrics()
        {
            if (File.Exists($@"Sets\Lyrics\{this.Name}.txt"))
            {
                await Task.Delay(this.Delay);

                foreach (var Line in File.ReadAllLines($@"Sets\Lyrics\{this.Name}.txt"))
                {
                    MainUI.Teleprompter.Items.Add(Line);
                    MainUI.Teleprompter.Refresh();

                    MainUI.Teleprompter.SelectedIndex = MainUI.Teleprompter.Items.Count - 1;
                    MainUI.Teleprompter.SelectedIndex = -1;
                    
                    await Task.Delay(this.LyricDelay);
                }
            }
            else
            {
                MainUI.Teleprompter.Items.Add("Failed to find lyrics for the specified song!");
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        internal string Name
        {
            get;
        }

        /// <summary>
        /// Gets the delay.
        /// </summary>
        internal int Delay
        {
            get;
        }

        /// <summary>
        /// Gets the lyric delay.
        /// </summary>
        internal int LyricDelay
        {
            get;
        }
    }
}

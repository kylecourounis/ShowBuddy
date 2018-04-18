namespace ShowBuddy.Core
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    
    using ShowBuddy.Items;

    internal class MainUI
    {
        /// <summary>
        /// The form
        /// </summary>
        private static readonly Form Form = new Form
        {
            Text = "ShowBuddy",
            Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("ShowBuddy.icon.ico")),
            Width = 750,
            Height = 440,
            BackColor = Color.Silver
        };

        /// <summary>
        /// The up button
        /// </summary>
        internal static Button Add = new Button
        {
            Text = "Add",
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            BackColor = Color.ForestGreen,
            ForeColor = Color.White,
            Dock = DockStyle.Bottom
        };

        /// <summary>
        /// The up button
        /// </summary>
        internal static Button Up = new Button
        {
            Text = "Up",
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            BackColor = Color.ForestGreen,
            ForeColor = Color.White,
            Dock = DockStyle.Bottom
        };

        /// <summary>
        /// The down button
        /// </summary>
        internal static Button Down = new Button
        {
            Text = "Down",
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            BackColor = Color.ForestGreen,
            ForeColor = Color.White,
            Dock = DockStyle.Bottom
        };

        /// <summary>
        /// The teleprompter
        /// </summary>
        internal static ListBox Teleprompter = new ListBox
        {
            Font = new Font("Segoe UI", 15, FontStyle.Regular),
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill
        };

        /// <summary>
        /// The set list
        /// </summary>
        internal static ListBox SetList = new ListBox
        {
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill
        };
        
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            MainUI.Update();
            MainUI.Form.ShowDialog();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        private static void Update()
        {
            foreach (var Song in File.ReadAllLines(@"Sets/Set.txt"))
            {
                if (!Song.StartsWith("#"))
                {
                    string[] Data = Song.Split(',');
                    MainUI.SetList.Items.Add(Data[0]);
                }
            }

            MainUI.Add.Click += delegate
            {
                MainUI.ShowAddForm();
            };

            MainUI.SetList.DoubleClick += delegate
            {
                if (MainUI.SetList.SelectedItem == null || MainUI.SetList.SelectedIndex < 0)
                    return;

                MainUI.PlaySong(MainUI.SetList.SelectedItem.ToString());
            };
            
            MainUI.SetList.KeyDown += (Sender, Event) =>
            {
                if (Event.KeyCode == Keys.Delete)
                {
                    if (MainUI.SetList.SelectedItem == null || MainUI.SetList.SelectedIndex < 0)
                        return;

                    MainUI.SetList.Items.Remove(MainUI.SetList.SelectedItem);

                    File.WriteAllText("Sets/Set-Temp.txt", File.ReadAllText("Sets/Set.txt"));
                    File.Delete("Sets/Set.txt");
                    
                    foreach (var Item in MainUI.SetList.Items)
                    {
                        string[] Line = Array.Find(File.ReadAllLines(@"Sets\Set-Temp.txt"), Value => Value.Contains(Item.ToString())).Split(',');
                        File.AppendAllText("Sets/Set.txt", $"{Item},{Line[1]},{Line[2]}" + Environment.NewLine);
                    }

                    File.Delete("Sets/Set-Temp.txt");
                }
                if (Event.KeyCode == Keys.E)
                {
                    if (MainUI.SetList.SelectedItem == null || MainUI.SetList.SelectedIndex < 0)
                        return;

                    string[] Line = Array.Find(File.ReadAllLines(@"Sets\Set.txt"), Value => Value.Contains(MainUI.SetList.SelectedItem.ToString())).Split(',');

                    MainUI.ShowAddForm(true, Line[0], Line[1], Line[2]);
                }
            };

            MainUI.Up.Click += delegate
            {
                MainUI.MoveItem(-1);
            };

            MainUI.Down.Click += delegate
            {
                MainUI.MoveItem(1);
            };

            Panel Set = new Panel
            {
                Dock = DockStyle.Left,
                Padding = new Padding(3)
            };
            Set.Controls.Add(MainUI.SetList);
            Set.Controls.Add(MainUI.Add);
            Set.Controls.Add(MainUI.Up);
            Set.Controls.Add(MainUI.Down);

            Panel Prompter = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(3)
            };
            Prompter.Controls.Add(MainUI.Teleprompter);

            MainUI.Form.Controls.Add(Prompter);
            MainUI.Form.Controls.Add(Set);

            MainUI.Form.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Shows the add form.
        /// </summary>
        /// <param name="Edit">if set to <c>true</c> [edit].</param>
        /// <param name="NameTxt">The name text.</param>
        /// <param name="DelayTxt">The delay text.</param>
        /// <param name="LyricDelayTxt">The lyric delay text.</param>
        internal static void ShowAddForm(bool Edit = false, string NameTxt = null, string DelayTxt = null, string LyricDelayTxt = null)
        {
            Form AddForm = new Form
            {
                Text = "Add Song",
                Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("ShowBuddy.icon.ico")),
                Width = 230,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MinimizeBox = false,
                MaximizeBox = false,
                BackColor = Color.AliceBlue,
            };

            TextBox SongName = new TextBox
            {
                Text = "Song Filename",
                Width = 200,
                Height = 30,
                Location = new Point(8, 5)
            };

            TextBox Delay = new TextBox
            {
                Text = "Delay",
                Width = 200,
                Height = 30,
                Location = new Point(8, 40)
            };

            TextBox LyricDelay = new TextBox
            {
                Text = "Lyric Delay",
                Width = 200,
                Height = 30,
                Location = new Point(8, 70)
            };

            Button Save = new Button
            {
                Text = "Save",
                Width = 200,
                Height = 20,
                Location = new Point(8, 100),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
            };

            if (Edit)
            {
                SongName.Text = NameTxt;
                Delay.Text = DelayTxt;
                LyricDelay.Text = LyricDelayTxt;
            }

            Save.Click += delegate
            {
                AddForm.Close();

                if (Edit)
                {
                    string Text = File.ReadAllText("Sets/Set.txt");
                    File.WriteAllText("Sets/Set.txt", Text.Replace($"{NameTxt},{DelayTxt},{LyricDelayTxt}", $"{SongName.Text},{Delay.Text},{LyricDelay.Text}"));
                }
                else
                {
                    File.AppendAllText("Sets/Set.txt", $"{SongName.Text},{Delay.Text},{LyricDelay.Text}" + Environment.NewLine);
                }

                MainUI.SetList.Items.Clear();

                foreach (var Song in File.ReadAllLines(@"Sets\Set.txt"))
                {
                    if (!Song.StartsWith("#"))
                    {
                        string[] Data = Song.Split(',');
                        MainUI.SetList.Items.Add(Data[0]);
                    }
                }
            };

            using (AddForm)
            {
                AddForm.Controls.Add(SongName);
                AddForm.Controls.Add(Delay);
                AddForm.Controls.Add(LyricDelay);
                AddForm.Controls.Add(Save);

                AddForm.ShowDialog();
            }
        }

        /// <summary>
        /// Plays the song.
        /// </summary>
        internal static void PlaySong(string SongName)
        {
            string[] Line = Array.Find(File.ReadAllLines(@"Sets\Set.txt"), Value => Value.Contains(SongName)).Split(',');

            var Song = new Song(Line[0], int.Parse(Line[1]), int.Parse(Line[2]));
            Song.Play();
        }
        
        /// <summary>
        /// Moves the item.
        /// </summary>
        /// <param name="Direction">The direction.</param>
        internal static void MoveItem(int Direction)
        {
            if (MainUI.SetList.SelectedItem == null || MainUI.SetList.SelectedIndex < 0)
                return;
            
            int NewIndex = MainUI.SetList.SelectedIndex + Direction;
            if (NewIndex < 0 || NewIndex >= MainUI.SetList.Items.Count)
                return; 

            object Selected = MainUI.SetList.SelectedItem;
            
            MainUI.SetList.Items.Remove(Selected);
            MainUI.SetList.Items.Insert(NewIndex, Selected);
            MainUI.SetList.SetSelected(NewIndex, true);

            File.WriteAllText("Sets/Set-Temp.txt", File.ReadAllText("Sets/Set.txt"));
            File.Delete("Sets/Set.txt");
            
            foreach (var Item in MainUI.SetList.Items)
            {
                string[] Line = Array.Find(File.ReadAllLines(@"Sets\Set-Temp.txt"), Value => Value.Contains(Item.ToString())).Split(',');
                File.AppendAllText("Sets/Set.txt", $"{Item},{Line[1]},{Line[2]}" + Environment.NewLine);
            }

            File.Delete("Sets/Set-Temp.txt");
        }
    }
}

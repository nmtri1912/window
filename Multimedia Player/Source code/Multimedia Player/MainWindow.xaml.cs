using Gma.System.MouseKeyHook;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Multimedia_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<string> _playList = new List<string>();
        MediaPlayer _player = new MediaPlayer();

        private bool IsDraggingSlider = false;
        private bool IsChosen = false;
        private bool IsLoaded = false;
        int _index = 0;
        DispatcherTimer _timer = new DispatcherTimer();

        int _time = 0;

        //hook ban phim
        private IKeyboardMouseEvents _hook;

        private void btnAddMedia_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                var filenames = screen.FileNames;

                foreach (var filename in filenames)
                {
                    PlayList.Items.Add(GetName(filename));
                    _playList.Add(filename);
                }
                IsChosen = true;
            }

            PlayList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
        }

        private string GetName(string name)
        {
            string[] tokens = name.Split(new string[] { "\\" }, StringSplitOptions.None);

            return tokens[tokens.Length - 1];
        }

        private void btnRemoveMedia_Click(object sender, RoutedEventArgs e)
        {
            if (PlayList.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Moi ban chon bai hat de xoa");
            }

            for (int i = PlayList.SelectedItems.Count - 1; i >= 0; i--)
            {
                _playList.RemoveAt(int.Parse(PlayList.SelectedIndex.ToString()));
                PlayList.Items.Remove(PlayList.SelectedItem);
            }
        }

        private void btnPlayMedia_Click(object sender, RoutedEventArgs e)
        {
            if (IsChosen == true)
            {
                if (PlayList.SelectedIndex == -1)
                {
                    _player.Open(new Uri(_playList[_index]));
                    _player.Play();
                    songName.Content = GetName(_playList[_index]);
                }
                else
                {
                    _index = int.Parse(PlayList.SelectedIndex.ToString());
                    _player.Open(new Uri(_playList[_index]));
                    _player.Play();
                    songName.Content = GetName(_playList[_index]);
                }

                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += timer_Tick;
                _timer.Start();
                _player.MediaEnded += _player_MediaEnded;


                //
            }
            else
            {
                System.Windows.MessageBox.Show("Playlist đang trống, không thể bắt đầu");
            }
        }

        private void _player_MediaEnded(object sender, EventArgs e)
        {
            if (randomRadioButton.IsChecked == true)
            {
                Random random = new Random();
                _index = random.Next(1, _playList.Count) - 1;

                _player.Open(new Uri(_playList[_index]));
                _player.Play();

                songName.Content = GetName(_playList[_index]);
            }
            else
            {
                _index++;
                if (_index == _playList.Count)
                {
                    if (infinityRadioButton.IsChecked == true)
                    {
                        _index = 0;

                        _player.Open(new Uri(_playList[_index]));
                        _player.Play();
                        songName.Content = GetName(_playList[_index]);
                    }
                    else
                    {
                        _player.Stop();
                    }
                }
                else
                {
                    _player.Open(new Uri(_playList[_index]));
                    _player.Play();
                    songName.Content = GetName(_playList[_index]);
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_player.Source != null && _player.NaturalDuration.HasTimeSpan && !IsDraggingSlider)
            {
                TimeStatus.Content = String.Format("{0} / {1}", _player.Position.ToString(@"mm\:ss"),
                    _player.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                TimeLine.Minimum = 0;
                TimeLine.Maximum = _player.NaturalDuration.TimeSpan.TotalSeconds;
                TimeLine.Value = _player.Position.TotalSeconds;
            }
        }

        private void TimeLine_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsDraggingSlider = true;
        }

        private void TimeLine_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsDraggingSlider = false;
            _player.Position = TimeSpan.FromSeconds(TimeLine.Value);
        }

        private void TimeLine_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeStatus.Content = String.Format("{0} / {1}", TimeSpan.FromSeconds(TimeLine.Value).ToString(@"mm\:ss"),
                    _player.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }

        private void btnPauseButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Pause();
        }

        private void btnResumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoaded == true)
            {
                _player.Position = TimeSpan.FromSeconds(_time);
                IsLoaded = false;
            }
            //_player.Position = TimeSpan.FromSeconds(_time);
            _player.Play();
            _timer.Start();
            _player.MediaEnded += _player_MediaEnded;
        }

        private void btnStopButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();
        }

        private void btnNextButton_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void Next()
        {
            if (_index < _playList.Count - 1)
            {
                _index = _index + 1;
                _player.Open(new Uri(_playList[_index]));
                _player.Play();
                songName.Content = GetName(_playList[_index]);


                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += timer_Tick;
                _timer.Start();
                _player.MediaEnded += _player_MediaEnded;
            }
            else
            {
                System.Windows.MessageBox.Show("Can't next!");
            }
        }

        private void btnPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            Previous();
        }

        private void Previous()
        {
            if (_index > 0)
            {
                _index = _index - 1;
                _player.Open(new Uri(_playList[_index]));
                _player.Play();
                songName.Content = GetName(_playList[_index]);


                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += timer_Tick;
                _timer.Start();
                _player.MediaEnded += _player_MediaEnded;
            }
            else
            {
                System.Windows.MessageBox.Show("Can't previous!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Khoi dong chuong trinh, load

            try
            {
                var filename = "save.txt";
                var reader = new StreamReader(filename);
                IsChosen = true;
                if (reader != null)
                {
                    var index = int.Parse(reader.ReadLine());   // index
                    _time = int.Parse(reader.ReadLine());    // 
                    var count = int.Parse(reader.ReadLine());

                    for (int i = 0; i < count; i++)
                    {
                        var fileName = reader.ReadLine().ToString();
                        _playList.Add(fileName);
                        PlayList.Items.Add(GetName(fileName));
                    }

                    reader.Close();
                    IsLoaded = true;

                    //tiep tuc choi bai hat truoc luc tat chuong trinh
                    _player.Open(new Uri(_playList[index]));

                    //
                    songName.Content = GetName(_playList[index]);
                    //

                    _timer.Interval = TimeSpan.FromSeconds(1);
                    _timer.Tick += timer_Tick;
                    //
                }
                
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }

            _hook = Hook.GlobalEvents();

            _hook.KeyUp += _hook_KeyUp;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            myGif.Position = new TimeSpan(0, 0, 1);
            myGif.Play();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Khi tat chuong trinh
            var filename = "save.txt";
            var writer = new StreamWriter(filename);

            writer.WriteLine(_index);       // vi tri bai hat

            writer.WriteLine(Convert.ToInt32(_player.Position.TotalSeconds));    // thoi diem

            writer.WriteLine(_playList.Count);

            for (int i = 0; i < _playList.Count; i++)
            {
                writer.WriteLine(_playList[i]);
            }

            writer.Close();

            _hook.KeyUp -= _hook_KeyUp;
            _hook.Dispose();
        }

        private void _hook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Previous
            if (e.Control && e.Shift && (e.KeyCode == Keys.V))
            {
                Previous();
            }
            // Next
            if (e.Control && e.Shift && (e.KeyCode == Keys.B))
            {
                Next();
            }
            //Pause
            if (e.Control && e.Shift && (e.KeyCode == Keys.N))
            {
                _player.Pause();
            }
            // Play
            if (e.Control && e.Shift && (e.KeyCode == Keys.M))
            {
                _player.Play();
                _timer.Start();
                _player.MediaEnded += _player_MediaEnded;
            }
        }

        private void btnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsChosen)
            {
                var screen = new Microsoft.Win32.OpenFileDialog();

                if (screen.ShowDialog() == true)
                {
                    var filename = screen.FileName;
                    var writer = new StreamWriter(filename);
                    writer.WriteLine(_playList.Count);

                    for (int i = 0; i < _playList.Count; i++)
                    {
                        writer.WriteLine(_playList[i]);
                    }

                    writer.Close();
                    System.Windows.MessageBox.Show("Da save");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Playlist đang trống");
            }
            
        }

        private void btnLoadButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();

            _playList.Clear();
            PlayList.Items.Clear();

            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;
                var reader = new StreamReader(filename);

                var count = int.Parse(reader.ReadLine());   // doc so luong bai hat

                for (int i = 0; i < count; i++)
                {
                    var fileName = reader.ReadLine().ToString();
                    _playList.Add(fileName);
                    PlayList.Items.Add(GetName(fileName));
                }

                reader.Close();
                System.Windows.MessageBox.Show("Da load");
            }
        }
    }
}

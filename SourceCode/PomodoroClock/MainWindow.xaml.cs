using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;
using PomodoroClock.Annotations;

namespace PomodoroClock
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged

    {
        public bool AnimationBreak
        {
            get { return _animationBreak; }
            set
            {
                if (this._animationBreak != value)
                {
                    this._animationBreak = value;
                    this.OnPropertyChanged(nameof(AnimationBreak));
                }

            }
        }
        public bool AnimationPomodoro
        {
            get { return _animationPomodoro; }
            set
            {
                if (this._animationPomodoro != value)
                {
                    this._animationPomodoro = value;
                    this.OnPropertyChanged(nameof(AnimationPomodoro));
                }

            }
        }

        public TimeSpan PomodoroTimeSpan { get; set; }
        public TimeSpan BreakTimeSpan { get; set; }

        /// <summary>
        /// End Time is set dynamically, depends if its pomodoro time or break time
        /// </summary>
        public DateTime EndTime { get; set; }   
        public DateTime StartTime { get; set; }
        
        DispatcherTimer _timer = new DispatcherTimer();
        private MediaPlayer _mediaPlayer = new MediaPlayer();


        private bool _animationPomodoro;
        private bool _animationBreak;
        private bool _breakTime;
        private readonly TimeSpan _timerIntervall = TimeSpan.FromMilliseconds(200);

        public MainWindow()
        {
            this.Top = Properties.Settings.Default.mainTop;  //Better solution: http://stackoverflow.com/questions/937298/restoring-window-size-position-with-multiple-monitors
            this.Left = Properties.Settings.Default.mainLeft;
            this.Height = Properties.Settings.Default.mainHeight;
            this.Width = Properties.Settings.Default.mainWidth;

            InitializeComponent();
            this.DataContext = this;

            LblMainBar.MinWidth = CalculateMainBarMinWidth();
            _timer.Tick += timer_Tick;
            _timer.Interval = _timerIntervall;
            Panele.MouseLeftButtonDown += (s, e) => DragMove();

            //Initialize
            LblMainBar.Width = this.Width - LblMainBar.Margin.Left - LblMainBar.Margin.Right - 20;
            PomodoroTimeSpan = TimeSpan.FromMinutes(25);
            BreakTimeSpan = TimeSpan.FromMinutes(5);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan remTimeSpan = EndTime - DateTime.Now;
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;

            if (_breakTime == false)
            {
                AnimationPomodoro = true;

                DrawMainBar(remTimeSpan, new TimeSpan(EndTime.Ticks - StartTime.Ticks));
                TaskbarItemInfo.ProgressValue = CalculateProgressPercentage(remTimeSpan, new TimeSpan(EndTime.Ticks - StartTime.Ticks));
            }
            else  // Break Time
            {
                AnimationBreak = true;

                // Bar has to count upwarts
                TimeSpan tmp = DateTime.Now - StartTime;
                DrawMainBar(tmp, new TimeSpan(EndTime.Ticks - StartTime.Ticks));
                TaskbarItemInfo.ProgressValue = CalculateProgressPercentage(tmp, new TimeSpan(EndTime.Ticks - StartTime.Ticks));
            }

            // Timer has finished
            if (remTimeSpan.TotalSeconds <= 0)
            {
                _timer.IsEnabled = false;
 
                AnimationBreak = false;
                AnimationPomodoro = false;

                _breakTime = _breakTime != true;  // Toggle breaktime

                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;

                //_mediaPlayer.Open();
                //_mediaPlayer.Play();

            }

            remTimeSpan += TimeSpan.FromSeconds(1);
            TbTime.Text = remTimeSpan.ToString(@"mm\:ss");

        }

        private void DrawMainBar(TimeSpan remTimeSpan, TimeSpan maxTimeSpan)
        {
            var maxFlexBarWidth = Panele.ActualWidth - LblMainBar.Margin.Left - LblMainBar.Margin.Right - CalculateMainBarMinWidth() - 10;  //10 ist der offset, unten auch 
            double percentage = CalculateProgressPercentage(remTimeSpan, maxTimeSpan);
            LblMainBar.Width = CalculateMainBarMinWidth() + 10 + (maxFlexBarWidth * percentage);
        }

        private static double CalculateProgressPercentage(TimeSpan remainingTimeSpan, TimeSpan maxTimeSpan)
        {
            var percentage = remainingTimeSpan.TotalSeconds / maxTimeSpan.TotalSeconds;

            if (percentage < 0)
            {
                percentage = 0;
            }
            else if (percentage > 1)
            {
                percentage = 1;
            }

            return percentage;
        }

        private void LblMainBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now.Add(_breakTime == false ? PomodoroTimeSpan : BreakTimeSpan);  // When we ware not on a break then start a PomodoroTimeSpan, otherwise a BreakTimeSpan

            // Possibility to skip breaks
            if (AnimationBreak == true)
            {
                EndTime = DateTime.Now;  // Fast forward
            }

            _timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawMainBar(PomodoroTimeSpan, PomodoroTimeSpan);
        }

        private double CalculateMainBarMinWidth()
        {
            return  VbTime.ActualWidth + VbTime.Margin.Left + VbTime.Margin.Right;
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true;  //http://stackoverflow.com/questions/20050426/wpf-always-on-top
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }


        private void Close_OnButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //Fensterposition und Größe abspeichern
            Properties.Settings.Default.mainTop = this.Top;
            Properties.Settings.Default.mainLeft = this.Left;
            Properties.Settings.Default.mainHeight = this.Height;
            Properties.Settings.Default.mainWidth = this.Width;

            PomodoroClock.Properties.Settings.Default.Save();
        }

        private void Panele_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Top = 20;
            this.Left = 20;
            this.Height = 100;
            this.Width = 400;
        }
    }
}

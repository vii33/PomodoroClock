using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
  
        private bool _animationPomodoro;
        private bool _animationBreak;
        private bool _breakTime;
        private double BarMinWidth = 10;
        private readonly TimeSpan _timerIntervall = TimeSpan.FromMilliseconds(200);

        public MainWindow()
        {
            this.Top = Properties.Settings.Default.mainTop;  //Better solution: http://stackoverflow.com/questions/937298/restoring-window-size-position-with-multiple-monitors
            this.Left = Properties.Settings.Default.mainLeft;
            this.Height = Properties.Settings.Default.mainHeight;
            this.Width = Properties.Settings.Default.mainWidth;

            InitializeComponent();
            this.DataContext = this;

            lblMainBar.MinWidth = BarMinWidth;
            _timer.Tick += timer_Tick;
            _timer.Interval = _timerIntervall;
            Panele.MouseLeftButtonDown += (s, e) => DragMove();
           

            //Initialize
            lblMainBar.Width = this.Width - lblMainBar.Margin.Left - lblMainBar.Margin.Right - 20;
            PomodoroTimeSpan = TimeSpan.FromMinutes(25);
            BreakTimeSpan = TimeSpan.FromMinutes(5);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan remTimeSpan = EndTime - DateTime.Now;

            if (_breakTime == false)
            {
                //Farbanimation einschalten
                AnimationPomodoro = true;

                ZeichneBalken(remTimeSpan, PomodoroTimeSpan);

                if (remTimeSpan.TotalSeconds <= 0)
                {
                    _timer.IsEnabled = false;
                    _breakTime = true;
                    AnimationPomodoro = false;
                }
            }
            else  //Zeit war vorbei --> Pausenzeit
            {
                //Neue Farbanimation einschalten
                AnimationBreak = true;


                TimeSpan tmp = DateTime.Now - StartTime;
                ZeichneBalken(tmp, BreakTimeSpan);

                if (remTimeSpan.TotalSeconds <= 0)
                {
                    _timer.IsEnabled = false;
                    _breakTime = false;
                    AnimationBreak = false;
                }
            }

            remTimeSpan += TimeSpan.FromSeconds(1);
            tbTime.Text = remTimeSpan.ToString(@"mm\:ss");

        }

        private void ZeichneBalken(TimeSpan remTimeSpan, TimeSpan maxTimeSpan)
        {
            var maxFlexBarWidth = Panele.ActualWidth - lblMainBar.Margin.Left - lblMainBar.Margin.Right - BarMinWidth - 10;  //10 ist der offset, unten auch 
            var percentage = remTimeSpan.TotalSeconds / maxTimeSpan.TotalSeconds;

            if (percentage > 0 && percentage <= 1)
            {
                lblMainBar.Width = BarMinWidth + 10 + (maxFlexBarWidth * percentage);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ZeichneBalken(PomodoroTimeSpan, PomodoroTimeSpan);
            BarMinWidth = vBTime.ActualWidth;
        }


        private void LblMainBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_breakTime == false)
            {
                EndTime = DateTime.Now.Add(PomodoroTimeSpan);
                StartTime = DateTime.Now;
            }
            else
            {
                EndTime = DateTime.Now.Add(BreakTimeSpan);
                StartTime = DateTime.Now;
            }

            _timer.Start();
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

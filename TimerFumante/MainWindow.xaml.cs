using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimerFumante
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private TimerViewModel _viewModel;
        private FumanteService fumanteService;
        private DispatcherTimer _midnightTimer;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new TimerViewModel();
            DataContext = _viewModel;
            fumanteService = new FumanteService();
            fumanteService.SetHoje(DateOnly.FromDateTime(DateTime.Now));
            fumanteService.ImportCigarrosFumados();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            _midnightTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _midnightTimer.Tick += MidnightTimer_Tick;
            _midnightTimer.Start();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            fumanteService.SaveCigarrosFumados();
            base.OnClosing(e);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _viewModel.DecreaseTime();
        }

        private void MidnightTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
            {
                fumanteService.SaveCigarrosFumados();

                fumanteService.SetHoje(DateOnly.FromDateTime(DateTime.Now));
                fumanteService.SetCigarrosFumados(0);
                _viewModel.CigarrosFumados = 0;
            }
        }

        private void ToggleInterval_Click(object sender, RoutedEventArgs e)
        {
            if (IntervalBorder.Visibility == Visibility.Collapsed && IntervalStackPanel.Visibility == Visibility.Collapsed) 
            { 
                IntervalBorder.Visibility = Visibility.Visible;
                IntervalStackPanel.Visibility = Visibility.Visible;
                ToggleIntervalButton.Content = "Esconder intervalo";
            } else
            {
                IntervalBorder.Visibility = Visibility.Collapsed;
                IntervalStackPanel.Visibility = Visibility.Collapsed;
                ToggleIntervalButton.Content = "Mostrar intervalo";
            }
        }       
        
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void FumouButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan currentTime = _viewModel.GetCurrentTime();
            _viewModel.ResetTime();
            _viewModel.IncrementTime(currentTime);
            _timer.Start();

            _viewModel.IncrementCigarrosFumados();
            fumanteService.IncrementCigarrosFumados();
            fumanteService.SaveCigarrosFumados();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetTime();
            _timer.Stop();
        }


        private void IncreaseHours_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.IncreaseHours();
        }

        private void DecreaseHours_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DecreaseHours();
        }

        private void IncreaseMinutes_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.IncreaseMinutes();
        }

        private void DecreaseMinutes_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DecreaseMinutes();
        }

        private void IncreaseSeconds_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.IncreaseSeconds();
        }

        private void DecreaseSeconds_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DecreaseSeconds();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}

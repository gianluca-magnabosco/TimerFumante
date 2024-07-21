using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TimerFumante
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private TimeSpan _time;
        private TimeSpan interval;
        private FumanteService _fumanteService;

        public TimerViewModel()
        {
            _fumanteService = new FumanteService();
            _time = ImportDefaultInterval();
            interval = ImportDefaultInterval();
        }

        public TimeSpan ImportDefaultInterval()
        {
            string fileName = $"config.txt";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"3600");
                }
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string content = reader.ReadLine();
                if (int.TryParse(content, out int importedCount))
                {
                    return TimeSpan.FromSeconds(importedCount);
                }
            }
            
            return TimeSpan.FromSeconds(3600);
        }

        public void SaveNewInterval(int value)
        {
            string fileName = $"config.txt";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            if (File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"{value}");
                }
            }
        }

        public void SaveFileCigarrosFumados()
        {
            _fumanteService.SaveCigarrosFumados();
        }

        public int CigarrosFumados
        {
            get => _fumanteService.GetCigarrosFumados();
            set => SetCigarrosFumados(value);
        }

        private void SetCigarrosFumados(int value)
        {
            _fumanteService.SetCigarrosFumados(value);
            OnPropertyChanged(nameof(CigarrosFumados));
        }

        public void IncrementCigarrosFumados()
        {
            _fumanteService.IncrementCigarrosFumados();
            OnPropertyChanged(nameof(CigarrosFumados));
        }

        public string Hours => _time.Hours.ToString("D2");
        public string Minutes => _time.Minutes.ToString("D2");
        public string Seconds => _time.Seconds.ToString("D2");

        public string IntervalHours => interval.Hours.ToString("D2");
        public string IntervalMinutes => interval.Minutes.ToString("D2");
        public string IntervalSeconds => interval.Seconds.ToString("D2");

        public void SetInterval(int interval, TimeSpan oldInterval)
        {
            this.interval = TimeSpan.FromSeconds(interval);
            SaveNewInterval(interval);
            if (oldInterval == this._time)
            {
                ResetTime();
            }
        }        
        
        public TimeSpan GetInterval()
        {
            return this.interval;
        }

        public TimeSpan GetCurrentTime()
        {
            return this._time;
        }

        public void IncrementTime(TimeSpan timeToIncrement)
        {
            _time = _time.Add(timeToIncrement);
            OnPropertyChanged(nameof(Hours));
            OnPropertyChanged(nameof(Minutes));
            OnPropertyChanged(nameof(Seconds));
        }

        public void DecreaseTime()
        {
            _time = _time.Subtract(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(Hours));
            OnPropertyChanged(nameof(Minutes));
            OnPropertyChanged(nameof(Seconds));
        }

        public void ResetTime()
        {
            _time = this.interval;
            OnPropertyChanged(nameof(Hours));
            OnPropertyChanged(nameof(Minutes));
            OnPropertyChanged(nameof(Seconds));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void IncreaseHours()
        {
            TimeSpan oldInterval = this.interval;
            interval = interval.Add(TimeSpan.FromHours(1));
            OnPropertyChanged(nameof(IntervalHours));
            OnPropertyChanged(nameof(IntervalMinutes));
            OnPropertyChanged(nameof(IntervalSeconds));
            SetInterval((int)interval.TotalSeconds, oldInterval);
        }

        public void DecreaseHours()
        {
            if (interval.Hours > 0)
            {
                TimeSpan oldInterval = this.interval;
                interval = interval.Subtract(TimeSpan.FromHours(1));
                OnPropertyChanged(nameof(IntervalHours));
                OnPropertyChanged(nameof(IntervalMinutes));
                OnPropertyChanged(nameof(IntervalSeconds));
                SetInterval((int)interval.TotalSeconds, oldInterval);
            }
        }

        public void IncreaseMinutes()
        {
            TimeSpan oldInterval = this.interval;
            interval = interval.Add(TimeSpan.FromMinutes(1));
            OnPropertyChanged(nameof(IntervalHours));
            OnPropertyChanged(nameof(IntervalMinutes));
            OnPropertyChanged(nameof(IntervalSeconds));
            SetInterval((int)interval.TotalSeconds, oldInterval);
        }

        public void DecreaseMinutes()
        {
            if (interval.TotalMinutes > 1)
            {
                TimeSpan oldInterval = this.interval;
                interval = interval.Subtract(TimeSpan.FromMinutes(1));
                OnPropertyChanged(nameof(IntervalHours));
                OnPropertyChanged(nameof(IntervalMinutes));
                OnPropertyChanged(nameof(IntervalSeconds));
                SetInterval((int)interval.TotalSeconds, oldInterval);
            }
        }

        public void IncreaseSeconds()
        {
            TimeSpan oldInterval = this.interval;
            interval = interval.Add(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(IntervalHours));
            OnPropertyChanged(nameof(IntervalMinutes));
            OnPropertyChanged(nameof(IntervalSeconds));
            SetInterval((int)interval.TotalSeconds, oldInterval);
        }

        public void DecreaseSeconds()
        {
            if (interval.TotalSeconds > 1)
            {
                TimeSpan oldInterval = this.interval;
                interval = interval.Subtract(TimeSpan.FromSeconds(1));
                OnPropertyChanged(nameof(IntervalHours));
                OnPropertyChanged(nameof(IntervalMinutes));
                OnPropertyChanged(nameof(IntervalSeconds));
                SetInterval((int)interval.TotalSeconds, oldInterval);
            }
        }
    }
}

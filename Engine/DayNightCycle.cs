using System;

namespace Engine
{
    public class DayNightCycle : IObserver<long>
    {
        private readonly GameDateTime _startGameDate;
        private GameDateTime _currentGameDate;
        private int _totalSeconds;

        public static int DayLength = 10 * 60 * 24;
        public static int HalfDayLength = DayLength/2;

        public DayNightCycle()
        {
            _startGameDate = new GameDateTime(0,0,0,8,0);
            _currentGameDate = _startGameDate;
        }

        public void OnNext(long value)
        {
            var currentTime = DateTime.UtcNow;
            var substraction = currentTime.Subtract(Game.StartDate);
            var totalSecondsDouble = substraction.TotalSeconds;
            int currentTotalSeconds = Convert.ToInt32(totalSecondsDouble);

            if (currentTotalSeconds == _totalSeconds)
                return;

            _totalSeconds = currentTotalSeconds;

            var currentTotalMinutes = (int)substraction.TotalMinutes + _startGameDate.Hour;
            var currentYear = currentTotalMinutes / (24 * 10 * 12);
            var currentWithoutYear = currentTotalMinutes % (24 * 10 * 12);

            var currentMonth = currentWithoutYear / (24 * 10);
            var currentWithoutMonth = currentWithoutYear % (24 * 10);

            var currentDay = currentWithoutMonth / (24);
            var currentWithoutDay = currentWithoutMonth % (24);

            var currentMinute = substraction.Seconds;

            _currentGameDate = new GameDateTime(currentYear, currentMonth, currentDay, currentWithoutDay, currentMinute);
        }

        public double Lightness()
        {
            if (_currentGameDate.Hour >= 10 && _currentGameDate.Hour < 20)
                return 0;

            if (_currentGameDate.Hour >= 1 && _currentGameDate.Hour < 5)
                return 0.7;

            int parts = 4;
            int minutesDiv = 60/parts;
            double fract = 1/((double)(5*parts));

            if (_currentGameDate.Hour >= 5 && _currentGameDate.Hour < 10)
            {
                return 0.7 - 0.7 * (((_currentGameDate.Hour - 5) * 60 + _currentGameDate.Minute) / minutesDiv * fract + fract);
            }

            if ((_currentGameDate.Hour >= 20 && _currentGameDate.Hour < 24) || _currentGameDate.Hour < 1)
            {
                if (_currentGameDate.Hour < 1)
                    return 0.7 * ((4 * 60 + _currentGameDate.Minute) / minutesDiv * fract + fract);

                return 0.7 * (((_currentGameDate.Hour - 20) * 60 + _currentGameDate.Minute) / minutesDiv * fract + fract);
            }

            return 0;
        }

        public GameDateTime CurrentGameDate => _currentGameDate;

        public bool IsNight()
        {
            return _currentGameDate.Hour >= 1 && _currentGameDate.Hour < 5;
        }

        public bool IsDusk()
        {
            return (_currentGameDate.Hour >= 5 && _currentGameDate.Hour < 8) || _currentGameDate.Hour < 1 || (_currentGameDate.Hour >= 22 && _currentGameDate.Hour < 24);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }

    public class GameDateTime
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int Hour { get; set; }

        public int Minute { get; set; }

        public GameDateTime(int year, int month, int day, int hour, int minute)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
        }
    }
}

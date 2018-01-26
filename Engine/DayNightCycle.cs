using System;

namespace Engine
{
    internal class DayNightCycle : IObserver<long>
    {
        private readonly GameDateTime _startGameDate;
        private GameDateTime _currentGameDate;
        private int _totalSeconds;
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
            var currentYear = (int)(currentTotalMinutes / (24 * 10 * 12));
            var currentWithoutYear = (int)(currentTotalMinutes % (24 * 10 * 12));

            var currentMonth = (int)(currentWithoutYear / (24 * 10));
            var currentWithoutMonth = (int)(currentWithoutYear % (24 * 10));

            var currentDay = (int)(currentWithoutMonth / (24));
            var currentWithoutDay = (int)(currentWithoutMonth % (24));

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
                return 0.7 - 0.7 * ((int)(((_currentGameDate.Hour - 5) * 60 + _currentGameDate.Minute) / minutesDiv) * fract + fract);
            }

            if ((_currentGameDate.Hour >= 20 && _currentGameDate.Hour < 24) || _currentGameDate.Hour < 1)
            {
                if (_currentGameDate.Hour < 1)
                    return 0.7 * ((int)((4 * 60 + _currentGameDate.Minute) / minutesDiv) * fract + fract);

                return 0.7 * ((int)(((_currentGameDate.Hour - 20) * 60 + _currentGameDate.Minute) / minutesDiv) * fract + fract);
            }

            return 0;
        }

        public GameDateTime CurrentGameDate => _currentGameDate;

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

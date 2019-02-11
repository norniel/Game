using System;

namespace Engine
{
    public class DayNightCycle : IObserver<long>
    {
        private readonly GameDateTime _startGameDate;
        private GameDateTime _currentGameDate;
        private int _totalSeconds;
        private uint _totalTicks = 0;

        public static int MinutesInHour = 60;
        public static int HoursInDay = 24;
        public static int DaysInMonth = 10;
        public static int MonthesInYear = 12;
        public static int TicksInSecond = 1000 / Game.TimeStep;
        public static int DayLength = TicksInSecond * MinutesInHour * HoursInDay;
        public static int HalfDayLength = DayLength/2;
        public static int OneSixDayLength = DayLength/6;
        public static int OneSevenDayLength = DayLength/7;
        public static int OneEightDayLength = DayLength/8;

        public DayNightCycle()
        {
            _startGameDate = new GameDateTime(0,0,0,8,0);
            _currentGameDate = _startGameDate;
        }

        public void OnNext(long value)
        {
            _totalTicks++;
            int currentTotalSeconds = (int)(_totalTicks / TicksInSecond);

           // var currentTime = DateTime.UtcNow;
           // var substraction = currentTime.Subtract(Game.StartDate);
          //  var totalSecondsDouble = substraction.TotalSeconds;
          //  int currentTotalSeconds = Convert.ToInt32(totalSecondsDouble);

            if (currentTotalSeconds == _totalSeconds)
                return;

            _totalSeconds = currentTotalSeconds;

            var currentTotalMinutes = currentTotalSeconds + _startGameDate.Hour * MinutesInHour;
            var currentYear = currentTotalMinutes / (MinutesInHour * HoursInDay * DaysInMonth * MonthesInYear);
            var currentWithoutYear = currentTotalMinutes % (MinutesInHour * HoursInDay * DaysInMonth * MonthesInYear);

            var currentMonth = currentWithoutYear / (MinutesInHour * HoursInDay * DaysInMonth);
            var currentWithoutMonth = currentWithoutYear % (MinutesInHour * HoursInDay * DaysInMonth);

            var currentDay = currentWithoutMonth / (MinutesInHour * HoursInDay);
            var currentWithoutDay = currentWithoutMonth % (MinutesInHour * HoursInDay);

            var currentHour = currentWithoutDay / (MinutesInHour);
            var currentMinute = currentWithoutDay % (MinutesInHour);

            _currentGameDate = new GameDateTime(currentYear, currentMonth, currentDay, currentHour, currentMinute);

            /*

            var currentTotalMinutes = (int)substraction.TotalMinutes + _startGameDate.Hour;
            var currentYear = currentTotalMinutes / (24 * 10 * 12);
            var currentWithoutYear = currentTotalMinutes % (24 * 10 * 12);

            var currentMonth = currentWithoutYear / (24 * 10);
            var currentWithoutMonth = currentWithoutYear % (24 * 10);

            var currentDay = currentWithoutMonth / (24);
            var currentWithoutDay = currentWithoutMonth % (24);

            var currentMinute = substraction.Seconds;

            _currentGameDate = new GameDateTime(currentYear, currentMonth, currentDay, currentWithoutDay, currentMinute);*/
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

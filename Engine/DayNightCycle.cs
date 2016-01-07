using System;

namespace Engine
{
    internal class DayNightCycle : IObserver<long>
    {
        private readonly GameDateTime startGameDate;
        private GameDateTime currentGameDate;
        private int totalSeconds = 0;
        public DayNightCycle()
        {
            startGameDate = new GameDateTime(0,0,0,3,0);
            currentGameDate = startGameDate;
        }

        public void OnNext(long value)
        {
            var currentTime = DateTime.UtcNow;
            var substraction = currentTime.Subtract(Game.StartDate);
            var totalSecondsDouble = substraction.TotalSeconds;
            int currentTotalSeconds = Convert.ToInt32(totalSecondsDouble);

            if (currentTotalSeconds == totalSeconds)
                return;

            totalSeconds = currentTotalSeconds;

            var currentTotalMinutes = (int)substraction.TotalMinutes + startGameDate.Hour;
            var currentYear = (int)(currentTotalMinutes / (24 * 10 * 12));
            var currentWithoutYear = (int)(currentTotalMinutes % (24 * 10 * 12));

            var currentMonth = (int)(currentWithoutYear / (24 * 10));
            var currentWithoutMonth = (int)(currentWithoutYear % (24 * 10));

            var currentDay = (int)(currentWithoutMonth / (24));
            var currentWithoutDay = (int)(currentWithoutMonth % (24));

            var currentMinute = substraction.Seconds;

            currentGameDate = new GameDateTime(currentYear, currentMonth, currentDay, currentWithoutDay, currentMinute);
        }

        public double Lightness()
        {
            if (currentGameDate.Hour >= 10 && currentGameDate.Hour < 20)
                return 0;

            if (currentGameDate.Hour >= 1 && currentGameDate.Hour < 5)
                return 0.7;

            int parts = 4;
            int minutesDiv = 60/parts;
            double fract = 1/((double)(5*parts));

            if (currentGameDate.Hour >= 5 && currentGameDate.Hour < 10)
            {
                return 0.7 - 0.7 * ((int)(((currentGameDate.Hour - 5) * 60 + currentGameDate.Minute) / minutesDiv) * fract + fract);
            }

            if ((currentGameDate.Hour >= 20 && currentGameDate.Hour < 24) || currentGameDate.Hour < 1)
            {
                if (currentGameDate.Hour < 1)
                    return 0.7 * ((int)((4 * 60 + currentGameDate.Minute) / minutesDiv) * fract + fract);

                return 0.7 * ((int)(((currentGameDate.Hour - 20) * 60 + currentGameDate.Minute) / minutesDiv) * fract + fract);
            }

            return 0;
        }

        public GameDateTime CurrentGameDate {
            get { return currentGameDate; }
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

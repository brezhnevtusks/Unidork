using System;

namespace Unidork.Utility
{
    public static class TimeSpanExtensions
    {
        public static string ToFormattedTimer(this TimeSpan timeSpan, string dayLabel, string hourLabel, 
                                              string minuteLabel, string secondLabel)
        {
            string formattedTimer;

            int days = timeSpan.Days;
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;
            
            if (timeSpan.Days > 0)
            {
                formattedTimer = $"{days}{dayLabel} {hours}{hourLabel}";
            }
            else if (timeSpan.Hours > 0)
            {
                formattedTimer =  $"{hours}{hourLabel} {minutes}{minuteLabel}";
            }
            else if (timeSpan.Minutes > 0)
            {
                formattedTimer = $"{minutes}{minuteLabel} {seconds}{secondLabel}";
            }
            else
            {
                formattedTimer = $"{seconds}{secondLabel}";
            }

            return formattedTimer;
        }
    }
}
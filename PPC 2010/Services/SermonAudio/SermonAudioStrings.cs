﻿namespace PPC_2010.Services.SermonAudio
{
    public static class SermonAudioStrings
    {
        public const int SeriesNameMaxLength = 30;
        public const int SermonTitleMaxLength = 85;

        public static string TruncateSeriesName(string series)
        {
            if (series.Length > SeriesNameMaxLength) return series.Substring(0, SeriesNameMaxLength);
            return series;
        }
        public static string TruncateSermonTitle(string title)
        {
            if (title.Length > SeriesNameMaxLength) return title.Substring(0, SermonTitleMaxLength);
            return title;
        }
    }
}
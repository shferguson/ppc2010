using System;

namespace PPC_2010.Services
{
    public static class SpeakerNameHelper
    {
        public static string SpeakerName(string speakerName)
        {
            var nameParts = speakerName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            return nameParts[0].Trim();
        }

        public static string FormalName(string speakerName)
        {
            var nameParts = speakerName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length > 1)
                return nameParts[1].Trim();
            return nameParts[2].Trim();
        }
    }
}
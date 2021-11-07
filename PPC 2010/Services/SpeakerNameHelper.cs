using System;

namespace PPC_2010.Services
{
    /// <summary>
    /// Speaker name is stored as "Normal Name [Formal Name]"
    /// </summary>
    public static class SpeakerNameHelper
    {
        public static string SpeakerName(string speakerName)
        {
            return GetName(speakerName, false);
        }

        public static string FormalName(string speakerName)
        {
            return GetName(speakerName, true);
        }

        private static string GetName(string speakerName, bool formal)
        {
            if (string.IsNullOrWhiteSpace(speakerName))
                return "";

            var nameParts = speakerName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (formal && nameParts.Length > 1)
                return nameParts[1].Trim();
            if (nameParts.Length > 0)
                return nameParts[0].Trim();
            return "";
        }
    }
}
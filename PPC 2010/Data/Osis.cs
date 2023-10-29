using Elmah.ContentSyndication;
using log4net;
using Lucene.Net.Search;
using StructureMap.Building;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Web;
using static Lucene.Net.Index.SegmentReader;
using static SupportClass;
using static umbraco.cms.businesslogic.ContentType;
using static Umbraco.Core.Constants;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.utilities;

namespace PPC_2010.Data
{
    public class Osis
    {
        static Dictionary<string, string> nameToId = new Dictionary<string, string>()
        {
            {"Genesis", "GEN"},
            {"Exodus", "EXO"},
            {"Leviticus", "LEV"},
            {"Numbers", "NUM"},
            {"Deuteronomy", "DEU"},
            {"Joshua", "JOS"},
            {"Judges", "JDG"},
            {"Ruth", "RUT"},
            {"1 Samuel", "1SA"},
            {"2 Samuel", "2SA"},
            {"1 Kings", "1KI"},
            {"2 Kings", "2KI"},
            {"1 Chronicles", "1CH"},
            {"2 Chronicles", "2CH"},
            {"Ezra", "EZR"},
            {"Nehemiah", "NEH"},
            {"Esther", "EST"},
            {"Job", "JOB"},
            {"Psalm", "PSA"},
            {"Proverbs", "PRO"},
            {"Ecclesiastes", "ECC"},
            {"Song of Solomon", "SNG"},
            {"Isaiah", "ISA"},
            {"Jeremiah", "JER"},
            {"Lamentations", "LAM"},
            {"Ezekiel", "EZK"},
            {"Daniel", "DAN"},
            {"Hosea", "HOS"},
            {"Joel", "JOL"},
            {"Amos", "AMO"},
            {"Obadiah", "OBA"},
            {"Jonah", "JON"},
            {"Micah", "MIC"},
            {"Nahum", "NAM"},
            {"Habakkuk", "HAB"},
            {"Zephaniah", "ZEP"},
            {"Haggai", "HAG"},
            {"Zechariah", "ZEC"},
            {"Malachi", "MAL"},
            {"Matthew", "MAT"},
            {"Mark", "MRK"},
            {"Luke", "LUK"},
            {"John", "JHN"},
            {"Acts", "ACT"},
            {"Romans", "ROM"},
            {"1 Corinthians", "1CO"},
            {"2 Corinthians", "2CO"},
            {"Galatians", "GAL"},
            {"Ephesians", "EPH"},
            {"Philippians", "PHP"},
            {"Colossians", "COL"},
            {"1 Thessalonians", "1TH"},
            {"2 Thessalonians", "2TH"},
            {"1 Timothy", "1TI"},
            {"2 Timothy", "2TI"},
            {"Titus", "TIT"},
            {"Philemon", "PHM"},
            {"Hebrews", "HEB"},
            {"James", "JAS"},
            {"1 Peter", "1PE"},
            {"2 Peter", "2PE"},
            {"1 John", "1JN"},
            {"2 John", "2JN"},
            {"3 John", "3JN"},
            {"Jude", "JUD"},
            {"Revelation", "REV"},
        };

        public static string IdForBibleBookName(string bibleBookName)
        {
            if (string.IsNullOrEmpty(bibleBookName))
                return null;

            if (nameToId.TryGetValue(bibleBookName, out var id))
                return id;
            return bibleBookName;
        }
    }
}
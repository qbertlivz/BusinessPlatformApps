using System;
using System.IO;

namespace Microsoft.Deployment.Common.Helpers
{
    public static class RandomGenerator
    {
        public static string GetDateStamp()
        {
            DateTime d = DateTime.Now;
            return FormatDate(d.Year) + FormatDate(d.Month) + FormatDate(d.Day) +
                FormatDate(d.Hour) + FormatDate(d.Minute) + FormatDate(d.Second);
        }

        public static string GetRandomCharacters()
        {
            var path = Path.GetRandomFileName() + Path.GetRandomFileName();
            return path.Replace(".", "A").Substring(0,15);
        }

        public static string GetRandomLowerCaseCharacters(int length)
        {
            Random random = new Random();
            string rasndomString = string.Empty;
            for (int i=0; i< length; i++)
            {
                int num = random.Next(0, 26); // Zero to 25
                char let = (char)('a' + num);
                rasndomString += let;
            }
            return rasndomString;
        }

        private static string FormatDate(int d)
        {
            string date = d.ToString();
            return date.Length < 2 ? "0" + date : date;
        }
    }
}
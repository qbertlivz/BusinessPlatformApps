using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Deployment.Common.Helpers
{
    public static class StringExtension
    {
        public static bool EqualsIgnoreCase(this string value, string compare)
        {
            if (value == null || compare == null)
            {
                return false;
            }

            return value.Equals(compare, StringComparison.OrdinalIgnoreCase);
        }

        public static byte[] GetUTF8Bytes(this string value)
        {
            return (value == null ? null : Encoding.UTF8.GetBytes(value));
        }

        public static string GetStringFromUTF8(this byte[] value)
        {
            return (value == null ? null : Encoding.UTF8.GetString(value));
        }

        public static List<string> SplitByCommaSpaceTabReturnList(this string value)
        {
            return value == null ? null : value.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static string[] SplitByCommaSpaceTabReturnArray(this string value)
        {
            return value == null ? null : value.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
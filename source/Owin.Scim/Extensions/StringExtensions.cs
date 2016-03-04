﻿namespace Owin.Scim.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string RemoveMultipleSpaces(this string value)
        {
            return Regex.Replace(value.Trim(), @"\s+", " ");
        }

        /// <summary>
        /// Sets first letter to uppercase, and all remaining to lowercase
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string text)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
        }
    }
}
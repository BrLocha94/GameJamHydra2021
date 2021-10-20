using System;
using System.Collections.Generic;

public static class StringExtensions
{
    public static string Truncate(this string str, int maxLength = 1)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return str.Length <= maxLength ? str : str.Substring(0, maxLength);
    }

    public static List<int> AllIndexesOf(this string str, string value)
    {
        if (String.IsNullOrEmpty(value)) throw new ArgumentException("the string to find may not be empty", "value");
        List<int> indexes = new List<int>();
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1)
                return indexes;
            indexes.Add(index);
        }
    }
}

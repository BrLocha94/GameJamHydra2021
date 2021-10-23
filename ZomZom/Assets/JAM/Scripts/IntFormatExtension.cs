using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntFormatExtension
{
    public static string FormatStringCashNoCents(this int target)
    {
        if (target < 10)
            return "$0,0" + target.ToString();
        else if (target < 100)
            return "$0," + target.ToString();

        string output = "$" + (target / 100).ToString() + ",";
        int cents = target % 100;

        if (cents < 10)
            return output + "0" + cents.ToString();

        return output + cents.ToString();
    }
}

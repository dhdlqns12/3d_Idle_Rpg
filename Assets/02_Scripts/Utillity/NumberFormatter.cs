using System;
using System.Numerics;
using UnityEngine;

public class NumberFormatter : MonoBehaviour
{
    public static string FormatNumber(BigInteger number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }

        int formatNum = 0;
        BigInteger temp = number;

        while (temp >= 1000)
        {
            temp /= 1000;
            formatNum++;
        }

        double displayValue = (double)number / Math.Pow(1000, formatNum);

        string suffix = GetSuffix(formatNum);

        return displayValue.ToString("0.###") + suffix;
    }

    private static string GetSuffix(int magnitude)
    {
        string[] baseSuffixes = { "", "K", "M", "B", "T" };

        if (magnitude < baseSuffixes.Length)
        {
            return baseSuffixes[magnitude];
        }

        return GetAlphabetSuffix(magnitude - 5);
    }

    private static string GetAlphabetSuffix(int num)
    {
        string result = "";
        num++;

        while (num > 0)
        {
            num--;
            result = (char)('a' + (num % 26)) + result;
            num /= 26;
        }

        return result;
    }

    public static string FormatGold(BigInteger gold)
    {
        return FormatNumber(gold);
    }
}

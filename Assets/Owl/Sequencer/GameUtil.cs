using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using Spacewars.Data;
//using Shared.Util;

public static class GameUtil
{
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }


    public static void ShiftLeft(int amount, ref byte[] array)
    {
        var size = array.Count();
        for (var i = 0; i < size; i++)
        {
            var index = i + amount;

            if (index < size - 1)
                array[i] = array[index];
            else
                array[i] = (byte) 0;
        }
    }

	public static string ToRoman(int number)
	{
		if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value between 1 and 3999");
		if (number < 1) return string.Empty;            
		if (number >= 1000) return "M" + ToRoman(number - 1000);
		if (number >= 900) return "CM" + ToRoman(number - 900);
		if (number >= 500) return "D" + ToRoman(number - 500);
		if (number >= 400) return "CD" + ToRoman(number - 400);
		if (number >= 100) return "C" + ToRoman(number - 100);            
		if (number >= 90) return "XC" + ToRoman(number - 90);
		if (number >= 50) return "L" + ToRoman(number - 50);
		if (number >= 40) return "XL" + ToRoman(number - 40);
		if (number >= 10) return "X" + ToRoman(number - 10);
		if (number >= 9) return "IX" + ToRoman(number - 9);
		if (number >= 5) return "V" + ToRoman(number - 5);
		if (number >= 4) return "IV" + ToRoman(number - 4);
		if (number >= 1) return "I" + ToRoman(number - 1);
		throw new ArgumentOutOfRangeException("something bad happened");
	}

	/// <summary>
	/// Tos the human number format.
	/// </summary>
	/// <returns>The human number format.</returns>
	/// <param name="number">Number.</param>
	public static string ToHumanNumberFormat(int number)
	{
		return string.Format("{0:n0}", number);
	}

    public static int Clamp(int value, int min, int max)
    {
        return Math.Max(Math.Min(value, max), min);
    }

    public static Vector3 LerpAngle(Vector3 A, Vector3 B, float p)
    {
        return new Vector3(Mathf.LerpAngle(A.x, B.x, p), Mathf.LerpAngle(A.y, B.y, p), Mathf.LerpAngle(A.z, B.z, p));        
    }

    /// <summary>
    /// Destroy all the children of the passed in transform
    /// </summary>
    /// <param name="parent"></param>
    public static void DestroyChildren(Transform parent)
    {
        foreach( Transform child in parent )
            UnityEngine.Object.Destroy(child.gameObject);
    }

    /// <summary>
    /// This is different than the Vector3.Angle, in that it gives
    /// you a signed angle between two vectors
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Angle(Vector3 a, Vector3 b)
    {
        var angle = Vector3.Angle(a, b); // calculate angle

        // assume the sign of the cross product's Y component:
        return angle * Mathf.Sign(Vector3.Cross(a, b).y);
    }

    /// <summary>
    /// This is different than the Vector3.Angle, in that it gives
    /// you a signed angle between two vectors
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float AngleY(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(b.z - a.z, b.x - a.x);
    }

    /// <summary>
    /// Get the title case for the passed in string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToTitleCase(string value)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(value); //War And Peace        
    }

    public static string ToPercent(float value)
    {
        var v = Mathf.RoundToInt(value*100.0f);
        return v + "%";
    }

	public static string ToPascalCase(this string value)
	{
		// If there are 0 or 1 characters, just return the string.
		if (value == null) return value;
		if (value.Length < 2) return value.ToUpper();

		// Split the string into words.
		string[] words = value.Split(
			new char[] { },
			StringSplitOptions.RemoveEmptyEntries);

		// Combine the words.
		string result = "";
		foreach (string word in words)
		{
			result +=
				word.Substring(0, 1).ToUpper() +
				word.Substring(1);
		}

		return result;
	}
}

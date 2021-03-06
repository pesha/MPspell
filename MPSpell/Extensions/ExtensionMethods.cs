﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSpell.Extensions
{
    public static class ExtensionsMethods
    {

        public static Dictionary<string, double> ToDictionary(this List<string> data, int defaultValue = 1)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            foreach (string item in data)
            {
                result.Add(item, defaultValue);
            }

            return result;
        }

        public static string ToStringRepresentation(this List<string> list, string separator = ",")
        {
            string res = String.Empty;

            foreach (string item in list)
            {
                res += item + separator;
            }

            return res.Length > 0 ? res.Substring(0, res.Length - separator.Length) : "";
        }

        public static void ShiftLeft(this BitArray array, bool newValue)
        {
            for (int i = 1; i < array.Count; i++)
            {
                array[i - 1] = array[i];
            }

            array[array.Count - 1] = newValue;
        }

        public static char[] ToCharArray(this string[] array)
        {
            char[] result = new char[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i][0];
            }

            return result;
        }

        public static string[] ToStringArray(this char[] array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i].ToString();
            }

            return result;
        }


    }
}

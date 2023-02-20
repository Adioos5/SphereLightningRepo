using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK___projekt_2.Containers
{
    public static class MyExtensions
    {
        public static string ReadUntilCharacter(this string s, char c)
        {
            char i = s[0];
            int k = 1;
            string result = "";
            while (i != c)
            {
                result += i;
                i = s[k];
                k++;
            }
            return result;
        }

        public static string ReadAfterCharacter(this string s, char c)
        {
            int k = s.IndexOf(c);
            char i = s[k];
            string result = "";
            while (i == c)
            {
                k++;
                i = s[k];
            }
            while (k < s.Length)
            {
                result += s[k];
                k++;
            }

            return result;
        }
    }
}

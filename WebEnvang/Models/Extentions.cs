using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WebEnvang.Models
{
    public static class Extentions
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string FirstName(this string FullName)
        {
            string[] nameParts = FullName.Split(' ');
            return nameParts[nameParts.Length - 1];
        }

        public static string LastName(this string FullName)
        {
            string[] nameParts = FullName.Split(' ');
            return nameParts[0];
        }

        public static string MiddleName(this string FullName)
        {
            string[] nameParts = FullName.Split(' ');
            string middleName = string.Empty;
            for (int i=1; i < nameParts.Length - 1; i++)
            {
                middleName = middleName + " " + nameParts[i];
            }
            return middleName.Trim();
        }

        public static Task<List<T>> ToListTask<T>(this IQueryable<T> query)
        {
            var task = Task<List<T>>.Run(() =>
            {
                return query.ToList();
            });
            return task;
        }

        public static Task<T> FirstOrDefaultTask<T>(this IQueryable<T> query)
        {
            var task = Task<dynamic>.Run(() =>
            {
                return query.FirstOrDefault();
            });
            return task;
        }
    }
}
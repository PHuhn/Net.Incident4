// 
using System;
//
namespace System
{
    public static class Extensions
    {
        //
        /// <summary>
        /// Replace in a string CR to \n
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceCR(this string str)
        {
            return str.Replace(Convert.ToChar(13).ToString(), @"\n");
        }
    }
}
//
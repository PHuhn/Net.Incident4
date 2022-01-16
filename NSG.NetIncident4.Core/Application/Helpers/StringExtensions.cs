// 
using System;
//
namespace System
{
    public static class Extensions
    {
        //
        /// <summary>
        /// Replace in a string CR and <br /> to \n
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceCRToN(this string str)
        {
            return str.Replace(Environment.NewLine, @"\n").Replace("<br />", @"\n");
        }
        //
        /// <summary>
        /// Replace in a string CR and \n to <br />
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceToBR(this string str)
        {
            return str.Replace(Environment.NewLine, "<br />").Replace(@"\n", "<br />");
        }
        //
    }
}
//
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    public static class Encryption
    {
        public static string Encrypt(string text)
        {
            return new string(text.Reverse().ToArray());
        }
    }
}

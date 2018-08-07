using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule
{
    static class ConsoleBuffer
    {
        private static StringBuilder buffer = new StringBuilder();

        public static void Add(string text)
        {
            buffer.Append(text + '\n');
        }
        public static string GetBuffer()
        {
            return buffer.ToString();
        }
        public static void Reset()
        {
            buffer.Clear();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule
{
    static class LoopStack
    {
        private static Stack<object> stack = new Stack<object>();

        public static void Push()
        {
            stack.Push(new object());
        }
        public static void Pop()
        {
            stack.Pop();
        }
        public static int Id()
        {
            return stack.Count;
        }
        public static void Reset()
        {
            stack.Clear();
        }
    }
}

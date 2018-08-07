using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Statements
{
    class ControlStatement : IStatement
    {
        //Vars
        private string control;

        //Constructor
        public ControlStatement(string text)
        {
            control = text;
        }
        public bool IsLoop()
        {
            if (control == "loop") return true;
            else return false;
        }

        //IStatement
        public void Execute()
        {
            if (control == "break")
                LoopStack.Pop();
            else if (control == "continue")
                LoopStack.Push();
            else throw new Exception("Unknown control");
        }
    }
}

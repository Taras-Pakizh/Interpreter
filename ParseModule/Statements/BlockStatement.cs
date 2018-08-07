using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Statements
{
    class BlockStatement : IStatement
    {
        //Vars
        private List<IStatement> block;
        private bool loop;
        private int id;

        //Constructor
        public BlockStatement()
        {
            block = new List<IStatement>();
            loop = false;
        }

        //IStatement
        public void AddStatement(IStatement statement)
        {
            if (statement is ControlStatement && ((ControlStatement)statement).IsLoop()) loop = true;
            else block.Add(statement);
        }
        public void Execute()
        {
            if (loop) id = LoopStack.Id();
            foreach (var item in block)
            {
                item.Execute();
                if (loop && id < LoopStack.Id())
                {
                    LoopStack.Pop();
                    break;
                }
            }
        }
    }
}

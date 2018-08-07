using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Statements
{
    class LoopStatement : IStatement
    {
        //Vars
        private IExpression condition;
        private IStatement block;
        private int id;

        //Constructor
        public LoopStatement(IExpression exp, IStatement _block)
        {
            condition = exp;
            block = _block;
        }

        //IStatement
        public void Execute()
        {
            bool Condition = false;
            object obj = condition.Evaluate().GetData();
            if (obj is bool) Condition = (bool)obj;
            else throw new Exception("Not boolen expression in Loop statement");
            LoopStack.Push();
            id = LoopStack.Id();
            while (Condition)
            {
                block.Execute();
                if (id > LoopStack.Id()) break;
                Condition = (bool)condition.Evaluate().GetData();
            }
        }
    }
}

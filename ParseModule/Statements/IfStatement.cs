using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Statements
{
    class IfStatement : IStatement
    {
        //Vars
        private IExpression condition;
        private IStatement ifstat, elsestat;

        //Constructor
        public IfStatement(IExpression exp, IStatement statif, IStatement statelse)
        {
            condition = exp;
            ifstat = statif;
            elsestat = statelse;
        }

        //IStatement
        public void Execute()
        {
            bool Condition = false;
            object obj = condition.Evaluate().GetData();
            if (obj is bool) Condition = (bool)obj;
            else throw new Exception("Not boolen expression in if statement");
            if (Condition)
                ifstat.Execute();
            else if(elsestat != null) elsestat.Execute();
        }
    }
}

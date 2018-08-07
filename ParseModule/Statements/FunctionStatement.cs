using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Statements
{
    class FunctionStatement:IStatement
    {
        //Vars
        private IExpression function;

        //Constructor
        public FunctionStatement(IExpression fun)
        {
            function = fun;
        }

        //IStatement
        public void Execute()
        {
            function.Evaluate();
        }
    }
}

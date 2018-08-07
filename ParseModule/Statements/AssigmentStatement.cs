using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Statements
{
    class AssigmentStatement : IStatement
    {
        //Vars
        private string variable;
        private IExpression expression;

        //Constructor
        public AssigmentStatement(string name, IExpression exp)
        {
            variable = name;
            expression = exp;
        }

        //IStatement
        public void Execute()
        {
            VariablesDictionary.PutVariable(variable, expression.Evaluate());
        }
    }
}

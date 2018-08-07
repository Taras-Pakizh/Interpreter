using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParseModule.Objects;

namespace ParseModule.Expressioons
{
    class FunctionExpression:IExpression
    {
        //Vars
        private string name;
        private List<IExpression> parameters;

        //Constructors
        public FunctionExpression(string _name, List<IExpression> list)
        {
            name = _name;
            parameters = list;
        }
        public FunctionExpression(string _name)
        {
            name = _name;
            parameters = new List<IExpression>();
        }

        public IExpressionData Evaluate()
        {
            return FunctionDictionary.Invoke(name, parameters);
        }
        public void AddExpression(IExpression expression)
        {
            parameters.Add(expression);
        }
    }
}

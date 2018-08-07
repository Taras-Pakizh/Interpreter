using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Expressioons
{
    class ValueExpression : IExpression
    {
        private IExpressionData data;

        public ValueExpression(string str) => data = new StringType(str);
        public ValueExpression(double number) => data = new DoubleType(number);
        public ValueExpression(bool condition) => data = new BoolType(condition);

        public IExpressionData Evaluate()
        {
            return data;
        }
    }
}

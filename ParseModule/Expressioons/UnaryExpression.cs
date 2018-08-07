using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Expressioons
{
    class UnaryExpression : IExpression
    {
        private IExpression expression;
        private char Operator;

        public UnaryExpression(char _Operator, IExpression _expression)
        {
            expression = _expression;
            Operator = _Operator;
        }

        public IExpressionData Evaluate()
        {
            IExpressionData data = expression.Evaluate();
            if (data is DoubleType)
                switch (Operator)
                {
                    case '-': return new DoubleType(-(double)data.GetData());
                    case '+': return data;
                    default: return data;
                }
            else if (data is BoolType && Operator == '!')
                return new BoolType(!(bool)data.GetData());
            else throw new Exception("Cant create unary expression. Evaluate");
        }
    }
}

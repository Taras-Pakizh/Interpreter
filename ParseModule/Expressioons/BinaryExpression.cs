using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Expressioons
{
    class BinaryExpression : IExpression
    {
        private IExpression leftExpression, rightExpression;
        private char Operation;

        public BinaryExpression(char _oper, IExpression left, IExpression right)
        {
            Operation = _oper;
            leftExpression = left;
            rightExpression = right;
        }

        public IExpressionData Evaluate()
        {
            IExpressionData leftData = leftExpression.Evaluate();
            IExpressionData rightData = rightExpression.Evaluate();
            if (leftData.GetType() != rightData.GetType())
                throw new Exception("Operation on different types");
            if (leftData is DoubleType)
            {
                double left, right;
                left = (double)leftData.GetData();
                right = (double)rightData.GetData();
                switch (Operation)
                {
                    case '+': return new DoubleType(left + right);
                    case '-': return new DoubleType(left - right);
                    case '*': return new DoubleType(left * right);
                    case '/': return new DoubleType(left / right);
                    default: throw new Exception("Unknown operation");
                }
            }
            else if (leftData is StringType)
            {
                if (Operation != '+') throw new Exception("Unknown operation on string");
                string left = (string)leftData.GetData();
                string right = (string)rightData.GetData();
                return new StringType(left + right);
            }
            else if (leftData is BoolType) throw new Exception("Cant use operator on boolen type");
            else throw new Exception("Not initialized data");
        }
    }
}

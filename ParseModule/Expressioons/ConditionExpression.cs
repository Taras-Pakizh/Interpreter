using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Expressioons
{
    class ConditionExpression : IExpression
    {
        private IExpression leftExpression, rightExpression;
        private string Operation;

        public ConditionExpression(string _oper, IExpression left, IExpression right)
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
            if(Operation == "&" || Operation == "|")
            {
                if (leftData.GetType() != typeof(BoolType)) throw new Exception("Boolen operation on none boolen variable");
                bool left = (bool)leftData.GetData();
                bool right = (bool)rightData.GetData();
                if(Operation == "&" && (left && right)) return new BoolType(true);
                else if (Operation == "|" && (left || right)) return new BoolType(true);
                else return new BoolType(false);
            }
            else if(Operation == ">" || Operation == "<" || Operation == "==" || Operation == "<=" || Operation == ">=" || Operation == "!=")
            {
                if (leftData.GetType() == typeof(BoolType)) throw new Exception("Not boolen operation on boolen type");
                if (leftData.GetType() == typeof(StringType))
                {
                    if (Operation == "==")
                    {
                        if ((string)leftData.GetData() == (string)rightData.GetData()) return new BoolType(true);
                        else return new BoolType(false);
                    }
                    else if(Operation == "!=")
                    {
                        if ((string)leftData.GetData() != (string)rightData.GetData()) return new BoolType(true);
                        else return new BoolType(false);
                    }
                    else throw new Exception("Cant use this operation on string");
                }
                else if (leftData.GetType() == typeof(DoubleType))
                {
                    double left = (double)leftData.GetData();
                    double right = (double)rightData.GetData();
                    bool result = false;
                    switch (Operation)
                    {
                        case ">": if (left > right) result = true; break;
                        case "<": if (left < right) result = true; break;
                        case ">=": if (left >= right) result = true; break;
                        case "<=": if (left <= right) result = true; break;
                        case "==": if (left == right) result = true; break;
                        case "!=": if (left != right) result = true; break;
                    }
                    return new BoolType(result);
                }
                else throw new Exception("Unknown data type");
            }
            else throw new Exception("Unknown operation in Condition expression");
        }
    }
}

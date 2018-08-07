using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using ParseModule.Objects;

namespace ParseModule.Statements
{
    class EqualStatement : IStatement
    {
        //Vars
        private string variable;
        private IExpression expression;
        private List<IExpression> ExpressionIndexes;
        private bool IsObject;
        private ArrayList indexes;

        //Constructor
        public EqualStatement(string name, IExpression exp)
        {
            variable = name;
            expression = exp;
            IsObject = false;
        }
        public EqualStatement(string name, List<IExpression> _Indexes, IExpression exp)
        {
            variable = name;
            expression = exp;
            IsObject = true;
            ExpressionIndexes = _Indexes;
        }

        //IStatement
        public void Execute()
        {
            if (!IsObject)
                VariablesDictionary.SetVariable(variable, expression.Evaluate());
            else ExecuteArray();
        }
        private void ExecuteArray()
        {
            IObject obj = VariablesDictionary.GetObject(variable);
            IObject result = obj;
            GetIndexes();
            for(int i = 0; i < indexes.Count; ++i)
            {
                if(i == indexes.Count - 1)
                {
                    if (obj is ArrayIndex && indexes[i] is int)
                        ((ArrayIndex)obj)[(int)indexes[i]] = getObject();
                    else if (obj is ArrayObject && indexes[i] is string)
                        ((ArrayObject)obj)[(string)indexes[i]] = getObject();
                    else throw new Exception("wrong index or type");
                    break;
                }
                if (obj is ArrayIndex && indexes[i] is int)
                    obj = ((ArrayIndex)obj)[(int)indexes[i]];
                else if (obj is ArrayObject && indexes[i] is string)
                    obj = ((ArrayObject)obj)[(string)indexes[i]];
                else throw new Exception("wrong index or type");
            }
            VariablesDictionary.SetVariable(variable, result);
        }
        private IObject getObject()
        {
            IExpressionData data = expression.Evaluate();
            IObject result = null;
            if (data is StringType) result = new VariableData(typeof(string), data.GetData());
            else if (data is DoubleType) result = new VariableData(typeof(double), data.GetData());
            else if (data is BoolType) result = new VariableData(typeof(bool), data.GetData());
            else if (data is ObjectType) result = (IObject)data.GetData();
            return result;
        }
        private void GetIndexes()
        {
            indexes = new ArrayList();
            IExpressionData data;
            foreach (var item in ExpressionIndexes)
            {
                data = item.Evaluate();
                if (data is StringType)
                    indexes.Add(data.GetData());
                else if (data is DoubleType)
                    indexes.Add((int)(double)data.GetData());
                else throw new Exception("Inappropriate type in brackets");
            }
        }
    }
}

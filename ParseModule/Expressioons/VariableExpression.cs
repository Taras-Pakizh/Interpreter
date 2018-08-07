using ParseModule.Objects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ParseModule.Expressioons
{
    class VariableExpression : IExpression
    {
        //Vars
        private string name;
        private bool IsObject;

        private List<IExpression> ExpressionIndexes;
        private ArrayList Indexes;

        //Constructors
        public VariableExpression(string _name)
        {
            name = _name;
            IsObject = false;
        }
        public VariableExpression(string _name, List<IExpression> _indexes)
        {
            name = _name;
            ExpressionIndexes = _indexes;
            IsObject = true;
        }

        //Methods
        public IExpressionData Evaluate()
        {
            if (!IsObject) return DefaultEvaluate();
            IExpressionData container = (new VariableExpression(name)).Evaluate();
            IObject array = null;
            if (container is ObjectType) array = (IObject)container.GetData();
            else throw new Exception("Brackets to a variable");
            GetIndexes();
            for(int i = 0; i < Indexes.Count; ++i)
            {
                if (Indexes[i] is string && array is ArrayObject)
                    array = ((ArrayObject)array)[(string)Indexes[i]];
                else if (Indexes[i] is int && array is ArrayIndex)
                    array = ((ArrayIndex)array)[(int)Indexes[i]];
                else throw new Exception("Inappropriate index in brackets");
            }
            if (array is VariableData)
                return getVariable(array);
            else throw new Exception("Inappropriate count of indexes");
        }
        private void GetIndexes()
        {
            Indexes = new ArrayList();
            IExpressionData data;
            foreach(var item in ExpressionIndexes)
            {
                data = item.Evaluate();
                if (data is StringType)
                    Indexes.Add(data.GetData());
                else if (data is DoubleType)
                    Indexes.Add((int)(double)data.GetData());
                else throw new Exception("Inappropriate type in brackets");
            }
        }

        //default
        private IExpressionData DefaultEvaluate()
        {
            IObject data = VariablesDictionary.GetObject(name);
            return getVariable(data);
        }
        private IExpressionData getVariable(IObject Data)
        {
            IExpressionData expressionData = null;
            if(Data is VariableData)
            {
                VariableData data = (VariableData)Data;
                if (typeof(Double) == data.type)
                    expressionData = new DoubleType((double)data.value);
                else if (data.type == typeof(String))
                    expressionData = new StringType((string)data.value);
                else if (data.type == typeof(Boolean))
                    expressionData = new BoolType((bool)data.value);
                else throw new Exception("Unknown data type, Expression evaluate");
            }
            else if(Data is ArrayIndex || Data is ArrayObject)
            {
                expressionData = new ObjectType(Data);
            }
            return expressionData;
        }

    }
}

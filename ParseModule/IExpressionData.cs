using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParseModule.Objects;

namespace ParseModule
{
    public interface IExpressionData
    {
        object GetData();
    }

    class DoubleType : IExpressionData
    {
        private double Value;

        public DoubleType(double number) => Value = number;
        public DoubleType(string strNumber) => Value = Double.Parse(strNumber);
        public object GetData()
        {
            return Value;
        }
    }

    class StringType : IExpressionData
    {
        private string Value;

        public StringType(string str) => Value = str;
        public StringType(double strNumber) => Value = strNumber.ToString();
        public object GetData()
        {
            return Value;
        }
    }

    class BoolType : IExpressionData
    {
        private bool Value;

        public BoolType(bool val) => Value = val;
        public BoolType(string str) => Value = Boolean.Parse(str);
        public object GetData()
        {
            return Value;
        }
    }

    class ObjectType : IExpressionData
    {
        private IObject Value;

        public ObjectType(IObject val) => Value = val;
        public object GetData()
        {
            return Value;
        }
    }
}

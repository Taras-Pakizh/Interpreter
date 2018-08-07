using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParseModule.Objects;

namespace ParseModule
{
    static class VariablesDictionary
    {
        private static Dictionary<string, IObject> dictionary = new Dictionary<string, IObject>()
        {
            ["PI"] = new VariableData(typeof(Double), Math.PI),
            ["true"] = new VariableData(typeof(Boolean), true),
            ["false"] = new VariableData(typeof(Boolean), false),
        };

        public static VariableData GetVariable(string name)
        {
            if (dictionary.ContainsKey(name) && dictionary[name] is VariableData)
                return (VariableData)dictionary[name];
            else throw new Exception("Call for unexisting variable");
        }
        public static IObject GetObject(string name)
        {
            if (dictionary.ContainsKey(name))
                return dictionary[name];
            else throw new Exception("Call for unexisting object");
        }
        public static IObject GetObject(string name, int index)
        {
            if (dictionary.ContainsKey(name) && dictionary[name] is ArrayIndex)
            {
                ArrayIndex array = (ArrayIndex)dictionary[name];
                return array[index];
            }
            else throw new Exception("Error in calling arrayindex");
        }
        public static IObject GetObject(string name, string index)
        {
            if (dictionary.ContainsKey(name) && dictionary[name] is ArrayObject)
            {
                ArrayObject array = (ArrayObject)dictionary[name];
                return array[index];
            }
            else throw new Exception("Error in calling arrayobject");
        }

        public static void Reset()
        {
            dictionary = new Dictionary<string, IObject>()
            {
                ["PI"] = new VariableData(typeof(Double), Math.PI),
                ["true"] = new VariableData(typeof(Boolean), true),
                ["false"] = new VariableData(typeof(Boolean), false),
            };
        }
        public static void PutVariable(string name, IExpressionData data)
        {
            dictionary.Add(name, getData(data));
        }
        public static void SetVariable(string name, IExpressionData data)
        {
            dictionary[name] = getData(data);
        }
        public static void SetVariable(string name, IObject data)
        {
            dictionary[name] = data;
        }
        private static IObject getData(IExpressionData data)
        {
            IObject Value = null;
            if (data is DoubleType)
                Value = new VariableData(typeof(Double), (double)data.GetData());
            else if (data is StringType)
                Value = new VariableData(typeof(String), (string)data.GetData());
            else if (data is BoolType)
                Value = new VariableData(typeof(Boolean), (bool)data.GetData());
            else if (data is ObjectType)
                Value = (IObject)data.GetData();
            else throw new Exception("Unknown data type in Variable Dictionary");
            return Value;
        }
    }

    class VariableData : IObject
    {
        //Vars
        public Type type { get; set; }
        public object value { get; set; }

        public VariableData(Type _type, object _value)
        {
            type = _type;
            value = _value;
        }
    }
}

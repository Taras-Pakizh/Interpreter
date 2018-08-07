using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParseModule.Objects;
using System.Text.RegularExpressions;
using System.IO;

namespace ParseModule
{
    delegate IExpressionData DelFunction(List<IExpression> expression);

    static class FunctionDictionary
    {
        private static Dictionary<string, DelFunction> dictionary = new Dictionary<string, DelFunction>()
        {
            ["ArrayIndex"] = ArrayIndex,
            ["ArrayObject"] = ArrayObject,
            ["ConsoleWrite"] = ConsoleWrite,
            ["FindAllWords"] = FindAllWords,
            ["FindAllIndexes"] = FindAllIndexes,
            ["FindNumbers"] = FindNumbers,
            ["FindNumbersIndexes"] = FindNumbersIndexes,
            ["IsEmail"] = IsEmail,
            ["LoadText"] = LoadText,
            ["DeleteNumbers"] = DeleteNumbers,
            ["RegExp"] = RegExp,
        };

        public static IExpressionData Invoke(string name, List<IExpression> parameters)
        {
            if (dictionary.ContainsKey(name)) return dictionary[name].Invoke(parameters);
            else throw new Exception("Called for unexisting function");
        }

        //Array and Object
        private static IExpressionData ArrayIndex(List<IExpression> parameters)
        {
            List<IObject> list = new List<IObject>();
            IExpressionData data;
            foreach (var item in parameters)
            {
                data = item.Evaluate();
                if (data is StringType) list.Add(new VariableData(typeof(string), data.GetData()));
                else if (data is BoolType) list.Add(new VariableData(typeof(bool), data.GetData()));
                else if (data is DoubleType) list.Add(new VariableData(typeof(double), data.GetData()));
                else if (data is ObjectType) list.Add((IObject)data.GetData());
            }
            ArrayIndex array = new ArrayIndex(list);
            return new ObjectType(array);
        } 
        private static IExpressionData ArrayObject(List<IExpression> parameters)
        {
            Dictionary<string, IObject> dictionary = new Dictionary<string, IObject>();
            IExpressionData data;
            for (int i = 0; i < parameters.Count; ++i)
            {
                data = parameters[i].Evaluate();
                string name = "";
                if (data is StringType)
                    name = (string)data.GetData();
                else throw new Exception("Wrong parameter. ArrayObject");
                ++i;
                data = parameters[i].Evaluate();
                if (data is StringType) dictionary.Add(name, new VariableData(typeof(string), data.GetData()));
                else if (data is BoolType) dictionary.Add(name, new VariableData(typeof(bool), data.GetData()));
                else if (data is DoubleType) dictionary.Add(name, new VariableData(typeof(double), data.GetData()));
                else if (data is ObjectType) dictionary.Add(name, (IObject)data.GetData());
            }
            ArrayObject array = new ArrayObject(dictionary);
            return new ObjectType(array);
        }
        
        //Console
        private static IExpressionData ConsoleWrite(List<IExpression> parameters)
        {
            List<string> list = new List<string>();
            IExpressionData data;
            foreach (var item in parameters)
            {
                data = item.Evaluate();
                if (data is ObjectType) throw new Exception("I cant write array. Sorry");
                if (data is StringType) list.Add((string)data.GetData());
                else if (data is BoolType) list.Add(((bool)data.GetData()).ToString());
                else if (data is DoubleType) list.Add(((double)data.GetData()).ToString());
                else throw new Exception("NULL");
            }
            string result = String.Join(" ", list);
            ConsoleBuffer.Add(result);
            return new StringType(result);
        }

        //Regex--------------------------------
        //words
        private static List<MatchCollection> FindAll(List<IExpression> parameters)
        {
            List<MatchCollection> list = new List<MatchCollection>();
            Regex regex;
            IExpressionData data;
            bool first = true;
            string word = ""; string text = "";
            foreach (var item in parameters)
            {
                data = item.Evaluate();
                if (data is ObjectType) throw new Exception("Object type in Regex parameters");
                if (data is StringType) word = (string)data.GetData();
                else if (data is DoubleType) word = ((double)data.GetData()).ToString();
                else if (data is BoolType) word = ((bool)data.GetData()).ToString();
                if (first)
                {
                    text = word;
                    first = false;
                    continue;
                }
                word = @"\w*" + word + @"\w*";
                regex = new Regex(@word);
                list.Add(regex.Matches(text));
            }
            return list;
        }
        private static IExpressionData FindAllWords(List<IExpression> parameters)
        {
            var list = FindAll(parameters);
            List<string> words = new List<string>();
            foreach (var item in list)
                if (item.Count > 0)
                    foreach (Match match in item)
                        words.Add(match.Value);
            return new StringType(String.Join(" ", words));
        }
        private static IExpressionData FindAllIndexes(List<IExpression> parameters)
        {
            var list = FindAll(parameters);
            List<string> indexes = new List<string>();
            foreach (var item in list)
                if (item.Count > 0)
                    foreach (Match match in item)
                        indexes.Add(match.Index.ToString());
            return new StringType(String.Join(" ", indexes));
        }
        //numbers
        private static string FindNumbersSub(List<IExpression> parameters, bool index)
        {
            List<string> result = new List<string>();
            Regex regex;
            IExpressionData data; string text = "";
            foreach (var item in parameters)
            {
                data = item.Evaluate();
                if (data is StringType)
                {
                    text = (string)data.GetData();
                    regex = new Regex(@"\d+");
                    MatchCollection collection = regex.Matches(text);
                    if (collection.Count > 0 && index)
                        foreach (Match match in collection)
                            result.Add(match.Index.ToString());
                    else if(collection.Count > 0 && !index)
                        foreach (Match match in collection)
                            result.Add(match.Value);
                }
                else throw new Exception("parameters should be string");
            }
            return String.Join(" ", result);
        }
        private static IExpressionData FindNumbers(List<IExpression> parameters)
        {
            return new StringType(FindNumbersSub(parameters, false));
        }
        private static IExpressionData FindNumbersIndexes(List<IExpression> parameters)
        {
            return new StringType(FindNumbersSub(parameters, true));
        }
        private static IExpressionData DeleteNumbers(List<IExpression> parameters)
        {
            List<string> result = new List<string>();
            Regex regex;
            IExpressionData data; string text = "";
            foreach (var item in parameters)
            {
                data = item.Evaluate();
                if (data is StringType)
                {
                    text = (string)data.GetData();
                    regex = new Regex(@"\d+");
                    result.Add(regex.Replace(text, ""));
                }
                else throw new Exception("parameters should be string");
            }
            return new StringType(String.Join(" ", result));
        }
        //email
        private static IExpressionData IsEmail(List<IExpression> parameters)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            List<string> result = new List<string>();
            IExpressionData data; string text = "";
            foreach (var item in parameters)
            {
                data = item.Evaluate();
                if (data is StringType)
                {
                    text = (string)data.GetData();
                    if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase))
                        result.Add(text);
                }
                else throw new Exception("parameters should be string");
            }
            return new StringType(String.Join(" ", result));
        }
        //File
        private static IExpressionData LoadText(List<IExpression> parameters)
        {
            if (parameters.Count == 0) throw new Exception("Empty");
            IExpressionData data = parameters[0].Evaluate();
            string path = "";
            if (data is StringType) path = (string)data.GetData();
            else throw new Exception("Not string");
            string text = "";
            using(StreamReader sr = new StreamReader(path))
                text = sr.ReadToEnd();
            return new StringType(text);
        }

        private static IExpressionData RegExp(List<IExpression> parameters)
        {
            List<string> result = new List<string>();
            Regex regex;
            IExpressionData data; string text = "";
            if (parameters.Count < 2) throw new Exception("Wrong");
            data = parameters[0].Evaluate();
            if (data is StringType)
            {
                text = (string)data.GetData();
            }
            else throw new Exception();
            string patern = "";
            data = parameters[1].Evaluate();
            if (data is StringType)
            {
                patern = (string)data.GetData();
            }
            else throw new Exception();
            regex = new Regex(@patern);
            MatchCollection collection = regex.Matches(text);
            if (collection.Count > 0)
                foreach (Match match in collection)
                    result.Add(match.Value.ToString());

            return new StringType(String.Join(" ", result));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Objects
{
    class ArrayObject:IObject
    {
        //Vars
        protected Dictionary<string, IObject> dictionary;

        //Properties
        public int Count { get { return dictionary.Count; } }
        public IObject this[string index]
        {
            get
            {
                if (dictionary.ContainsKey(index)) return dictionary[index];
                else return null;
            }
            set
            {
                if (dictionary.ContainsKey(index)) dictionary[index] = value;
                else dictionary.Add(index, value);
            }
        }

        //Constructor
        public ArrayObject() => dictionary = new Dictionary<string, IObject>();
        public ArrayObject(Dictionary<string, IObject> _dictionary) => dictionary = _dictionary;

        //Methods
        public void Add(string name, IObject item) => dictionary.Add(name, item);
    }
}

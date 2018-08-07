using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseModule.Objects
{
    class ArrayIndex:IObject
    {
        //Vars
        protected List<IObject> list;

        //Properties
        public int Count { get { return list.Count; } }
        public IObject this[int index]
        {
            get
            {
                if (index >= 0 && index < Count)
                    return list[index];
                else throw new Exception("Array. Index out of range");
            }
            set
            {
                if (index >= Count) Add(value);
                else list[index] = value;
            }
        }

        //Constructor
        public ArrayIndex() => list = new List<IObject>();
        public ArrayIndex(List<IObject> _list) => list = _list;
        public ArrayIndex(IObject[] array) => list.AddRange(array);

        //Methods
        public void Add(IObject item) => list.Add(item);
    }
}

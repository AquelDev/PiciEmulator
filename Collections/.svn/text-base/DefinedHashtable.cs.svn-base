using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Butterfly.Collections
{
    class DefinedHashtable<T,V> : Hashtable
    {
        public DefinedHashtable()
            : base()
        { }

        internal V this[T key]
        {
            get
            {
                if (base.ContainsKey(key))
                    return (V)base[key];
                else return default(V);
            }
        }

        public void Add(T key, T value)
        {
            base.Add(key, value);
        }

        public void Remove(T key)
        {
            base.Remove(key);
        }

        public override ICollection Values
        {
            get
            {
                return base.Values;
            }
        }

        public override void Clear()
        {
            base.Clear();
        }

        public override bool Contains(object key)
        {
            return base.Contains(key);
        }

        public bool ContainsKey(T key)
        {
            return base.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            return base.ContainsValue(value);
        }
        
        
    }
}

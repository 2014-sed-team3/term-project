using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.AppLib
{
    public class AttributesDictionary<TValue> : Dictionary<AttributeUtils.Attribute, TValue>
    {
        private int index;

        public AttributesDictionary()
            : base()
        {
            index = 0;
            foreach (AttributeUtils.Attribute oAttribute in AttributeUtils.UserAttributes)
            {
                this.Add(oAttribute, default(TValue));
            }            
        }        

        public TValue this[string sKey]
        {
            get { return this[Keys.First(x => x.value.Equals(sKey))]; }
            set { this[Keys.First(x => x.value.Equals(sKey))] = value; }
        }

        public void Add(string sKey, TValue value)
        {
            this[Keys.First(x => x.value.Equals(sKey))] = value;
        }

        public void Add(TValue value)
        {
            if (index < Keys.Count)
            {
                this[Keys.ElementAt(index)] = value;
                index++;
            }
        }

        public void Add(List<TValue> values)
        {
            if(values.Count!=Keys.Count)
                throw new Exception("Values count must match keys count");

            for(int i=0;i<values.Count;i++)
            {
                this[Keys.ElementAt(i)] = values[i];
            }
        }

        public bool ContainsKey(string sKey)
        {
            try
            {
                this.Keys.First(x => x.value.Equals(sKey));
                return true;
            }
            catch(Exception e)
            {
                return false;
            }            
        }
    }        
}

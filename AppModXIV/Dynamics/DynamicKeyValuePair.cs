using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace AppModXIV.Dynamics
{
    internal sealed class DynamicKeyValuePair<TKey, TValue> : DynamicObject
    {
        private KeyValuePair<TKey, TValue> kvp;

        internal DynamicKeyValuePair(KeyValuePair<TKey, TValue> item)
        {
            this.kvp = item;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name;
            if (this.kvp.Key.ToString() == key)
            {
                result = this.kvp.Value;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public TKey Key
        {
            get
            {
                return this.kvp.Key;
            }
        }

        public TValue Value
        {
            get
            {
                return this.kvp.Value;
            }
        }
    }
}

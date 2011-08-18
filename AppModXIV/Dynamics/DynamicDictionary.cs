using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace AppModXIV.Dynamics
{
    /// <summary>
    /// An implementation of a <see cref="System.Collections.Generic.IDictionary&lt;string, TValue&gt;"/> which uses dynamics to allow property accessors
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class inherits from <see cref="System.Dynamic.DynamicObject"/> and <see cref="System.Collections.Generic.IDictionary&lt;string, TValue&gt;"/> (to give default dictionary features as well).
    /// It allows you to have a dictionary which you access the key store via standard dot-notation. This is exposed via extension methods for users to create.
    /// </para>
    /// <example>
    ///     var dictionary = new Dictionary<string, string> { { "hello", "world" } };
    ///     var dynamicDictionary = dictionary.AsDynamic();
    ///     //access data
    ///     var local = dynamicDictionary.hello;
    ///     //create new key
    ///     dynamicDictionary.newValue = "I'm new!";
    /// </example>
    /// </remarks>
    /// <typeparam name="string">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public sealed class DynamicDictionary<TValue> : DynamicObject, IDictionary<string, TValue>
    {
        private IDictionary<string, TValue> dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDictionary&lt;string, TValue&gt;"/> class.
        /// </summary>
        public DynamicDictionary()
            : this(new Dictionary<string, TValue>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDictionary&lt;string, TValue&gt;"/> class from a given dictionary
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public DynamicDictionary(IDictionary<string, TValue> dictionary)
        {
            //set the internal dictionary instance
            this.dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name;
            //check if the key exists in the dictionary
            if (dictionary.ContainsKey(key))
            {
                //set it and return true to indicate its found
                result = dictionary[key];
                return true;
            }
            //look into the base implementation, it might be there
            var found = base.TryGetMember(binder, out result);

            //if it wasn't found we'll raise an exception
            if (!found)
                throw new KeyNotFoundException(string.Format("Key \"{0}\" was not found in the given dictionary", key));

            return found;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("This dictionary instance is read-only, you cannot modify the data it contains");

            var key = binder.Name;
            //check if the dictionary already has this key
            if (dictionary.ContainsKey(key))
            {
                //it did so we can assign it to the new value
                dictionary[key] = (TValue)value;
                return true;
            }
            else
            {
                //check the base for the property
                var found = base.TrySetMember(binder, value);
                //if it wasn't found then the user must have wanted a new key
                //we'll expect implicit casting here, and an exception will be raised
                //if it cannot explicitly cast
                if (!found)
                    dictionary.Add(key, (TValue)value);

                return true;
            }
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            var key = binder.Name;

            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                return true;
            }
            else
            {
                //throw new KeyNotFoundException(string.Format("Key \"{0}\" was not found in the given dictionary", key));
                return false;
            }
        }

        //public void Add(DynamicKeyValuePair<string, TValue> item)
        //{
        //    this.dictionary.Add(new KeyValuePair<string, TValue>(item.Key, item.Value));
        //}

        #region IDictionary<string,TValue> Members

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, TValue value)
        {
            this.dictionary.Add(key, value);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<TValue> Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TValue"/> with the specified key.
        /// </summary>
        /// <value></value>
        public TValue this[string key]
        {
            get
            {
                return this.dictionary[key];
            }
            set
            {
                this.dictionary[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string,TValue>> Members

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<string, TValue> item)
        {
            this.dictionary.Add(item);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.dictionary.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return this.dictionary.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return this.dictionary.IsReadOnly;
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return this.dictionary.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,TValue>> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}

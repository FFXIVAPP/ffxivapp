using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppModXIV.Dynamics
{
    public static class DynamicExtensions
    {
        /// <summary>
        /// Converts a dictionary into a dynamic implementation
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>
        /// A dynamic implementation of the dictionary using <see cref="DynamicDictionary&lt;TKey, TValue&gt;"/>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the dictionary is null</exception>
        public static dynamic AsDynamic<TValue>(this IDictionary<string, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary", "Dictionary cannot be null");
            return new DynamicDictionary<TValue>(dictionary);
        }

        /// <summary>
        /// Converts a KeyValuePair into a dynamic implementation
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static dynamic AsDynamic<TKey, TValue>(this KeyValuePair<TKey, TValue> item)
        {
            return new DynamicKeyValuePair<TKey, TValue>(item);
        }
    }
}

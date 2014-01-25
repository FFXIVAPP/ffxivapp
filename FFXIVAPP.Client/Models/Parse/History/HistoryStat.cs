// FFXIVAPP.Client
// HistoryStat.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse.History
{
    [DoNotObfuscate]
    public class HistoryStat<T>
    {
        /// <summary>
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        protected HistoryStat(string name = "", T value = default(T))
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public T Value { get; set; }
    }
}

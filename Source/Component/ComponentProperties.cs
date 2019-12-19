using System.Collections;
using System.Collections.Generic;

namespace SharpEngine.Component
{
    public class ComponentProperties : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly IDictionary<string, object> _internal = new Dictionary<string, object>();

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _internal.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _internal.GetEnumerator();

        public object this[string key]
        {
            get => _internal[key];
            set => Add(key, value);
        }

        public void Add(string key, object property)
        {
            _internal[key] = property;
        }
    }
}

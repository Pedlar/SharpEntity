using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine
{
    public interface IPool<T>
    {
        // Create Item in the Pool
        T Create();

        // Get Item from the Pool
        T Get(ulong index);

        // Remote Item from the Pool
        void Remove(T item);

        // Does Pool Contain Item
        bool Contains(T item);

        // Get Size of Pool
        int GetSize();

        // Checks if the item is still valid
        bool IsValid(T item);

        // Clear the Pool
        void Clear();
    }
}

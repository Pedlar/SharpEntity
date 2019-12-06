using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine
{

    public enum CacheDesignation { ALIVE, ACTIVACTION, DEACTIVATION, ZOMBIE }

    public interface ICache<T>
    {
        void Add(CacheDesignation cacheDesignation, T item);
        T Last(CacheDesignation cacheDesignation);
        void ClearTemp();
        void Clear();
        List<Entity> List(CacheDesignation cacheDesignation);
    }
}

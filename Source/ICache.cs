using System.Collections.Generic;

namespace SharpEngine
{

    public enum CacheDesignation { ALIVE, ACTIVACTION, DEACTIVATION, ZOMBIE }

    public interface ICache<T>
    {
        void Add(CacheDesignation cacheDesignation, T item);
        T Last(CacheDesignation cacheDesignation);
        void ClearTemp();
        void Clear();
        List<IEntity> List(CacheDesignation cacheDesignation);
    }
}

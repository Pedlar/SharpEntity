using System.Collections.Generic;
using System.Linq;

namespace SharpEngine
{
    class EntityCache : ICache<IEntity>
    {
        List<IEntity> alive;  // List of Entities
        List<IEntity> awaitingActivation; // List of Entities Awaiting Activation
        List<IEntity> awaitingDeactivation; // List of Entities Awaiting to be Deactivated
        List<IEntity> zombie; // Cleared on Refresh

        public EntityCache()
        {
            alive = new List<IEntity>();
            awaitingActivation = new List<IEntity>();
            awaitingDeactivation = new List<IEntity>();
            zombie = new List<IEntity>();
        }

        public void Add(CacheDesignation cacheDesignation, IEntity entity)
        {
            switch(cacheDesignation)
            {
                case CacheDesignation.ALIVE:
                    alive.Add(entity);
                    break;
                case CacheDesignation.ZOMBIE:
                    zombie.Add(entity);
                    break;
                case CacheDesignation.ACTIVACTION:
                    awaitingActivation.Add(entity);
                    break;
                case CacheDesignation.DEACTIVATION:
                    awaitingDeactivation.Add(entity);
                    break;
            }
        }

        public IEntity Last(CacheDesignation cacheDesignation)
        {
            switch(cacheDesignation)
            {
                case CacheDesignation.ALIVE:
                    return alive.Last();
                case CacheDesignation.ZOMBIE:
                    return zombie.Last();
                case CacheDesignation.ACTIVACTION:
                    return awaitingActivation.Last();
                case CacheDesignation.DEACTIVATION:
                    return awaitingDeactivation.Last();
            }

            return null;
        }

        public List<IEntity> List(CacheDesignation cacheDesignation)
        {
            switch (cacheDesignation)
            {
                case CacheDesignation.ALIVE:
                    return alive;
                case CacheDesignation.ZOMBIE:
                    return zombie;
                case CacheDesignation.ACTIVACTION:
                    return awaitingActivation;
                case CacheDesignation.DEACTIVATION:
                    return awaitingDeactivation;
                default:
                    break;
            }

            return new List<IEntity>();
        }

        public void ClearTemp()
        {
            zombie.Clear();
            awaitingActivation.Clear();
            awaitingDeactivation.Clear();
        }

        public void Clear()
        {
            alive.Clear();
            ClearTemp();
        }
    }
}

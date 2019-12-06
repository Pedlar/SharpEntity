using System.Collections.Generic;
using System.Linq;

namespace SharpEngine
{
    class EntityCache : ICache<Entity>
    {
        List<Entity> alive;  // List of Entities
        List<Entity> awaitingActivation; // List of Entities Awaiting Activation
        List<Entity> awaitingDeactivation; // List of Entities Awaiting to be Deactivated
        List<Entity> zombie; // Cleared on Refresh

        public EntityCache()
        {
            alive = new List<Entity>();
            awaitingActivation = new List<Entity>();
            awaitingDeactivation = new List<Entity>();
            zombie = new List<Entity>();

        }


        public void Add(CacheDesignation cacheDesignation, Entity entity)
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

        public Entity Last(CacheDesignation cacheDesignation)
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

        public List<Entity> List(CacheDesignation cacheDesignation)
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

            return new List<Entity>();
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

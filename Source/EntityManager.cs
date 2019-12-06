using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpEngine
{
    public class EntityManager
    {
        private readonly ICache<Entity> entityCache;
        private readonly IPool<Entity.Id> entityIdPool;
        private readonly IDictionary<ulong, Entity.Attributes> entityAttributes;

        public EntityManager(ICache<Entity> _entityCache = null, IPool<Entity.Id> _entityIdPool = null)
        {
            entityCache = _entityCache == null ? new EntityCache() : _entityCache;
            entityIdPool = _entityIdPool == null ? new EntityIdPool() : _entityIdPool;
            entityAttributes = new Dictionary<ulong, Entity.Attributes>();
        }

        public int EntityCount {
            get {
                return entityCache.List(CacheDesignation.ALIVE).Count();
            }
        }

        public Entity CreateEntity()
        {
            var entity = new Entity(this, entityIdPool.Create());
            entityCache.Add(CacheDesignation.ALIVE, entity);
            entityAttributes[entity.entityId.Index] = new Entity.Attributes();
            return entityCache.Last(CacheDesignation.ALIVE);
        }

        // Create an Entity and Run templateFunc(Entity entity) and return the modified entity
        public Entity CreateEntity(Func<Entity, Entity> templateFunc)
        {
            Entity entity = CreateEntity();
            return templateFunc(entity);
        }

        public Entity Get(Entity.Id entityId)
        {
            if (!entityIdPool.IsValid(entityId))
            {
                throw new Exception("Invalid EntityId"); 
            }

            return new Entity(this, entityId);
        }

        public bool IsValid(Entity entity)
        {
            return entityIdPool.IsValid(entity.entityId);
        }

        public void Activate(Entity entity) => entityCache.Add(CacheDesignation.ACTIVACTION, entity);
        public void Deactivate(Entity entity) => entityCache.Add(CacheDesignation.DEACTIVATION, entity);
        public void Desotry(Entity entity) => entityCache.Add(CacheDesignation.ZOMBIE, entity);

        public bool IsActivated(Entity entity) => entityAttributes[entity.entityId.Index].Activated;

        public List<Entity> GetEntities()
        {
            return entityCache.List(CacheDesignation.ALIVE);
        }

        public void Update()
        {
            GetAwaitingActivationEntities().ForEach(entity =>
            {
                var attr = entityAttributes[entity.entityId.Index];
                attr.Activated = true;
            });

            GetAwaitingDeactivationEntities().ForEach(entity =>
            {
                var attr = entityAttributes[entity.entityId.Index];
                attr.Activated = false;
            });

            GetZombieEntities().ForEach(entity =>
            {
                entityCache.List(CacheDesignation.ALIVE).Remove(entity);
                entityAttributes.Remove(entity.entityId.Index);
                entityIdPool.Remove(entity.entityId);
            });

            entityCache.ClearTemp();
        }

        public void Clear()
        {
            entityCache.Clear();
            entityIdPool.Clear();
        }

        private List<Entity> GetZombieEntities()
        {
            return entityCache.List(CacheDesignation.ZOMBIE);
        }

        private List<Entity> GetAwaitingActivationEntities()
        {
            return entityCache.List(CacheDesignation.ACTIVACTION);
        }

        private List<Entity> GetAwaitingDeactivationEntities()
        {
            return entityCache.List(CacheDesignation.DEACTIVATION);
        }
    }
}

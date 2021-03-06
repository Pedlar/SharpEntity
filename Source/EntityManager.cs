﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpEngine
{
    public static class CacheHelperExtensions
    {
        public static void AddAlive(this ICache<IEntity> cache, IEntity entity) => cache.Add(CacheDesignation.ALIVE, entity);
        public static void AddAwaitingActivation(this ICache<IEntity> cache, IEntity entity) => cache.Add(CacheDesignation.ACTIVACTION, entity);
        public static void AddAwaitingDeactivation(this ICache<IEntity> cache, IEntity entity) => cache.Add(CacheDesignation.DEACTIVATION, entity);
        public static void AddZombie(this ICache<IEntity> cache, IEntity entity) => cache.Add(CacheDesignation.ZOMBIE, entity);

        public static void RemoveAlive(this ICache<IEntity> cache, IEntity entity) => cache.List(CacheDesignation.ALIVE).Remove(entity);

        public static IEntity GetLastAlive(this ICache<IEntity> cache) => cache.Last(CacheDesignation.ALIVE);
        public static List<IEntity> GetAlive(this ICache<IEntity> cache) => cache.List(CacheDesignation.ALIVE);
        public static List<IEntity> GetAwaitingActivation(this ICache<IEntity> cache) => cache.List(CacheDesignation.ACTIVACTION);
        public static List<IEntity> GetAwaitingDeactivation(this ICache<IEntity> cache) => cache.List(CacheDesignation.DEACTIVATION);
        public static List<IEntity> GetZombies(this ICache<IEntity> cache) => cache.List(CacheDesignation.ZOMBIE);
    }

    public class EntityAttributes
    {
        public bool Activated { get; set; }
    }

    public class EntityComponents
    {
        public IDictionary<Type, IComponent> Components;

        public EntityComponents()
        {
            Components = new Dictionary<Type, IComponent>();
        }
    }

    public class EntityEventArgs : EventArgs
    {
        public IEntity Entity { get; set; }
    }

    public class EntityManager : IDisposable
    {
        public event EventHandler<EntityEventArgs> OnActivated;
        public event EventHandler<EntityEventArgs> OnDeactivated;
        public event EventHandler<EntityEventArgs> OnDestroy;

        private readonly EntityFactory entityFactory;
        private readonly ICache<IEntity> entityCache;
        private readonly IPool<IEntityId> entityIdPool;
        private readonly IDictionary<ulong, EntityAttributes> entityAttributes;
        private readonly IDictionary<ulong, EntityComponents> entityComponents;

        public EntityManager(ICache<IEntity> _entityCache = null, IPool<IEntityId> _entityIdPool = null, EntityFactory _entityFactory = null)
        {
            entityCache = _entityCache == null ? new EntityCache() : _entityCache;
            entityIdPool = _entityIdPool == null ? new EntityIdPool() : _entityIdPool;
            entityFactory = _entityFactory == null ? new EntityFactory(this, entityIdPool) : _entityFactory;

            entityAttributes = new Dictionary<ulong, EntityAttributes>();
            entityComponents = new Dictionary<ulong, EntityComponents>();
        }

        public int EntityCount {
            get {
                return entityCache.GetAlive().Count();
            }
        }

        public IEntity CreateEntity(Action<IEntity> configure = null)
        {
            var entity = entityFactory.Create();
            entityCache.AddAlive(entity);
            entityAttributes[entity.Id.Index] = new EntityAttributes();
            entityComponents[entity.Id.Index] = new EntityComponents();

            configure?.Invoke(entity);

            foreach(IComponent component in entityComponents[entity.Id.Index].Components.Values)
            {
                component.Setup();
            }

            return entity;
        }

        // Create an Entity and Run templateFunc(Entity entity) and return the modified entity
        public IEntity CreateEntity(Func<IEntity, IEntity> templateFunc)
        {
            IEntity entity = CreateEntity();
            return templateFunc(entity);
        }

        public IEntity Get(IEntityId entityId)
        {
            if (!entityIdPool.IsValid(entityId))
            {
                throw new Exception("Invalid EntityId"); 
            }

            return entityFactory.Create(_id: entityId);
        }

        public List<IEntity> GetEntities()
        {
            return entityCache.GetAlive();
        }

        public IEnumerable<IEntity> GetEntitiesWithComponents<T>()
            =>  from entity in entityCache.GetAlive()
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T))
                select entity;

        public IEnumerable<IEntity> GetEntitiesWithComponents<T1, T2>()
            =>  from entity in entityCache.GetAlive()
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T1))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T2))
                select entity; 

        public IEnumerable<IEntity> GetEntitiesWithComponents<T1, T2, T3, T4>()
            => from entity in entityCache.GetAlive()
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T1))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T2))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T3))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T4))
                select entity;

        public IEnumerable<IEntity> GetEntitiesWithComponents<T1, T2, T3, T4, T5>()
            => from entity in entityCache.GetAlive()
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T1))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T2))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T3))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T4))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T5))
                select entity;

        public IEnumerable<IEntity> GetEntitiesWithComponents<T1, T2, T3, T4, T5, T6>()
            => from entity in entityCache.GetAlive()
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T1))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T2))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T3))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T4))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T5))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T6))
                select entity;

        public IEnumerable<IEntity> GetEntitiesWithComponents<T1, T2, T3, T4, T5, T6, T7>()
            => from entity in entityCache.GetAlive()
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T1))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T2))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T3))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T4))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T5))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T6))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T7))
                select entity;

        public IEnumerable<IEntity> GetEntitiesWithComponents<T1, T2, T3, T4, T5, T6, T7, T8>()
            => from entity in entityCache.GetAlive()
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T1))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T2))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T3))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T4))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T5))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T6))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T7))
                where entityComponents[entity.Id.Index].Components.ContainsKey(typeof(T8))
                select entity;

        public void Update()
        {
            entityCache.GetAwaitingActivation().ForEach(entity =>
            {
                var attrs = entityAttributes[entity.Id.Index];
                attrs.Activated = true;
                OnActivated?.Invoke(this, new EntityEventArgs() { Entity = entity });
            });

            entityCache.GetAwaitingDeactivation().ForEach(entity =>
            {
                var attrs = entityAttributes[entity.Id.Index];
                attrs.Activated = false;
                OnDeactivated?.Invoke(this, new EntityEventArgs() { Entity = entity });
            });

            entityCache.GetZombies().ForEach(entity =>
            {
                OnDestroy?.Invoke(this, new EntityEventArgs() { Entity = entity });
                entity.Destroy();
                entityCache.RemoveAlive(entity);
                entityAttributes.Remove(entity.Id.Index);
                entityComponents.Remove(entity.Id.Index);
                entityIdPool.Remove(entity.Id);
            });

            entityCache.ClearTemp();
        }

        public void Clear()
        {
            foreach (var entity in entityCache.GetAlive()
                .Concat(entityCache.GetAwaitingActivation())
                .Concat(entityCache.GetAwaitingDeactivation())
                .Concat(entityCache.GetZombies()))
            {
                OnDestroy?.Invoke(this, new EntityEventArgs() { Entity = entity });
                entity.Destroy();
            }

            entityCache.Clear();
            entityAttributes.Clear();
            entityComponents.Clear();
            entityIdPool.Clear();
        }

        /*
        * These are for coupling Entity to ID, shouldn't call these directly, only Entity should call them. 
        */
        #region delegationmethods
        #region entity

        public void Activate(IEntity entity) => entityCache.AddAwaitingActivation(entity);
        public void Deactivate(IEntity entity) => entityCache.AddAwaitingDeactivation(entity);
        public void Desotry(IEntity entity) => entityCache.AddZombie(entity);
        public bool IsActivated(IEntity entity) => entityAttributes[entity.Id.Index].Activated;
        public bool IsValid(IEntity entity) => entityIdPool.IsValid(entity.Id);

        #endregion entity
        #region components

        internal T GetComponent<T>(IEntity entity, Type componentType)
            => (T)entityComponents[entity.Id.Index].Components[componentType];

        internal List<IComponent> GetComponents(IEntity entity)
            => entityComponents[entity.Id.Index].Components.Values.ToList();

        internal void AddComponent(IEntity entity, IComponent componenet, Type componentType) 
            => entityComponents[entity.Id.Index].Components[componentType] = componenet;

        internal void RemoveComponent(IEntity entity, Type componentType) 
            => entityComponents[entity.Id.Index].Components.Remove(componentType);

        internal bool HasComponent(IEntity entity, Type componentType) 
            => entityComponents[entity.Id.Index].Components.ContainsKey(componentType);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Clear();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #endregion components
        #endregion delegationmethods
    }
}

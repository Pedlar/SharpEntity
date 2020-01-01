
using System;
using System.Collections.Generic;

namespace SharpEngine
{
    using SharpEngine.Component;
    using System.Reflection;
    using static Utilities.DictionaryExtensions;
    using static Utilities.EnumerableExtensions;
    using static Utilities.StandardExtensions;

    public class Entity : IEntity
    {
        public Entity(EntityManager manager, IEntityId id)
        {
            _entityManager = manager;
            _id = id;
        }

        public static bool operator ==(Entity left, Entity right) {
            return right.Manager == left.Manager && right.Id == left.Id;
        }

        public static bool operator !=(Entity left, Entity right) => !(left == right);
        public static bool operator <(Entity left, Entity right) => left.Id.Value() < right.Id.Value();
        public static bool operator >(Entity left, Entity right) => left.Id.Value() > right.Id.Value();
        public static bool operator <=(Entity left, Entity right) => left.Id.Value() <= right.Id.Value();
        public static bool operator >=(Entity left, Entity right) => left.Id.Value() >= right.Id.Value();

        public bool IsActive
        {
            get
            {
                return Manager.IsActivated(this);
            }
            set
            {
                if(value)
                {
                    Manager.Activate(this);
                }
                else
                {
                    Manager.Deactivate(this);
                }
            }
        }

        private EntityManager _entityManager;
        public EntityManager Manager { get { return _entityManager; } set { throw new InvalidArgumentException("Cannot Set Manager through Setter"); } }
        private IEntityId _id;

        public IEntityId Id { get { return _id; } set { throw new InvalidArgumentException("Cannot Set Id through Setter"); } }

        public bool IsValid() => Manager.IsValid(this);
        public void Destroy() => Manager.Desotry(this);


        public void AddActivationListener(EventHandler<EntityEventArgs> onActivated) => Manager.OnActivated += onActivated;
        public void AddDeactivationListener(EventHandler<EntityEventArgs> onDeactivated) => Manager.OnDeactivated += onDeactivated;
        public void AddDestroyListener(EventHandler<EntityEventArgs> onDestroy) => Manager.OnDestroy += onDestroy;
        public void RemoveActivationListener(EventHandler<EntityEventArgs> onActivated) => Manager.OnActivated -= onActivated;
        public void RemoveDeactivationListener(EventHandler<EntityEventArgs> onDeactivated) => Manager.OnDeactivated -= onDeactivated;
        public void RemoveDestroyListener(EventHandler<EntityEventArgs> onDestroy) => Manager.OnDestroy -= onDestroy;

        public T GetComponent<T>()
        {
            return GetComponent<T>(typeof(T));
        }

        public T GetComponent<T>(Type componentType)
        {
            return Manager.GetComponent<T>(this, componentType);
        }

        public List<IComponent> GetComponents()
        {
            return Manager.GetComponents(this);
        }

        public void AddComponent<T>(params dynamic[] args)
        {
            AddComponent(typeof(T), args);
        }

        public void AddComponent(Type componentType, params dynamic[] args)
        {
            IComponent componenet = constructComponent(componentType, args, null);

            Manager.AddComponent(this, componenet, componentType);
        }

        public void AddComponent<T>(ComponentProperties properties, params dynamic[] args)
        {
            AddComponent(typeof(T), properties, args);
        }

        public void AddComponent(Type componentType, ComponentProperties properties, params dynamic[] args)
        {
            IComponent componenet = constructComponent(componentType, args, properties);

            Manager.AddComponent(this, componenet, componentType);
        }

        public void RemoveComponent<T>()
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type componentType)
        {
            Manager.RemoveComponent(this, componentType);
        }

        public bool HasComponent<T>()
        {
            return HasComponent(typeof(T));
        }

        public bool HasComponent(Type componentType)
        {
            return Manager.HasComponent(this, componentType);
        }

        public void ForEachComponent(Action<IComponent> action)
        {
            Manager.GetComponents(this).ForEach(action);
        }

        private IComponent constructComponent(Type componentType, dynamic[] args, ComponentProperties properties)
        {
            Type[] argTypes = new Type[args.Length];
            for(int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                argTypes[argIndex] = args[argIndex].GetType();
            }

            object componentObj = componentType.GetConstructor(argTypes).Invoke(args);

            properties?.ForEach(propPair =>
            {
                PropertyInfo propInfo = componentType.GetProperty(propPair.Key, BindingFlags.Public | BindingFlags.Instance);
                propInfo?.TakeIf(_ => _.CanWrite)?.SetValue(componentObj, propPair.Value, null);
            });

            return (IComponent)componentObj;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Entity))
            {
                return false;
            }

            return this == (Entity)obj;
        }
    }
}

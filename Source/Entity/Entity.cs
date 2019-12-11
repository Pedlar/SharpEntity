
using System;
using System.Collections.Generic;

namespace SharpEngine
{
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

        public T GetComponent<T>()
        {
            return Manager.GetComponent<T>(this, typeof(T));
        }

        public List<IComponent> GetComponents()
        {
            return Manager.GetComponents(this);
        }

        public void AddComponent<T>(params dynamic[] args)
        {
            Type componentType = typeof(T);
            IComponent componenet = constructComponent(componentType, args);

            Manager.AddComponent(this, componenet, componentType);
        }

        public void RemoveComponent<T>()
        {
            Manager.RemoveComponent(this, typeof(T));
        }

        public bool HasComponent<T>()
        {
            return Manager.HasComponent(this, typeof(T));
        }

        public void ForEachComponent(Action<IComponent> action)
        {
            Manager.GetComponents(this).ForEach(action);
        }

        private IComponent constructComponent(Type componentType, dynamic[] args)
        {
            Type[] argTypes = new Type[args.Length];
            for(int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                argTypes[argIndex] = args[argIndex].GetType();
            }

            return (IComponent)componentType.GetConstructor(argTypes).Invoke(args);
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

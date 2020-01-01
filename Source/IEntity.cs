using SharpEngine.Component;
using System;
using System.Collections.Generic;

namespace SharpEngine
{
    public interface IEntity
    {
        IEntityId Id { get; set; }
        EntityManager Manager { get; set; }

        bool IsActive { get; set; }
        bool IsValid();
        void Destroy();

        void AddActivationListener(EventHandler<EntityEventArgs> onActivated);
        void AddDeactivationListener(EventHandler<EntityEventArgs> onDeactivated);
        void AddDestroyListener(EventHandler<EntityEventArgs> onDestroy);

        void RemoveActivationListener(EventHandler<EntityEventArgs> onActivated);
        void RemoveDeactivationListener(EventHandler<EntityEventArgs> onDeactivated);
        void RemoveDestroyListener(EventHandler<EntityEventArgs> onDestroy);

        T GetComponent<T>();
        List<IComponent> GetComponents();
        void AddComponent<T>(params dynamic[] constructorArgs);
        void AddComponent<T>(ComponentProperties properties, params dynamic[] constructorArgs);
        void RemoveComponent<T>();
        bool HasComponent<T>();

        T GetComponent<T>(Type componentType);
        void AddComponent(Type componentType, params dynamic[] constructorArgs);
        void AddComponent(Type componentType, ComponentProperties properties, params dynamic[] constructorArgs);
        void RemoveComponent(Type componentType);
        bool HasComponent(Type componentType);

        void ForEachComponent(Action<IComponent> action);
    }
}

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

        T GetComponent<T>();
        List<IComponent> GetComponents();
        void AddComponent<T>(params dynamic[] args);
        void RemoveComponent<T>();
        bool HasComponent<T>();
        void ForEachComponent(Action<IComponent> action);
    }
}

namespace SharpEngine
{
    public interface IEntity
    {
        IEntityId Id { get; set; }
        EntityManager Manager { get; set; }

        bool IsActive { get; set; }
        bool IsValid();
        void Destroy();

        void AddComponent<T>(params dynamic[] args);
        void RemoveComponent<T>();
        bool HasComponent<T>();
    }
}

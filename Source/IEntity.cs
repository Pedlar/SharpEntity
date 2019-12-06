namespace SharpEngine
{
    public interface IEntity
    {
        IEntityId Id { get; set; }
        EntityManager Manager { get; set; }

        bool IsActive { get; set; }
        bool IsValid();
        void Destroy();
    }
}

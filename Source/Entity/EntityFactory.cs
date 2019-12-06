
namespace SharpEngine
{
    public class EntityFactory
    {
        private readonly EntityManager entityManager;
        private readonly IPool<IEntityId> entityIdPool;

        public EntityFactory(EntityManager entityManager, IPool<IEntityId> entityIdPool)
        {
            this.entityManager = entityManager;
            this.entityIdPool = entityIdPool;
        }

        public Entity Create(IEntityId _id = null) => new Entity(entityManager, _id == null ? entityIdPool.Create() : _id);
    }
}

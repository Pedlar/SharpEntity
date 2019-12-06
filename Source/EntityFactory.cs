
namespace SharpEngine
{
    public class EntityFactory
    {
        private readonly EntityManager entityManager;
        private readonly IPool<Entity.Id> entityIdPool;

        public EntityFactory(EntityManager entityManager, IPool<Entity.Id> entityIdPool)
        {
            this.entityManager = entityManager;
            this.entityIdPool = entityIdPool;
        }

        public Entity Create() => new Entity(entityManager, entityIdPool.Create());
    }
}

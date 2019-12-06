
namespace SharpEngine
{
    public class Entity
    {
        public class Id
        {
            private const int INDEX_BIT_COUNT = 48;
            private const int COUNTER_BIT_COUNT = 16;

            public ulong Index { get; private set; }
            public ulong Counter { get; set; }

            public Id(ulong index, ulong counter)
            {
                Index = index;
                Counter = counter;
            }

            static public implicit operator ulong(Id id) => id.Value();

            static public bool operator ==(Id left, Id right) => left.Value() == right.Value();
            static public bool operator !=(Id left, Id right) => !(left == right);
            static public bool operator >(Id left, Id right) => left.Value() > right.Value();
            static public bool operator <(Id left, Id right) => left.Value() < right.Value();
            static public bool operator >=(Id left, Id right) => left.Value() >= right.Value();
            static public bool operator <=(Id left, Id right) => left.Value() <= right.Value();

            public ulong Value() => (Counter << COUNTER_BIT_COUNT) | Index;
            public bool IsValid() => Value() > 0;

            public void Dispose()
            {
                Index = 0;
                Counter = 0;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Id))
                {
                    return false;
                }

                return this == (Id)obj;
            }

            public override int GetHashCode()
            {
                var hashCode = 2094007494;
                hashCode = hashCode * -1521134295 + Index.GetHashCode();
                hashCode = hashCode * -1521134295 + Counter.GetHashCode();
                return hashCode;
            }
        }

        public class Attributes
        {
            public bool Activated { get; set; }
        }

        public Entity(EntityManager manager, Id id)
        {
            entityManager = manager;
            entityId = id;
        }

        public static bool operator ==(Entity left, Entity right) {
            return right.entityManager == left.entityManager && right.entityId == left.entityId;
        }

        public static bool operator !=(Entity left, Entity right) => !(left == right);
        public static bool operator <(Entity left, Entity right) => left.entityId < right.entityId;
        public static bool operator >(Entity left, Entity right) => left.entityId > right.entityId;
        public static bool operator <=(Entity left, Entity right) => left.entityId <= right.entityId;
        public static bool operator >=(Entity left, Entity right) => left.entityId >= right.entityId;

        public bool IsActive
        {
            get
            {
                return entityManager.IsActivated(this);
            }
            set
            {
                if(value)
                {
                    entityManager.Activate(this);
                }
                else
                {
                    entityManager.Deactivate(this);
                }
            }
        }

        public EntityManager entityManager { get; private set; }
        public Id entityId { get; private set; }

        public bool IsValid() => entityManager.IsValid(this);
        public void Destroy() => entityManager.Desotry(this);

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

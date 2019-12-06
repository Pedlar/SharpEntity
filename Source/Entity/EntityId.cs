using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine
{
    class EntityId : IEntityId
    {
        private const int INDEX_BIT_COUNT = 48;
        private const int COUNTER_BIT_COUNT = 16;

        public ulong Index { get; set; }
        public ulong Counter { get; set; }

        public EntityId(ulong index, ulong counter)
        {
            Index = index;
            Counter = counter;
        }

        static public implicit operator ulong(EntityId id) => id.Value();

        static public bool operator ==(EntityId left, EntityId right) => left.Value() == right.Value();
        static public bool operator !=(EntityId left, EntityId right) => !(left == right);
        static public bool operator >(EntityId left, EntityId right) => left.Value() > right.Value();
        static public bool operator <(EntityId left, EntityId right) => left.Value() < right.Value();
        static public bool operator >=(EntityId left, EntityId right) => left.Value() >= right.Value();
        static public bool operator <=(EntityId left, EntityId right) => left.Value() <= right.Value();

        public ulong Value() => (Counter << COUNTER_BIT_COUNT) | Index;
        public bool IsValid() => Value() > 0;

        public void Dispose()
        {
            Index = 0;
            Counter = 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EntityId))
            {
                return false;
            }

            return this == (EntityId)obj;
        }

        public override int GetHashCode()
        {
            var hashCode = 2094007494;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Counter.GetHashCode();
            return hashCode;
        }
    }
}

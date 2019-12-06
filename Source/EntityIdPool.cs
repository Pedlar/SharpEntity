using System.Collections.Generic;
using System.Linq;

namespace SharpEngine
{
    public class EntityIdPool : IPool<Entity.Id>
    {

        private ulong nextAvailableId;
        private List<Entity.Id> recyclableIds;
        private IDictionary<ulong, ulong> idCounter;

        public EntityIdPool(ulong startId = 0, List<Entity.Id> _recyclableIds = null, IDictionary<ulong, ulong> _idCounter = null)
        {
            recyclableIds = _recyclableIds == null ? new List<Entity.Id>() : _recyclableIds;
            idCounter = _idCounter == null ? new Dictionary<ulong, ulong>() : _idCounter;
            nextAvailableId = startId;
        }

        public void Clear()
        {
            recyclableIds.Clear();
            idCounter.Clear();
            nextAvailableId = 0;
        }

        public bool Contains(Entity.Id item)
        {
            return idCounter.ContainsKey(item.Index);
        }

        public Entity.Id Create()
        {
            Entity.Id id;
            if(recyclableIds.Count > 0)
            {
                id = recyclableIds.Last();
                recyclableIds.Remove(id);
            }
            else
            {
                nextAvailableId++;
                id = new Entity.Id(nextAvailableId, 1);
                idCounter[id.Index] = 1;
            }

            return id;
        }

        public Entity.Id Get(ulong index)
        {
            if(index > (ulong)idCounter.Count)
            {
                throw new InvalidIndexIdException("Entity Id Not In Pool, Should Use Create First.");
            }

            return new Entity.Id(index, idCounter[index]);
        }

        public int GetSize()
        {
            return idCounter.Count;
        }

        public bool IsValid(Entity.Id item)
        {
            if(item.Index > (ulong)idCounter.Count) {
                return false;
            }

            return item.Counter == idCounter[item.Index] && item.Counter > 0;
        }

        public void Remove(Entity.Id item)
        {
            idCounter[item.Index]++;
            item.Counter = idCounter[item.Index];
            recyclableIds.Add(item);
        }
    }
}

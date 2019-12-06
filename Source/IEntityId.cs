using System;

namespace SharpEngine
{
    public interface IEntityId : IDisposable
    {
        ulong Index { get; set; }
        ulong Counter { get; set; }
        ulong Value();
        bool IsValid();
    }
}

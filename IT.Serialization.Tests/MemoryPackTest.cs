#if NETCOREAPP3_1_OR_GREATER

namespace IT.Serialization.Tests;

public class MemoryPackTest : SerializerTest
{
    private static readonly MemoryPack.Serialization _serializer = new();

    public MemoryPackTest() : base(_serializer) { }
}

#endif
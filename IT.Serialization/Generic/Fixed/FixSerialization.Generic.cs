using System;

namespace IT.Serialization.Generic.Fixed;

public abstract class FixSerialization<T> : Serialization<T>, IFixSerializer<T>
{
    public abstract int GetSerializedLength(in T? value);

    public abstract int Serialize(in T? value, Span<byte> span);

    public override byte[] Serialize(in T? value)
    {
        var bytes = new byte[GetSerializedLength(in value)];

        Serialize(in value, bytes);

        return bytes;
    }

    public override void Serialize<TBufferWriter>(in T? value, in TBufferWriter writer)
    {
        writer.Advance(Serialize(in value, writer.GetSpan(GetSerializedLength(in value))));
    }
}
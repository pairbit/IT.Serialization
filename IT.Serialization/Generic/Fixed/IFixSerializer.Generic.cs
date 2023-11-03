using System;

namespace IT.Serialization.Generic.Fixed;

public interface IFixSerializer<T> : ISerializer<T>
{
    int GetSerializedLength(in T? value);

    int Serialize(in T? value, Span<byte> span);
}
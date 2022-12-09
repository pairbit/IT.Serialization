#if NETSTANDARD2_0

namespace System.Text;

internal static class _Encoding
{
    public static unsafe Int32 GetByteCount(this Encoding encoding, ReadOnlySpan<Char> chars)
    {
        fixed (Char* charsPtr = chars)
            return encoding.GetByteCount(charsPtr, chars.Length);
    }

    public static unsafe Int32 GetBytes(this Encoding encoding, ReadOnlySpan<Char> chars, Span<Byte> bytes)
    {
        fixed (Char* charsPtr = chars)
        fixed (Byte* bytesPtr = bytes)
            return encoding.GetBytes(charsPtr, chars.Length, bytesPtr, bytes.Length);
    }

    public static unsafe Int32 GetCharCount(this Encoding encoding, ReadOnlySpan<Byte> bytes)
    {
        fixed (Byte* bytesPtr = bytes)
            return encoding.GetCharCount(bytesPtr, bytes.Length);
    }

    public static unsafe Int32 GetChars(this Encoding encoding, ReadOnlySpan<Byte> bytes, Span<Char> chars)
    {
        fixed (Byte* bytesPtr = bytes)
        fixed (Char* charsPtr = chars)
            return encoding.GetChars(bytesPtr, bytes.Length, charsPtr, chars.Length);
    }

    public static unsafe String GetString(this Encoding encoding, ReadOnlySpan<Byte> bytes)
    {
        fixed (Byte* bytesPtr = bytes)
            return encoding.GetString(bytesPtr, bytes.Length);
    }
}

#endif
#if NETSTANDARD2_0

namespace System.Text;

internal static class _Encoding
{
    public static unsafe int GetByteCount(this Encoding encoding, ReadOnlySpan<char> chars)
    {
        fixed (char* charsPtr = chars)
            return encoding.GetByteCount(charsPtr, chars.Length);
    }

    public static unsafe int GetBytes(this Encoding encoding, ReadOnlySpan<char> chars, Span<byte> bytes)
    {
        fixed (char* charsPtr = chars)
        fixed (byte* bytesPtr = bytes)
            return encoding.GetBytes(charsPtr, chars.Length, bytesPtr, bytes.Length);
    }

    public static unsafe int GetCharCount(this Encoding encoding, ReadOnlySpan<byte> bytes)
    {
        fixed (byte* bytesPtr = bytes)
            return encoding.GetCharCount(bytesPtr, bytes.Length);
    }

    public static unsafe int GetChars(this Encoding encoding, ReadOnlySpan<byte> bytes, Span<char> chars)
    {
        fixed (byte* bytesPtr = bytes)
        fixed (char* charsPtr = chars)
            return encoding.GetChars(bytesPtr, bytes.Length, charsPtr, chars.Length);
    }

    public static unsafe string GetString(this Encoding encoding, ReadOnlySpan<byte> bytes)
    {
        fixed (byte* bytesPtr = bytes)
            return encoding.GetString(bytesPtr, bytes.Length);
    }
}

#endif
//using System;

//namespace IT.Serialization;

//public class NumberSerializer : 
//    ITextSerializer<UInt64>, ITextSerializer<Int64>,
//    ITextSerializer<UInt32>, ITextSerializer<Int32>
//{
//    internal static readonly Char[] tableNum = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e',
//                                                 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
//                                                 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
//                                                 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
//                                                 'Y', 'Z', '-', '_' };
//    #region const

//    /// <summary>
//    /// 2^6
//    /// </summary>
//    public const UInt32 NumCount1 = 64;

//    /// <summary>
//    /// 2^12 = 4 096
//    /// </summary>
//    public const UInt32 NumCount2 = NumCount1 * NumCount1;

//    /// <summary>
//    /// 4 160
//    /// </summary>
//    public const UInt32 NumSum2 = NumCount2 + NumCount1;

//    /// <summary>
//    /// 2^18 = 262 144
//    /// </summary>
//    public const UInt32 NumCount3 = NumCount2 * NumCount1;

//    /// <summary>
//    /// 266 304
//    /// </summary>
//    public const UInt32 NumSum3 = NumCount3 + NumSum2;

//    /// <summary>
//    /// 2^24 = 16 777 216
//    /// </summary>
//    public const UInt32 NumCount4 = NumCount3 * NumCount1;

//    /// <summary>
//    /// 17 043 520
//    /// </summary>
//    public const UInt32 NumSum4 = NumCount4 + NumSum3;

//    /// <summary>
//    /// 2^30 = 1 073 741 824
//    /// </summary>
//    public const UInt32 NumCount5 = NumCount4 * NumCount1;

//    /// <summary>
//    /// 1 090 785 344
//    /// </summary>
//    public const UInt32 NumSum5 = NumCount5 + NumSum4;

//    /// <summary>
//    /// 2^36 = 68 719 476 736
//    /// </summary>
//    public const UInt64 NumCount6 = (UInt64)NumCount5 * NumCount1;

//    /// <summary>
//    /// 69 810 262 080
//    /// </summary>
//    public const UInt64 NumSum6 = NumCount6 + NumSum5;

//    /// <summary>
//    /// 2^42 = 4 398 046 511 104
//    /// </summary>
//    public const UInt64 NumCount7 = NumCount6 * NumCount1;

//    /// <summary>
//    /// 4 467 856 773 184
//    /// </summary>
//    public const UInt64 NumSum7 = NumCount7 + NumSum6;

//    /// <summary>
//    /// 2^48 = 281 474 976 710 656
//    /// </summary>
//    public const UInt64 NumCount8 = NumCount7 * NumCount1;

//    /// <summary>
//    /// 285 942 833 483 840
//    /// </summary>
//    public const UInt64 NumSum8 = NumCount8 + NumSum7;

//    /// <summary>
//    /// 2^54 = 18 014 398 509 481 984
//    /// </summary>
//    public const UInt64 NumCount9 = NumCount8 * NumCount1;

//    /// <summary>
//    /// 18 300 341 342 965 824
//    /// </summary>
//    public const UInt64 NumSum9 = NumCount9 + NumSum8;

//    /// <summary>
//    /// 2^60 = 1 152 921 504 606 846 976
//    /// </summary>
//    public const UInt64 NumCount10 = NumCount9 * NumCount1;

//    /// <summary>
//    /// 1 171 221 845 949 812 800
//    /// </summary>
//    public const UInt64 NumSum10 = NumCount10 + NumSum9;

//    /// <summary>
//    /// 2^66 = 73 786 976 294 838 206 464
//    /// </summary>
//    public const Decimal NumCount11 = (Decimal)NumCount10 * NumCount1;

//    /// <summary>
//    /// 74 958 198 140 788 019 264
//    /// </summary>
//    public const Decimal NumSum11 = NumCount11 + NumSum10;

//    #endregion const

//    #region ITextSerializer

//    UInt64 ITextSerializer<UInt64>.Deserialize(ReadOnlySpan<Char> value) => ToUInt64(value);

//    String ITextSerializer<UInt64>.Serialize(UInt64 value) => ToString(value);

//    Int64 ITextSerializer<Int64>.Deserialize(ReadOnlySpan<Char> value) => (Int64)ToUInt64(value);

//    String ITextSerializer<Int64>.Serialize(Int64 value) => ToString(value);

//    UInt32 ITextSerializer<UInt32>.Deserialize(ReadOnlySpan<Char> value) => ToUInt32(value);

//    String ITextSerializer<UInt32>.Serialize(UInt32 value) => ToString(value);

//    Int32 ITextSerializer<Int32>.Deserialize(ReadOnlySpan<Char> value) => (Int32)ToUInt32(value);

//    String ITextSerializer<Int32>.Serialize(Int32 value) => ToString(value);

//    #endregion ITextSerializer

//    public static UInt32 ToUInt32(ReadOnlySpan<Char> span)
//    {
//        var len = span.Length;

//        if (len == 0) throw new ArgumentException(nameof(span));

//        if (len > 6) throw new ArgumentOutOfRangeException(nameof(span), "Length 1-6");

//        if (len == 1) return GetIndexNum(span[0]);

//        if (len == 2) return (GetIndexNum(span[0]) << 6) + GetIndexNum(span[1]) + NumCount1;

//        if (len == 3) return (GetIndexNum(span[0]) << 12) + (GetIndexNum(span[1]) << 6) + GetIndexNum(span[2]) + NumSum2;

//        if (len == 4) return (GetIndexNum(span[0]) << 18) + (GetIndexNum(span[1]) << 12) + (GetIndexNum(span[2]) << 6) + GetIndexNum(span[3]) + NumSum3;

//        if (len == 5) return (GetIndexNum(span[0]) << 24) + (GetIndexNum(span[1]) << 18) + (GetIndexNum(span[2]) << 12) + (GetIndexNum(span[3]) << 6) + GetIndexNum(span[4]) + NumSum4;

//        return checked((GetIndexNum(span[0]) << 30) + (GetIndexNum(span[1]) << 24) + (GetIndexNum(span[2]) << 18) + (GetIndexNum(span[3]) << 12) + (GetIndexNum(span[4]) << 6) + GetIndexNum(span[5]) + NumSum5);
//    }

//    public static UInt64 ToUInt64(ReadOnlySpan<Char> span)
//    {
//        var len = span.Length;

//        if (len == 0) throw new ArgumentException(nameof(span));

//        if (len > 11) throw new ArgumentOutOfRangeException(nameof(span), "Length 1-11");

//        if (len < 6) return ToUInt32(span);

//        if (len == 6) return ((UInt64)GetIndexNum(span[0]) << 30) + (GetIndexNum(span[1]) << 24) + (GetIndexNum(span[2]) << 18) +
//                             (GetIndexNum(span[3]) << 12) + (GetIndexNum(span[4]) << 6) + GetIndexNum(span[5]) + NumSum5;

//        if (len == 7) return ((UInt64)GetIndexNum(span[0]) << 36) + ((UInt64)GetIndexNum(span[1]) << 30) + (GetIndexNum(span[2]) << 24) +
//                             (GetIndexNum(span[3]) << 18) + (GetIndexNum(span[4]) << 12) + (GetIndexNum(span[5]) << 6) + GetIndexNum(span[6]) + NumSum6;

//        if (len == 8) return ((UInt64)GetIndexNum(span[0]) << 42) + ((UInt64)GetIndexNum(span[1]) << 36) + ((UInt64)GetIndexNum(span[2]) << 30) + (GetIndexNum(span[3]) << 24) +
//                             (GetIndexNum(span[4]) << 18) + (GetIndexNum(span[5]) << 12) + (GetIndexNum(span[6]) << 6) + GetIndexNum(span[7]) + NumSum7;

//        if (len == 9) return ((UInt64)GetIndexNum(span[0]) << 48) + ((UInt64)GetIndexNum(span[1]) << 42) + ((UInt64)GetIndexNum(span[2]) << 36) + ((UInt64)GetIndexNum(span[3]) << 30) +
//                             (GetIndexNum(span[4]) << 24) + (GetIndexNum(span[5]) << 18) + (GetIndexNum(span[6]) << 12) + (GetIndexNum(span[7]) << 6) +
//                             GetIndexNum(span[8]) + NumSum8;

//        if (len == 10) return ((UInt64)GetIndexNum(span[0]) << 54) + ((UInt64)GetIndexNum(span[1]) << 48) + ((UInt64)GetIndexNum(span[2]) << 42) + ((UInt64)GetIndexNum(span[3]) << 36) +
//                              ((UInt64)GetIndexNum(span[4]) << 30) + (GetIndexNum(span[5]) << 24) + (GetIndexNum(span[6]) << 18) + (GetIndexNum(span[7]) << 12) +
//                              (GetIndexNum(span[8]) << 6) + GetIndexNum(span[9]) + NumSum9;

//        return checked(((UInt64)GetIndexNum(span[0]) << 60) + ((UInt64)GetIndexNum(span[1]) << 54) + ((UInt64)GetIndexNum(span[2]) << 48) + ((UInt64)GetIndexNum(span[3]) << 42) +
//               ((UInt64)GetIndexNum(span[4]) << 36) + ((UInt64)GetIndexNum(span[5]) << 30) + (GetIndexNum(span[6]) << 24) + (GetIndexNum(span[7]) << 18) +
//               (GetIndexNum(span[8]) << 12) + (GetIndexNum(span[9]) << 6) + GetIndexNum(span[10]) + NumSum10);
//    }

//    /*

//1 char |             64 :             0 -             63
//2 char |          4 096 :            64 -          4 159
//3 char |        262 144 :         4 160 -        266 303
//4 char |     16 777 216 :       266 304 -     17 043 519
//5 char |  1 073 741 824 :    17 043 520 -  1 090 785 343
//6 char | 68 719 476 736 : 1 090 785 344 - 69 810 262 079

// Int16 |         32 767
//UInt16 |         65 535
// Int32 |  2 147 483 647
//UInt32 |  4 294 967 295

//=======================================================

// 7 char |          4 398 046 511 104
// 8 char |        281 474 976 710 656
// 9 char |     18 014 398 509 481 984
//10 char |  1 152 921 504 606 846 976
//11 char | 73 786 976 294 838 206 464

//  Int64 |  9 223 372 036 854 775 807
// UInt64 | 18 446 744 073 709 551 615

//=======================================================
//15 char |  1 237 940 039 285 380 274 899 124 224
//16 char | 79 228 162 514 264 337 593 543 950 336
//Decimal | 79 228 162 514 264 337 593 543 950 335
//         */

//    public static String ToString(SByte value) => ToString((UInt32)(Byte)value);

//    public static String ToString(Byte value) => ToString((UInt32)value);

//    public static String ToString(Int16 value) => ToString((UInt32)(UInt16)value);

//    public static String ToString(UInt16 value) => ToString((UInt32)value);

//    public static String ToString(Int32 value) => ToString((UInt32)value);

//    public static String ToString(UInt32 value)
//    {
//        if (value < NumCount1) return tableNum[value].ToString();

//        if (value < NumSum2) return new String(new Char[] {
//            tableNum[(value -= NumCount1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        if (value < NumSum3) return new String(new Char[] {
//            tableNum[(value -= NumSum2) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        if (value < NumSum4) return new String(new Char[] {
//            tableNum[(value -= NumSum3) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        if (value < NumSum5) return new String(new Char[] {
//            tableNum[(value -= NumSum4) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        return new String(new Char[] {
//            tableNum[(value -= NumSum5) >> 30],
//            tableNum[(value &= NumCount5 - 1) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });
//    }

//    public static String ToString(Int64 value) => ToString((UInt64)value);

//    public static String ToString(UInt64 value)
//    {
//        if (value <= UInt32.MaxValue) return ToString((UInt32)value);

//        if (value < NumSum6) return new String(new Char[] {
//            tableNum[(value -= NumSum5) >> 30],
//            tableNum[(value &= NumCount5 - 1) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        if (value < NumSum7) return new String(new Char[] {
//            tableNum[(value -= NumSum6) >> 36],
//            tableNum[(value &= NumCount6 - 1) >> 30],
//            tableNum[(value &= NumCount5 - 1) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        if (value < NumSum8) return new String(new Char[] {
//            tableNum[(value -= NumSum7) >> 42],
//            tableNum[(value &= NumCount7 - 1) >> 36],
//            tableNum[(value &= NumCount6 - 1) >> 30],
//            tableNum[(value &= NumCount5 - 1) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        if (value < NumSum9) return new String(new Char[] {
//            tableNum[(value -= NumSum8) >> 48],
//            tableNum[(value &= NumCount8 - 1) >> 42],
//            tableNum[(value &= NumCount7 - 1) >> 36],
//            tableNum[(value &= NumCount6 - 1) >> 30],
//            tableNum[(value &= NumCount5 - 1) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        if (value < NumSum10) return new String(new Char[] {
//            tableNum[(value -= NumSum9) >> 54],
//            tableNum[(value &= NumCount9 - 1) >> 48],
//            tableNum[(value &= NumCount8 - 1) >> 42],
//            tableNum[(value &= NumCount7 - 1) >> 36],
//            tableNum[(value &= NumCount6 - 1) >> 30],
//            tableNum[(value &= NumCount5 - 1) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });

//        return new String(new Char[] {
//            tableNum[(value -= NumSum10) >> 60],
//            tableNum[(value &= NumCount10 - 1) >> 54],
//            tableNum[(value &= NumCount9 - 1) >> 48],
//            tableNum[(value &= NumCount8 - 1) >> 42],
//            tableNum[(value &= NumCount7 - 1) >> 36],
//            tableNum[(value &= NumCount6 - 1) >> 30],
//            tableNum[(value &= NumCount5 - 1) >> 24],
//            tableNum[(value &= NumCount4 - 1) >> 18],
//            tableNum[(value &= NumCount3 - 1) >> 12],
//            tableNum[(value &= NumCount2 - 1) >> 6],
//            tableNum[value & NumCount1 - 1]
//        });
//    }

//    //public static String ToString(Decimal value)
//    //{
//    //    if (value >= Int32.MinValue && value <= UInt32.MaxValue) return ToString((UInt32)value);
//    //    if (value >= Int64.MinValue && value <= UInt64.MaxValue) return ToString((UInt64)value);

//    //    return new string(new Char[] {
//    //        tableNum[(value -= NumSum10) >> 60],
//    //        tableNum[(value &= NumCount10 - 1) >> 54],
//    //        tableNum[(value &= NumCount9 - 1) >> 48],
//    //        tableNum[(value &= NumCount8 - 1) >> 42],
//    //        tableNum[(value &= NumCount7 - 1) >> 36],
//    //        tableNum[(value &= NumCount6 - 1) >> 30],
//    //        tableNum[(value &= NumCount5 - 1) >> 24],
//    //        tableNum[(value &= NumCount4 - 1) >> 18],
//    //        tableNum[(value &= NumCount3 - 1) >> 12],
//    //        tableNum[(value &= NumCount2 - 1) >> 6],
//    //        tableNum[value & (NumCount1 - 1)]
//    //    });
//    //}

//    public static UInt32 GetIndexNum(Char ch) => ch switch
//    {
//        '0' => 0,
//        '1' => 1,
//        '2' => 2,
//        '3' => 3,
//        '4' => 4,
//        '5' => 5,
//        '6' => 6,
//        '7' => 7,
//        '8' => 8,
//        '9' => 9,
//        'a' => 10,
//        'b' => 11,
//        'c' => 12,
//        'd' => 13,
//        'e' => 14,
//        'f' => 15,
//        'g' => 16,
//        'h' => 17,
//        'i' => 18,
//        'j' => 19,
//        'k' => 20,
//        'l' => 21,
//        'm' => 22,
//        'n' => 23,
//        'o' => 24,
//        'p' => 25,
//        'q' => 26,
//        'r' => 27,
//        's' => 28,
//        't' => 29,
//        'u' => 30,
//        'v' => 31,
//        'w' => 32,
//        'x' => 33,
//        'y' => 34,
//        'z' => 35,
//        'A' => 36,
//        'B' => 37,
//        'C' => 38,
//        'D' => 39,
//        'E' => 40,
//        'F' => 41,
//        'G' => 42,
//        'H' => 43,
//        'I' => 44,
//        'J' => 45,
//        'K' => 46,
//        'L' => 47,
//        'M' => 48,
//        'N' => 49,
//        'O' => 50,
//        'P' => 51,
//        'Q' => 52,
//        'R' => 53,
//        'S' => 54,
//        'T' => 55,
//        'U' => 56,
//        'V' => 57,
//        'W' => 58,
//        'X' => 59,
//        'Y' => 60,
//        'Z' => 61,
//        '-' => 62,
//        '_' => 63,
//        _ => throw new ArgumentOutOfRangeException(nameof(ch), $"Char '{ch}' is not Base64Num")
//    };
//}
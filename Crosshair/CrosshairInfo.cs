using System;
using System.Linq;
using System.Numerics;

using System.Text.RegularExpressions;

namespace ShareCode
{
    enum CrosshairStyle
    {
        Default,
        DefaultStatic,
        Classic,
        ClassicDynamic,
        ClassicStatic,
    }

    struct CrosshairInfo
    {
        public CrosshairStyle Style; // LODWORD [14] >> 1
        public bool HasCenterDot; // HIDWORD [14] & 1

        public float Length; // [15] / 10
        public float Thickness; // [13] / 10
        public float Gap; // unchecked((sbyte) [3]) / 10

        public bool HasOutline; // [11] & 8
        public float Outline; // [4] / 2

        public int Red; // [5]
        public int Green; // [6]
        public int Blue; // [7]

        public bool HasAlpha; // HIDWORD [14] & 4
        public int Alpha; // [8]

        public int SplitDistance; // [9]
        public float InnerSplitAlpha; // HIDWORD [11] / 10
        public float OuterSplitAlpha; // LODWORD [12] / 10
        public float SplitSizeRatio; // HIDWORD [12] / 10

        public bool IsTStyle; // HIDWORD [14] & 8

        public static CrosshairInfo Decode(byte[] bytes)
        {
            return new CrosshairInfo
            {
                Outline = (float)(bytes[4] / 2.0),
                Red = bytes[5],
                Green = bytes[6],
                Blue = bytes[7],
                Alpha = bytes[8],
                SplitDistance = bytes[9],

                InnerSplitAlpha = (float)((bytes[11] >> 4) / 10.0),
                HasOutline = (bytes[11] & 8) != 0,
                OuterSplitAlpha = (float)((bytes[12] & 0xF) / 10.0),
                SplitSizeRatio = (float)((bytes[12] >> 4) / 10.0),

                Thickness = (float)(bytes[13] / 10.0),
                Length = (float)(bytes[15] / 10.0),
                Gap = (float)(unchecked((sbyte)bytes[3]) / 10.0),

                HasCenterDot = ((bytes[14] >> 4) & 1) != 0,
                HasAlpha = ((bytes[14] >> 4) & 4) != 0,
                IsTStyle = ((bytes[14] >> 4) & 8) != 0,

                Style = (CrosshairStyle)((bytes[14] & 0xF) >> 1),
            };
        }

        public override string ToString()
        {
            return $"{nameof(Style)}: {Style}, {nameof(HasCenterDot)}: {HasCenterDot}, {nameof(Length)}: {Length}, {nameof(Thickness)}: {Thickness}, {nameof(Gap)}: {Gap}, {nameof(HasOutline)}: {HasOutline}, {nameof(Outline)}: {Outline}, {nameof(Red)}: {Red}, {nameof(Green)}: {Green}, {nameof(Blue)}: {Blue}, {nameof(HasAlpha)}: {HasAlpha}, {nameof(Alpha)}: {Alpha}, {nameof(SplitDistance)}: {SplitDistance}, {nameof(InnerSplitAlpha)}: {InnerSplitAlpha}, {nameof(OuterSplitAlpha)}: {OuterSplitAlpha}, {nameof(SplitSizeRatio)}: {SplitSizeRatio}, {nameof(IsTStyle)}: {IsTStyle}";
        }
    }

    public class ShareCode
    {
        const string DICTIONARY = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefhijkmnopqrstuvwxyz23456789";
        const string SHARECODE_PATTERN = "^CSGO(-?[\\w]{5}){5}$";

        /// <summary>
        /// Decode a share code from the string.
        /// </summary>
        /// <param name="shareCode"></param>
        /// <returns></returns>
        public static byte[] Decode(string shareCode)
        {
            var r = new Regex(SHARECODE_PATTERN);
            if (!r.IsMatch(shareCode))
                throw new Exception();

            var code = shareCode.Remove(0, 4).Replace("-", "");

            var big = BigInteger.Zero;
            foreach (var c in code.ToCharArray().Reverse())
            {
                big = BigInteger.Multiply(big, DICTIONARY.Length) + DICTIONARY.IndexOf(c);
            }

            var all = big.ToByteArray().ToArray();
            // sometimes the number isn't unsigned, add a 00 byte at the end of the array to make sure it is
            if (all.Length == 18)
                all = all.Concat(new byte[] { 0 }).ToArray();
            return all.Reverse().ToArray();
        }
    }

    //static class Program
    //{
       // static void DumpCode(string code)
        //{
        //    var info = CrosshairInfo.Decode(ShareCode.Decode(code));
        //    Console.WriteLine($"{code}: {info}");
       // }

       // static void Main(string[] args)
       // {
           // DumpCode("CSGO-zpstH-jozpK-AxiWq-GNC2a-pVnMC");
       // }
 //   }
}
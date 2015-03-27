using System;

namespace bgle.Contracts.Encoder
{
    public class ZBase32Encoder : Base32Encoder
    {
        //zBase32 encoding table: See http://zooko.com/repos/z-base-32/base32/DESIGN
        private const string DefEncodingTable = "ybndrfg8ejkmcpqxot1uwisza345h769";

        private const char DefPadding = '=';

        public ZBase32Encoder()
            : base(DefEncodingTable, DefPadding)
        {
        }

        public override string Encode(byte[] input)
        {
            var encoded = base.Encode(input);
            return encoded.TrimEnd(DefPadding);
        }

        public override byte[] Decode(string data)
        {
            //Guess the original data size
            int expectedOrigSize = Convert.ToInt32(Math.Floor(data.Length / 1.6));
            int expectedPaddedLength = 8 * Convert.ToInt32(Math.Ceiling(expectedOrigSize / 5.0));
            string base32Data = data.PadRight(expectedPaddedLength, DefPadding).ToLower();

            return base.Decode(base32Data);
        }
    }
}
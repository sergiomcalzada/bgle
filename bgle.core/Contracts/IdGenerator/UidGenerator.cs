using System;

using bgle.Contracts.Encoder;

namespace bgle.Contracts.IdGenerator
{
    public class UidGenerator
    {
        public const int NewGuidLength = 13;

        public const int NewIntegerGuidLength = 19;

        /// <summary>
        ///     http://www.csharphelp.com/2007/09/generate-unique-strings-and-numbers-in-c/
        /// </summary>
        /// <returns></returns>
        public static string NewStringUid()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= (b + 1);
            }

            // Pasa a ZBase32
            var bytes = BitConverter.GetBytes(i - DateTime.Now.Ticks);

            var encoder = new ZBase32Encoder();
            return encoder.Encode(bytes);
        }

        public static long NewInt64Uid()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
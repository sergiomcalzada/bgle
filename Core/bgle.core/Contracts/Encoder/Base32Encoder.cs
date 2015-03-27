﻿using System;
using System.Collections.Generic;
using System.Text;

namespace bgle.Contracts.Encoder
{
    public class Base32Encoder : IEncoder
    {
        private const string DefEncodingTable = "abcdefghijklmnopqrstuvwxyz234567";

        private const char DefPadding = '=';

        private readonly byte[] dTable; //Decoding table

        private readonly string eTable; //Encoding table

        private readonly char padding;

        public Base32Encoder()
            : this(DefEncodingTable, DefPadding)
        {
        }

        public Base32Encoder(char padding)
            : this(DefEncodingTable, padding)
        {
        }

        public Base32Encoder(string encodingTable)
            : this(encodingTable, DefPadding)
        {
        }

        public Base32Encoder(string encodingTable, char padding)
        {
            this.eTable = encodingTable;
            this.padding = padding;
            this.dTable = new byte[0x80];
            this.InitialiseDecodingTable();
        }

        public virtual string Encode(byte[] input)
        {
            var output = new StringBuilder();
            int specialLength = input.Length % 5;
            int normalLength = input.Length - specialLength;
            for (int i = 0; i < normalLength; i += 5)
            {
                int b1 = input[i] & 0xff;
                int b2 = input[i + 1] & 0xff;
                int b3 = input[i + 2] & 0xff;
                int b4 = input[i + 3] & 0xff;
                int b5 = input[i + 4] & 0xff;

                output.Append(this.eTable[(b1 >> 3) & 0x1f]);
                output.Append(this.eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                output.Append(this.eTable[(b2 >> 1) & 0x1f]);
                output.Append(this.eTable[((b2 << 4) | (b3 >> 4)) & 0x1f]);
                output.Append(this.eTable[((b3 << 1) | (b4 >> 7)) & 0x1f]);
                output.Append(this.eTable[(b4 >> 2) & 0x1f]);
                output.Append(this.eTable[((b4 << 3) | (b5 >> 5)) & 0x1f]);
                output.Append(this.eTable[b5 & 0x1f]);
            }

            switch (specialLength)
            {
                case 1:
                    {
                        int b1 = input[normalLength] & 0xff;
                        output.Append(this.eTable[(b1 >> 3) & 0x1f]);
                        output.Append(this.eTable[(b1 << 2) & 0x1f]);
                        output.Append(this.padding).Append(this.padding).Append(this.padding).Append(this.padding).Append(this.padding).Append(this.padding);
                        break;
                    }

                case 2:
                    {
                        int b1 = input[normalLength] & 0xff;
                        int b2 = input[normalLength + 1] & 0xff;
                        output.Append(this.eTable[(b1 >> 3) & 0x1f]);
                        output.Append(this.eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                        output.Append(this.eTable[(b2 >> 1) & 0x1f]);
                        output.Append(this.eTable[(b2 << 4) & 0x1f]);
                        output.Append(this.padding).Append(this.padding).Append(this.padding).Append(this.padding);
                        break;
                    }
                case 3:
                    {
                        int b1 = input[normalLength] & 0xff;
                        int b2 = input[normalLength + 1] & 0xff;
                        int b3 = input[normalLength + 2] & 0xff;
                        output.Append(this.eTable[(b1 >> 3) & 0x1f]);
                        output.Append(this.eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                        output.Append(this.eTable[(b2 >> 1) & 0x1f]);
                        output.Append(this.eTable[((b2 << 4) | (b3 >> 4)) & 0x1f]);
                        output.Append(this.eTable[(b3 << 1) & 0x1f]);
                        output.Append(this.padding).Append(this.padding).Append(this.padding);
                        break;
                    }
                case 4:
                    {
                        int b1 = input[normalLength] & 0xff;
                        int b2 = input[normalLength + 1] & 0xff;
                        int b3 = input[normalLength + 2] & 0xff;
                        int b4 = input[normalLength + 3] & 0xff;
                        output.Append(this.eTable[(b1 >> 3) & 0x1f]);
                        output.Append(this.eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                        output.Append(this.eTable[(b2 >> 1) & 0x1f]);
                        output.Append(this.eTable[((b2 << 4) | (b3 >> 4)) & 0x1f]);
                        output.Append(this.eTable[((b3 << 1) | (b4 >> 7)) & 0x1f]);
                        output.Append(this.eTable[(b4 >> 2) & 0x1f]);
                        output.Append(this.eTable[(b4 << 3) & 0x1f]);
                        output.Append(this.padding);
                        break;
                    }
            }

            return output.ToString();
        }

        public virtual byte[] Decode(string data)
        {
            var outStream = new List<Byte>();

            int length = data.Length;
            while (length > 0)
            {
                if (!this.Ignore(data[length - 1]))
                {
                    break;
                }
                length--;
            }

            int i = 0;
            int finish = length - 8;
            for (i = this.NextI(data, i, finish); i < finish; i = this.NextI(data, i, finish))
            {
                byte b1 = this.dTable[data[i++]];
                i = this.NextI(data, i, finish);
                byte b2 = this.dTable[data[i++]];
                i = this.NextI(data, i, finish);
                byte b3 = this.dTable[data[i++]];
                i = this.NextI(data, i, finish);
                byte b4 = this.dTable[data[i++]];
                i = this.NextI(data, i, finish);
                byte b5 = this.dTable[data[i++]];
                i = this.NextI(data, i, finish);
                byte b6 = this.dTable[data[i++]];
                i = this.NextI(data, i, finish);
                byte b7 = this.dTable[data[i++]];
                i = this.NextI(data, i, finish);
                byte b8 = this.dTable[data[i++]];

                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
                outStream.Add((byte)((b7 << 5) | b8));
            }
            this.DecodeLastBlock(outStream, data[length - 8], data[length - 7], data[length - 6], data[length - 5], data[length - 4], data[length - 3], data[length - 2],
                data[length - 1]);

            return outStream.ToArray();
        }

        protected virtual int DecodeLastBlock(ICollection<byte> outStream, char c1, char c2, char c3, char c4, char c5, char c6, char c7, char c8)
        {
            if (c3 == this.padding)
            {
                byte b1 = this.dTable[c1];
                byte b2 = this.dTable[c2];
                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                return 1;
            }

            if (c5 == this.padding)
            {
                byte b1 = this.dTable[c1];
                byte b2 = this.dTable[c2];
                byte b3 = this.dTable[c3];
                byte b4 = this.dTable[c4];
                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                return 2;
            }

            if (c6 == this.padding)
            {
                byte b1 = this.dTable[c1];
                byte b2 = this.dTable[c2];
                byte b3 = this.dTable[c3];
                byte b4 = this.dTable[c4];
                byte b5 = this.dTable[c5];

                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                return 3;
            }

            if (c8 == this.padding)
            {
                byte b1 = this.dTable[c1];
                byte b2 = this.dTable[c2];
                byte b3 = this.dTable[c3];
                byte b4 = this.dTable[c4];
                byte b5 = this.dTable[c5];
                byte b6 = this.dTable[c6];
                byte b7 = this.dTable[c7];

                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
                return 4;
            }

            else
            {
                byte b1 = this.dTable[c1];
                byte b2 = this.dTable[c2];
                byte b3 = this.dTable[c3];
                byte b4 = this.dTable[c4];
                byte b5 = this.dTable[c5];
                byte b6 = this.dTable[c6];
                byte b7 = this.dTable[c7];
                byte b8 = this.dTable[c8];
                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
                outStream.Add((byte)((b7 << 5) | b8));
                return 5;
            }
        }

        protected int NextI(string data, int i, int finish)
        {
            while ((i < finish) && this.Ignore(data[i]))
            {
                i++;
            }

            return i;
        }

        protected bool Ignore(char c)
        {
            return (c == '\n') || (c == '\r') || (c == '\t') || (c == ' ') || (c == '-');
        }

        protected void InitialiseDecodingTable()
        {
            for (int i = 0; i < this.eTable.Length; i++)
            {
                this.dTable[this.eTable[i]] = (byte)i;
            }
        }
    }
}
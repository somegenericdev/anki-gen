﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encoder = SevenZip.Compression.LZMA.Encoder;
using Decoder = SevenZip.Compression.LZMA.Decoder;


namespace AnkiGen.Utils
{
    public class Lzma
    {
        public static byte[] Compress(byte[] toCompress)
        {
            Encoder coder = new Encoder();

            using (MemoryStream input = new MemoryStream(toCompress))
            using (MemoryStream output = new MemoryStream())
            {

                coder.WriteCoderProperties(output);

                for (int i = 0; i < 8; i++)
                {
                    output.WriteByte((byte)(input.Length >> (8 * i)));
                }

                coder.Code(input, output, -1, -1, null);
                return output.ToArray();
            }
        }

        public static byte[] Decompress(byte[] toDecompress)
        {
            Decoder coder = new Decoder();

            using (MemoryStream input = new MemoryStream(toDecompress))
            using (MemoryStream output = new MemoryStream())
            {

                // Read the decoder properties
                byte[] properties = new byte[5];
                input.Read(properties, 0, 5);


                // Read in the decompress file size.
                byte[] fileLengthBytes = new byte[8];
                input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                coder.SetDecoderProperties(properties);
                coder.Code(input, output, input.Length, fileLength, null);

                return output.ToArray();
            }
        }
    }

}

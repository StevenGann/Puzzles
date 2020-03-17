using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCrypto
{
    internal class BadOtp
    {
        public byte[] Pad = new byte[4096];
        public int EncryptIndex = 0;
        public int DecryptIndex = 0;

        public BadOtp()
        {
            Initialize(0, 0);
        }

        public BadOtp(int Seed, int Index)
        {
            Initialize(Seed, Index);
        }

        public string Encrypt(string Plaintext)
        {
            char[] charsBuffer = Plaintext.ToCharArray();
            byte[] buffer = new byte[charsBuffer.Length];
#if DEBUG
            Console.WriteLine();
#endif
            for (int i = 0; i < charsBuffer.Length; i++)
            {
                try
                {
                    buffer[i] = (byte)(Convert.ToByte(charsBuffer[i]) ^ Pad[EncryptIndex++]);
                }
                catch
                {
                    //Unicode. Ugh. Who needs it?
                    buffer[i] = 0;
                    EncryptIndex++;
                }
#if DEBUG
                Console.Write(buffer[i] + " ");
#endif
                if (EncryptIndex >= Pad.Length) { EncryptIndex = 0; }
            }

            return Convert.ToBase64String(buffer);
        }

        public string Decrypt(string Cyphertext)
        {
            byte[] buffer = Convert.FromBase64String(Cyphertext);
            char[] charsBuffer = new char[buffer.Length];
#if DEBUG
            Console.WriteLine();
#endif
            for (int i = 0; i < charsBuffer.Length; i++)
            {
                charsBuffer[i] = Convert.ToChar((byte)(buffer[i] ^ Pad[DecryptIndex++]));
#if DEBUG
                Console.Write(buffer[i] + " ");
#endif
                if (DecryptIndex >= Pad.Length) { DecryptIndex = 0; }
            }

            return new string(charsBuffer);
        }

        public void Initialize(int Seed, int Index)
        {
            EncryptIndex = Index;
            DecryptIndex = Index;
            BadRandom RNG = new BadRandom(Seed);

#if DEBUG
            int[] counts = new int[256];
            Console.Write("Generating OTP\n\t");
#endif
            for (int i = 0; i < Pad.Length; i++)
            {
                byte b = RNG.NextByte();
                Pad[i] = b;

#if DEBUG
                counts[b]++;
                Console.Write(b.ToString() + "\t");
#endif
            }
#if DEBUG
            Console.Write("\n");
            Console.Write("OTP Distribution\n\t");
            for (int i = 0; i < 256; i++)
            {
                Console.Write(counts[i] + "\t");
            }
#endif
        }
    }
}
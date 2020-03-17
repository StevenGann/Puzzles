using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCrypto
{
    class Program
    {
        static void Main(string[] args)
        {
            BadOtp otp = new BadOtp();

            if (args.Length > 0)
            {
                if (args[0].ToLower().Contains(".txt"))
                {
                    if(args.Length >= 2)
                    {
                        try
                        {
                            int seed = int.Parse(args[1]);
                            otp = new BadOtp(seed, 0);
                        }
                        catch
                        {
                            Console.WriteLine("\nFailed to parse OTP seed! Using default.");
                            Console.ReadLine();
                        }
                    }

                    if (args[0].ToLower().Contains("encrypted"))
                    {
                        Console.WriteLine("Decrypting file...");
                        System.IO.StreamReader inputFile = new System.IO.StreamReader(args[0]);
                        System.IO.StreamWriter outFile = new System.IO.StreamWriter("decrypted." + Convert.ToString(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) + ".txt");
                        string line;
                        while ((line = inputFile.ReadLine()) != null)
                        {
                            Console.Write(".");
                            string plaintext = otp.Decrypt(line);
                            outFile.WriteLine(plaintext);
                        }

                        inputFile.Close();
                        outFile.Close();
                    }
                    else
                    {
                        Console.WriteLine("Encrypting file...");
                        Console.Write(".");
                        System.IO.StreamReader inputFile = new System.IO.StreamReader(args[0]);
                        System.IO.StreamWriter outFile = new System.IO.StreamWriter("encrypted." + Convert.ToString(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) + ".txt");
                        string line;
                        while ((line = inputFile.ReadLine()) != null)
                        {
                            string ciphertext = otp.Encrypt(line);
                            outFile.WriteLine(ciphertext);
                        }

                        inputFile.Close();
                        outFile.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Unsupported argument");
                }
            }
            else
            {
                Console.Write("\nInteractive Mode");
                Console.WriteLine("\n====================================================");
                Console.WriteLine("Enter some text to send securely!");
                while (true)
                {
                    string ciphertext = otp.Encrypt(Console.ReadLine());
                    string plaintext = otp.Decrypt(ciphertext);
                    Console.WriteLine("\n====================================================");
                    Console.Write("Encrypted:\n" + ciphertext);
                    Console.WriteLine("\n----------------------------------------------------");
                    Console.Write("Decrypted:\n" + plaintext);
                    Console.WriteLine("\n====================================================");
                }
            }
        }
    }
}

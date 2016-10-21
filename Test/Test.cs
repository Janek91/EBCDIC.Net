using System;
using System.Diagnostics;
using System.Text;
using Ebcdic = EbcdicEncoding.EbcdicEncoding;

namespace Test
{
    /// <summary>
    /// Very simple test class for the EBCDIC encoding
    /// </summary>
    internal class Test
    {
        private static void Main(string[] args)
        {
            // Display all the available names
            Console.WriteLine("Available encodings:");
            foreach (string name in Ebcdic.AllNames)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine();

            // Encode and decode "hello" in the UK variant
            Encoding e = Ebcdic.GetEncoding(Ebcdic.AvailableEncodings.EBCDIC_UK);
            Console.WriteLine("Encoding of \"hello\":");
            byte[] encoded = e.GetBytes("hello");
            Console.WriteLine(BitConverter.ToString(encoded));
            Console.WriteLine("Decoded from above: ");
            Console.WriteLine(e.GetString(encoded));
            Console.ReadLine();
        }
    }
}

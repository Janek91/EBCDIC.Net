using System;
using System.Text;

namespace JonSkeet.Ebcdic
{
	/// <summary>
	/// Very simple test class for the EBCDIC encoding
	/// </summary>
	class Test
	{
		static void Main(string[] args)
		{
            // Display all the available names
            Console.WriteLine ("Available encodings:");
            foreach (string name in EbcdicEncoding.AllNames)
            {
                Console.WriteLine (name);
            }
            Console.WriteLine ();

            // Encode and decode "hello" in the UK variant
            Encoding e = EbcdicEncoding.GetEncoding("EBCDIC-UK");
            Console.WriteLine ("Encoding of \"hello\":");
            byte[] encoded = e.GetBytes("hello");
            Console.WriteLine (BitConverter.ToString(encoded));
            Console.WriteLine ("Decoded from above: ");
            Console.WriteLine (e.GetString (encoded));
            Console.ReadLine();
        }
	}
}

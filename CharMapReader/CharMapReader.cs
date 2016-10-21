using System;
using System.IO;
using System.Collections;
using System.Text;

namespace JonSkeet.EBCDIC
{
    /// <summary>
    /// Class to read in files in the format at http://std.dkuug.dk/i18n/charmaps/
    /// and convert them all into a resource suitable for EbcdicEncoding. All
    /// files beginning with "EBCDIC-" in the current directory are read, and
    /// the resource is written to ebcdic.dat at the end.
    /// Format of resource file is:
    /// Number of encodings (single byte)
    /// For each encoding, the name as a length-prefixed (single byte length) ASCII string
    /// followed by the map from byte to character, with each character being given
    /// in big-endian order. 0 represents an unspecified character (no mapping)
    /// (or an actual 0 for the first entry).
    /// The name of each encoding is assumed to be unique, and entirely
    /// ASCII.
    /// </summary>
    public class CharMapReader
    {
        char[] byteToCharMap = new char[256];
    
        string name;
        bool inMap=false;
    
        CharMapReader(string name)
        {
            this.name = name;
        }

        void ParseLine (string line)
        {
            if (line.Trim()=="")
                return;
            else if (line.Trim()=="CHARMAP")
            {
                inMap=true;
            }
            else if (line.Trim()=="END CHARMAP")
            {
                inMap=false;
            }
            else if (inMap)
            {
                if (line.Length < 38)
                {
                    Console.WriteLine ("Invalid line in map: {0}", line);
                    return;
                }
                try
                {
                    int ebcdic = Convert.ToInt32 (line.Substring (25, 2), 16);
                    if (ebcdic==0)
                        return;
                    int unicode = Convert.ToInt32 (line.Substring (32, 4), 16);
                
                    if (ebcdic < 0 || ebcdic > 255 || unicode < 0 || unicode > 0xffff)
                        throw new ArgumentException();
                
                    if (byteToCharMap[ebcdic] != 0 && byteToCharMap[ebcdic] != unicode)
                    {
                        Console.WriteLine ("Conflicting map for {0:x}. Previous value: {1:x}, new value: {2:x}",
                            ebcdic, byteToCharMap[ebcdic], unicode);
                        return;
                    }
                    byteToCharMap[ebcdic]=(char)unicode;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine ("Invalid line in map: {0}", line);
                    return;
                }
            }
        }

        void WriteTo (Stream stream)
        {
            stream.WriteByte((byte)name.Length);
            stream.Write (Encoding.ASCII.GetBytes(name), 0, name.Length);
            byte[] bytes = new byte[512];
            for (int i=0; i < 256; i++)
            {
                bytes[i*2]=(byte)(byteToCharMap[i]>>8);
                bytes[i*2+1]=(byte)(byteToCharMap[i]&0xff);
            }
            stream.Write(bytes, 0, bytes.Length);
        }
    
        public static void Main(string[] args)
        {
            IList maps = new ArrayList();
        
            DirectoryInfo cwd = new DirectoryInfo (".");
            FileInfo[] files = cwd.GetFiles ("EBCDIC-*");
            foreach (FileInfo fi in files)
            {
                CharMapReader cmr = new CharMapReader(fi.Name.Substring(7));
                maps.Add (cmr);
                Console.WriteLine ("Reading {0}", fi.Name);
            
                using (StreamReader sr = new StreamReader (fi.Name))
                {
                    string line;
                
                    while ( (line=sr.ReadLine()) != null)
                        cmr.ParseLine (line);
                }
            }
            Console.WriteLine ("Writing data file");
            using (FileStream fs = new FileStream("ebcdic.dat", FileMode.Create))
            {
                fs.WriteByte((byte)maps.Count);
                foreach (CharMapReader cmr in maps)
                {
                    cmr.WriteTo(fs);
                }
            }
            Console.WriteLine ("Done");
        }
    }
}

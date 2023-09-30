using System.IO;

namespace BlitzedConfuser.Protections
{
    public class StripDOSHeader
    {
        private static readonly uint offset_lfanew = 0x3C;
        private static readonly int length_lfanew = sizeof(uint);

        private static readonly uint offset_magic = 0x00;
        private static readonly int length_magic = sizeof(ushort);

        public static void Execute()
        {
            byte[] blank_dos = new byte[64];
            byte[] magic = ReadArray(offset_magic, length_magic, Kappa.Stream);
            byte[] lfanew = ReadArray(offset_lfanew, length_lfanew, Kappa.Stream);

            Kappa.Stream.Position = 0;
            WriteArray(0, blank_dos, Kappa.Stream);
            WriteArray(offset_magic, magic, Kappa.Stream);
            WriteArray(offset_lfanew, lfanew, Kappa.Stream);
            WriteArray(0x4e, new byte[39], Kappa.Stream); // Overrides "This Kappa can not be run in DOS mode."
        }

        private static byte[] ReadArray(uint offset, int length, Stream stream)
        {
            var data = new byte[length];
            stream.Position = offset;
            stream.Read(data, 0, data.Length);
            return data;
        }

        private static int WriteArray(uint offset, byte[] data, Stream stream)
        {
            stream.Position = offset;
            stream.Write(data, 0, data.Length);
            return data.Length;
        }
    }
}

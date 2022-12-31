using System.Text;

namespace try_reverse.service
{
    public class PatchService : IPatchService
    {
        public int SearchPattern(byte[] patch, byte[] replacement, ref string data)
        {
            int len = patch.Length;
            int count = 0;

            byte[] dataBytes = StringToByteArray(data);

            for (int i = 0; i < dataBytes.Length - len + 1; i++)
            {
                if (dataBytes.Skip(i).Take(len).SequenceEqual(patch))
                {
                    Array.Copy(replacement, 0, dataBytes, i, len);
                    count++;
                }
            }

            data = GetString(dataBytes);

            return count;
        }

        public byte[] GetPatchBytes(string patch)
        {
            var bytes = new List<byte>();
            for (int i = 0; i < patch.Length; i += 2)
            {
                bytes.Add(Convert.ToByte(patch.Substring(i, 2), 16));
            }
            return bytes.ToArray();
        }

        public string GetString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public void GeneratePatchedFile(string outfile, string data, int count, string patched, string replacement)
        {
            byte[] modifiedBytes = StringToByteArray(data);
            File.WriteAllBytes(outfile, modifiedBytes);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Patched {count} occurences of {patched} with {replacement}.\n");
            Console.ResetColor();
        }

        private static byte[] StringToByteArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                    .ToArray();
        }
    }
}

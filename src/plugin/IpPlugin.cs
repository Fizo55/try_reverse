namespace try_reverse.plugin
{
    public class IpPlugin : IPlugin
    {
        private readonly IPatchService _patchService;

        public IpPlugin(IPatchService patchService)
        {
            _patchService = patchService;
        }

        public void Execute(string path, string data)
        {
            byte[] patch = { 0xC, 0x0, 0x0, 0x0, 0x37, 0x39, 0x2E, 0x31, 0x31, 0x30, 0x2E, 0x38, 0x34, 0x2E, 0x37, 0x35, 0x0, 0x0, 0x0, 0x0 };
            Console.WriteLine("What ip would you want to use ?\n");
            string ip = Console.ReadLine()!;
            string[] parts = ip.Split('.');
            var hexParts = parts.Select(part => Convert.ToInt32(part).ToString("X2")).ToList();
            var hexString = string.Join("2E", hexParts);
            while (hexString.Length < 40)
            {
                hexString += "0";
            }
            byte[] replacement = _patchService.GetPatchBytes(hexString);
            int count = _patchService.SearchPattern(patch, replacement, ref data);
            // TODO : Remove hardcoded outfile name
            _patchService.GeneratePatchedFile(Path.Combine(path, "localhost.exe"), data, count, _patchService.GetString(patch), _patchService.GetString(replacement));
        }
    }
}

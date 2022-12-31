namespace try_reverse;

public interface IPatchService
{
    int SearchPattern(byte[] patch, byte[] replacement, ref string data);

    byte[] GetPatchBytes(string patch);

    string GetString(byte[] bytes);

    void GeneratePatchedFile(string outfile, string data, int count, string patched, string replacement);
}
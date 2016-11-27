using System.IO;

namespace ClientShared
{
    public class Pict
    {

        public byte[] PictBytes { get; }
        public string PathToResult { get; set; }

        public Pict(string path)
        {
            PictBytes = File.ReadAllBytes(path);
            PathToResult = null;
        }

        public void SavePict(byte[] dataBytes)
        {
            File.WriteAllBytes(PathToResult, dataBytes);
        }
    }
}
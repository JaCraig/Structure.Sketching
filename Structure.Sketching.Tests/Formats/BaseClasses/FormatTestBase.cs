using System.IO;
using System.Linq;

namespace Structure.Sketching.Tests.Formats.BaseClasses
{
    public abstract class FormatTestBase
    {
        protected FormatTestBase()
        {
            new DirectoryInfo(OutputDirectory).Create();
        }

        public abstract string ExpectedDirectory { get; }

        public abstract string InputDirectory { get; }
        public abstract string OutputDirectory { get; }

        protected bool CheckFileCorrect(string fileName)
        {
            using (FileStream OutputStream = File.OpenRead(OutputDirectory + fileName))
            {
                using (FileStream ExpectedStream = File.OpenRead(ExpectedDirectory + fileName))
                {
                    return ReadBinary(OutputStream).SequenceEqual(ReadBinary(ExpectedStream));
                }
            }
        }

        protected byte[] ReadBinary(FileStream stream)
        {
            byte[] Buffer = new byte[1024];
            using (MemoryStream Temp = new MemoryStream())
            {
                while (true)
                {
                    var Count = stream.Read(Buffer, 0, Buffer.Length);
                    if (Count <= 0)
                        return Temp.ToArray();
                    Temp.Write(Buffer, 0, Count);
                }
            }
        }
    }
}
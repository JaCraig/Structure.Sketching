using System.IO;
using System.Linq;

namespace Structure.Sketching.Tests.BaseClasses
{
    public abstract class TestBaseClass
    {
        protected TestBaseClass()
        {
            Directory.CreateDirectory(OutputDirectory);
            Directory.CreateDirectory(ExpectedDirectory);
        }

        public abstract string ExpectedDirectory { get; }
        public abstract string OutputDirectory { get; }

        protected bool CheckFileCorrect(string expectedFilePath, string outputFilePath)
        {
            using (FileStream OutputStream = File.OpenRead(outputFilePath))
            {
                using (FileStream ExpectedStream = File.OpenRead(expectedFilePath))
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
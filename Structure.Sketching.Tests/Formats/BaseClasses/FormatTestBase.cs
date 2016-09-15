using System.IO;
using System.Linq;

namespace Structure.Sketching.Tests.Formats.BaseClasses
{
    public abstract class FormatTestBase
    {
        public abstract string ExpectedOutputFileName { get; }
        public abstract string InputFileName { get; }

        public abstract string OutputFileName { get; }

        protected bool CheckFileCorrect()
        {
            using (FileStream OutputStream = File.OpenRead(OutputFileName))
            {
                using (FileStream ExpectedStream = File.OpenRead(ExpectedOutputFileName))
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
                    int Count = stream.Read(Buffer, 0, Buffer.Length);
                    if (Count <= 0)
                        return Temp.ToArray();
                    Temp.Write(Buffer, 0, Count);
                }
            }
        }
    }
}
using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Tidalwave;

namespace Tidalwave.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class ContextBenchmarks
    {
        private readonly byte[] _byteArray = new byte[1024];
        private readonly ulong[] _buffer = new ulong[1048576]; 

        [GlobalSetup]
        public void Setup() {
            Random random = new Random();
            random.NextBytes(_byteArray);
            
            WriteContext writeContext = new WriteContext(_buffer);
            writeContext.WriteBytes(_byteArray);
        }

        [Benchmark]
        public void WriteByte() {
            WriteContext writeContext = new WriteContext(_buffer);
            writeContext.WriteByte(0xDE);
        }

        [Benchmark]
        public void ReadByte() {
            ReadContext readContext = new ReadContext(_buffer);
            readContext.ReadByte();
        }

        [Benchmark]
        public void WriteByteArray() {
            WriteContext writeContext = new WriteContext(_buffer);
            writeContext.WriteBytes(_byteArray);
        }

        [Benchmark]
        public void ReadByteArray() {
            ReadContext readContext = new ReadContext(_buffer);
            readContext.ReadBytes();
        }

        [Benchmark]
        public void WriteRandomBoolsAndOtherTypes() {
            WriteContext writeContext = new WriteContext(_buffer);
            Random random = new Random();
            for (int i = 0; i < 1000; i++) {
                writeContext.WriteBool(random.Next(2) == 1);
                writeContext.WriteInt(random.Next());
                writeContext.WriteDouble(random.NextDouble());
            }
        }
    }
}

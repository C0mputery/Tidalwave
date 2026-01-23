using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Tidalwave {
    public static class BufferPool {
        public static int BufferSizeInBytes { get; private set; } = 1024;
        public static void Initialize(int bufferSizeInBytes, int initialPoolSize) {
            BufferSizeInBytes = bufferSizeInBytes;
            int ulongArraySize = BufferSizeInBytes / sizeof(ulong);
            for (int i = 0; i < initialPoolSize; i++) { Pool.Push(new ulong[ulongArraySize]); }
        }

        private static readonly ConcurrentStack<ulong[]> Pool = new ConcurrentStack<ulong[]>();
        
        public static ulong[] Rent() { 
            if (Pool.TryPop(out ulong[]? buffer)) { return buffer; }
            return new ulong[BufferSizeInBytes];
        }
        
        public static byte[] RentAsBytes() {
            ulong[] buffer = Rent();
            Span<byte> a = MemoryMarshal.AsBytes<ulong>(buffer);
            return a.ToArray();
        }
        
        public static void Return(ulong[] buffer) { Pool.Push(buffer); }
    }
}
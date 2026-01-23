using System.Collections.Concurrent;

namespace Tidalwave {
    public static class BufferPool {
        private static int _bufferSizeInBytes = 1024;
        public static void Initialize(int bufferSizeInBytes, int initialPoolSize) {
            _bufferSizeInBytes = bufferSizeInBytes;
            int ulongArraySize = _bufferSizeInBytes / sizeof(ulong);
            for (int i = 0; i < initialPoolSize; i++) { Pool.Push(new ulong[ulongArraySize]); }
        }

        private static readonly ConcurrentStack<ulong[]> Pool = new ConcurrentStack<ulong[]>();
        
        public static ulong[] Rent() { 
            if (Pool.TryPop(out ulong[]? buffer)) { return buffer; }
            return new ulong[_bufferSizeInBytes];
        }
        
        public static void Return(ulong[] buffer) { Pool.Push(buffer); }
    }
}
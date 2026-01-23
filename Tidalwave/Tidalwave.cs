namespace Tidalwave {
    public static class Tidalwave {
        public static void Initialize(int bufferSizeInBytes = 1024, int initialPoolSize = 10) {
            MessageHandlerRegistry.Initialize();
            BufferPool.Initialize(bufferSizeInBytes, initialPoolSize);
        }
    }
}
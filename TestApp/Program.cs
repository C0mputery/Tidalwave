using Tidalwave.Logger;

namespace TestApp;

static class Program {
    static void Main(string[] args) {
        TidalwaveLogger.Initialize(Console.WriteLine, true);
    }
}
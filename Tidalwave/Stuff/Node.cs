using System;
using System.Runtime.InteropServices;
using ENet;
using Tidalwave.Logger;

namespace Tidalwave {
    public class Node {
        public readonly Host ENetHost = new Host();
        public Address ENetAddress = new Address();
        public Event ENetEvent = new Event();
        
        public void Poll() {
            while (ENetHost.CheckEvents(out ENetEvent) > 0) { HandlePacket(); }
            if (ENetHost.Service(0, out ENetEvent) > 0) { HandlePacket(); }
        }
        
        private void HandlePacket() { 
            switch (ENetEvent.Type) {
                case EventType.None: { break; } 
                case EventType.Connect: { break; }
                case EventType.Disconnect: { break; }
                case EventType.Timeout: { break; }
                case EventType.Receive: {
                    ulong[] buffer = BufferPool.Rent();
                    try { HandleUserPacket(buffer); }
                    catch (Exception e) { TidalwaveLogger.LogError($"Error handling user packet: {e}"); }
                    BufferPool.Return(buffer);
                    ENetEvent.Packet.Dispose();
                    break;
                }
            }
        }

        private void HandleUserPacket(ulong[] buffer) {
            int packetLength = ENetEvent.Packet.Length;
            if (packetLength > BufferPool.BufferSizeInBytes) {
               TidalwaveLogger.LogWarning("Received packet larger than buffer size."); 
                return;
            }

            unsafe {
                ReadOnlySpan<byte> source = new ReadOnlySpan<byte>(ENetEvent.Packet.Data.ToPointer(), packetLength);
                if (!source.TryCopyTo(MemoryMarshal.AsBytes<ulong>(buffer))) {
                    TidalwaveLogger.LogError("Failed to copy packet data to buffer."); 
                    return;
                }
            }
            
            int relevantUlongs = (packetLength + sizeof(ulong) - 1) / sizeof(ulong);
            ReadContext readContext = new ReadContext(new ReadOnlySpan<ulong>(buffer, 0, relevantUlongs));
            int totalBits = readContext.ReadInt();
            readContext.SetBitCapacity(totalBits);
        }
    }
}
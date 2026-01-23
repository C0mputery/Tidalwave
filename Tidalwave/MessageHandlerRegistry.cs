// TODO: support async message handlers.
// TODO:
//  Source gen a switch statement for message handlers to avoid unnecessary dictionary lookups.
//  Still support the dictionary for dynamic registration for mods and stuff
// TODO:
//  ponder supporting multiple handlers per message ID with priority levels
//  may be complex with source gen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tidalwave.Logger;

namespace Tidalwave {
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class MessageHandlerAttribute : Attribute {
        public readonly ushort MessageId;
        public MessageHandlerAttribute(ushort messageId) { MessageId = messageId; }
    }
    
    internal delegate void MessageHandler(ReadContext message);
    
    public static class MessageHandlerRegistry {
        private const BindingFlags StaticBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly Dictionary<ushort, MessageHandler> Messages = new();
        private static void RegisterMessageHandlers() {
            string thisAssemblyName = Assembly.GetExecutingAssembly().GetName().FullName;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies) {
                Type[] types = assembly.GetTypes();
                
                AssemblyName[] assemblyNames = assembly.GetReferencedAssemblies();
                bool referencesThisAssembly = assemblyNames.Any(name => name.FullName == thisAssemblyName);
                if (!referencesThisAssembly) { continue; }
                
                foreach (Type type in types) {
                    MethodInfo[] methods = type.GetMethods(StaticBindingFlags);
                    foreach (MethodInfo method in methods) {
                        IEnumerable<MessageHandlerAttribute> attributes = method.GetCustomAttributes<MessageHandlerAttribute>(false);
                        
                        foreach (MessageHandlerAttribute attribute in attributes) {
                            if (method.CreateDelegate(typeof(MessageHandler)) is MessageHandler handler) {
                                if (!Messages.TryAdd(attribute.MessageId, handler)) {
                                    TidalwaveLogger.Log(LogType.Error, $"Duplicate message handler for Message ID {attribute.MessageId} found in method {method.Name} in type {type.FullName}. This message ID is already handled by another method.");
                                }
                            } 
                            else { TidalwaveLogger.Log(LogType.Error, $"Method {method.Name} in type {type.FullName} has a MessageHandlerAttribute but does not match the MessageHandler delegate signature."); }
                        }
                    }
                }
            }
        }
        
        internal static void Initialize() { RegisterMessageHandlers(); }
        
        public static void HandleMessage(ushort messageId, ReadContext message) {
            if (Messages.TryGetValue(messageId, out MessageHandler? handler)) { handler.Invoke(message); }
            else { TidalwaveLogger.Log(LogType.Warning, $"No message handler found for Message ID {messageId}."); }
        }
    }
}
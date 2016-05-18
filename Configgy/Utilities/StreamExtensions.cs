using System;
using System.IO;

namespace Configgy.Utilities
{
    internal static class StreamExtensions
    {
        public static void WriteInt(this Stream stream, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}

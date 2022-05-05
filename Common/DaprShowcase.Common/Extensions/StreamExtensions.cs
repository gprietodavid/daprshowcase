using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaprShowcase.Common.Extensions
{
    public static class StreamExtensions
    {
        public static string ConvertToBase64String(this Stream stream)
        {
            byte[] bytes;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }
        public static Stream ConvertToBase64Stream(this Stream stream)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(ConvertToBase64String(stream)));
        }
        public static byte[] ToByteArray(this Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
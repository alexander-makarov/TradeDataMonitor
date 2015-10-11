using System.IO;

namespace TestHelpers
{
    public static class Helpers
    {
        /// <summary>
        /// Generate MemoryStream from string value
        /// http://stackoverflow.com/questions/1879395/how-to-generate-a-stream-from-a-string
        /// </summary>
        /// <param name="s">string value</param>
        /// <returns>Stream object</returns>
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
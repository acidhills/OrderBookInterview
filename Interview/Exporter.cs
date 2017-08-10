using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OrderBookInterview
{
    public class Exporter
    {
        public static void Export(Stream stream, double[] doubles)
        {
            var bs = new BinaryFormatter();
            bs.Serialize(stream, doubles);
        }
    }
}

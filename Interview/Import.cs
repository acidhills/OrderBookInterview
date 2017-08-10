using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OrderBookInterview
{
    public static class Importer
    {
        public static double[] Import(Stream stream)
        {
            var bs = new BinaryFormatter();
            return (double[]) bs.Deserialize(stream);
        }
    }
}

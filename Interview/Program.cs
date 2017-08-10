using System;
using System.IO;

namespace OrderBookInterview
{
    partial class Program
    {
        static void Main(string[] args)
        {

            //var orderBook = new[] { 57085, 154, 57084, 53, 57083, 31, 57082, 2, double.NaN, double.NaN, 57081, 1, 57080, 12, 57079, 30, 57078, 72, 57077, 26 };
            //var neworderBook = new[] { 57084, 53, 570831, 31, 57082, 2, 1, 154, double.NaN, double.NaN, 57081, 1, 57080, 12, 57079, 30, 57078, 72, 57077, 26 };
            double[] orderBook = null;
            double[] neworderBook = null;
            using (var fs = File.OpenRead("oldBook.dat") )
            {
                orderBook = Importer.Import(fs);
            }

            using (var fs = File.OpenRead("newBook.dat"))
            {
                neworderBook = Importer.Import(fs);
            }
            Show(orderBook);
            Show(neworderBook);
            var diff = DiffHelper.FindDiff(orderBook, neworderBook);
            var restored = DiffHelper.Restore(orderBook, diff);
            Show(restored);

            using (var fs = File.OpenWrite("resoredBook.dat"))
            {
                Exporter.Export(fs,restored);
            }

        }

        private static void Show(double[] source)
        {
            int i = 0;
            foreach (var item in source)
            {
                i++;
                Console.Write(item + " ");
                if (i == 2)
                {
                    i = 0;
                    Console.Write("| ");
                }
            }
            Console.WriteLine();
        }
        
    }
}

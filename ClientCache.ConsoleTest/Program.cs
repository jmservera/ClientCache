using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCache.ConsoleTest
{
class Program
{
    static string[] pictures = new string[]{
    "https://farm2.staticflickr.com/1381/1310759230_9203a83da3.jpg",
    "https://farm3.staticflickr.com/2345/2077570455_03891081db.jpg",
    "https://farm3.staticflickr.com/2272/1973927918_ce00011ef5.jpg"
    };

    static void Main(string[] args)
    {
        var index = 0;
        while (true)
        {
            var uriIndex = index++ % pictures.Length;
            var uri = pictures[uriIndex];
            byte[] file = null;

            Console.WriteLine(uri);
            System.Net.WebClient c = new System.Net.WebClient();
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            file = c.DownloadData(uri);
            Console.WriteLine("Non Cached WebClient Ellapsed: {0}", sw.ElapsedMilliseconds);

            c.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.Revalidate);
            sw = System.Diagnostics.Stopwatch.StartNew();
            file = c.DownloadData(uri);
            Console.WriteLine("Cached WebClient Ellapsed: {0}", sw.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
}

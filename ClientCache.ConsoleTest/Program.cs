namespace ClientCache.ConsoleTest
{
    using System;
    using System.Diagnostics;
    using System.Net.Cache;
    class Program
    {
        static string[] pictures = new string[]{
            "http://stlab.adobe.com/wiki/images/d/d3/Test.pdf",
            "http://media.w3.org/2010/05/sintel/trailer.mp4"
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
                Stopwatch sw = Stopwatch.StartNew();
                file = c.DownloadData(uri);
                Console.WriteLine("Non Cached WebClient Ellapsed: {0}", sw.ElapsedMilliseconds);

                c.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Revalidate);
                sw = Stopwatch.StartNew();
                file = c.DownloadData(uri);
                Console.WriteLine("Cached WebClient Ellapsed: {0}", sw.ElapsedMilliseconds);

                Console.ReadLine();
            }
        }
    }
}
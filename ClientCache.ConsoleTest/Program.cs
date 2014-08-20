namespace ClientCache.ConsoleTest
{
    using System;
    using System.Diagnostics;
    using System.Net.Cache;
    class Program
    {
        static string[] elements = new string[]{
            "http://stlab.adobe.com/wiki/images/d/d3/Test.pdf",
            "http://techslides.com/demos/sample-videos/small.mp4"
            };

        static void Main(string[] args)
        {
            var index = 0;
            while (true)
            {
                var uriIndex = index++ % elements.Length;
                var uri = elements[uriIndex];

                Console.WriteLine(uri);
                var request = System.Net.HttpWebRequest.CreateHttp(uri);

                Console.WriteLine("Start WebRequest without headers");
                Stopwatch sw = Stopwatch.StartNew();
                var response = request.GetResponse();
                byte[] file = readAllBytes(response);

                Console.WriteLine("Simple WebRequest Ellapsed: {0} FileLength: {1} IsFromCache: {2}",
                    sw.ElapsedMilliseconds, file.Length, response.IsFromCache);

                var lastModified= response.Headers[System.Net.HttpResponseHeader.LastModified];

                Console.WriteLine();
                Console.WriteLine("Start WebRequest with IfModifiedSince {0}",lastModified);

                request = System.Net.HttpWebRequest.CreateHttp(uri);
                if(lastModified!=null)
                {
                    request.IfModifiedSince = DateTime.Parse(lastModified);
                }
                sw = Stopwatch.StartNew();
                try
                {
                    file=new byte[0];
                    response = request.GetResponse();
                    file = readAllBytes(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("WebRequest Ellapsed: {1} FileLenght: {2}", lastModified, sw.ElapsedMilliseconds, file.Length);

                Console.WriteLine();
                Console.WriteLine("Press return.");
                Console.ReadLine();
            }
        }

        private static byte[] readAllBytes(System.Net.WebResponse response)
        {
            Console.Write("Reading stream ");
            using (var stream = response.GetResponseStream())
            {
                using (var m = new System.IO.MemoryStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesread;
                    while ((bytesread = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        m.Write(buffer, 0, bytesread);
                        Console.Write('.');
                    }
                    Console.WriteLine();
                    return m.ToArray();
                }
            }
        }
    }
}
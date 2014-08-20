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
                int fileLength = readAllBytes(response);

                Console.WriteLine("Simple WebRequest Ellapsed: {0} FileLength: {1} IsFromCache: {2}",
                    sw.ElapsedMilliseconds, fileLength, response.IsFromCache);

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
                    response = request.GetResponse();
                    fileLength = readAllBytes(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("WebRequest Ellapsed: {1} FileLenght: {2}", lastModified, sw.ElapsedMilliseconds, fileLength);

                Console.WriteLine();
                Console.WriteLine("Press return to continue, CTRL+C to finish.");
                Console.ReadLine();

            }
        }

        private static int readAllBytes(System.Net.WebResponse response)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = System.IO.Path.Combine(appData, "jmservera\\ClientCache");
            System.IO.Directory.CreateDirectory(path);

            var filePath = System.IO.Path.Combine(path, response.ResponseUri.LocalPath.Replace('/','_'));

            Console.Write("Reading stream ");
            using (var stream = response.GetResponseStream())
            {
                using (var m = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate))
                {
                    m.SetLength(0);
                    byte[] buffer = new byte[4096];
                    int bytesread;
                    int totalBytes=0;
                    while ((bytesread = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytes += bytesread;
                        m.Write( buffer, 0, bytesread);
                        Console.Write('.');
                    }
                    Console.WriteLine();
                    return totalBytes;
                }
            }
            
        }
    }
}
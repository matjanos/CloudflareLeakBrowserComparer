using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public const string UrlRegex = @"^((http[s]?|ftp):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$";
        public static void Main(string[] args)
        {
            Console.WriteLine("Preparing data...");
            var importer = new Importer(args[0]);
            var history = importer.GetCollectionOfDomains();
            var leakedHosts = GetLeakedHosts(args[1]);
            var checkedDomains = new ConcurrentDictionary<string, byte>();
            Console.WriteLine();
            Console.WriteLine("Data prepared. Started Processing...");

            using (var writer = new StreamWriter(File.Open("result.txt", FileMode.Create)))
            {
                int i = 0;
                Parallel.ForEach<HistoryElement, int>(history.BrowserHistory, () => 0, (url, loop, subtotal) =>
                         {
                             subtotal++;
                             var match = Regex.Match(url.url, UrlRegex);
                             var host = match.Groups[3].Value;
                             if (checkedDomains.ContainsKey(host))
                             {
                                 return subtotal;
                             }

                             if (!String.IsNullOrWhiteSpace(host)
                              && leakedHosts.ContainsKey(host))
                             {
                                 writer.WriteLine($"Address {host}");
                             }
                             checkedDomains.TryAdd(host, 0);
                             return subtotal;
                         }, (finalResult) =>
                         {
                             Interlocked.Add(ref i, finalResult);
                             lock (importer)
                                 ProgressBar(i, history.BrowserHistory.Length);
                         });
            }
        }

        private static ConcurrentDictionary<string,byte> GetLeakedHosts(string path)
        {
            var orderedLines = new ConcurrentDictionary<string, byte>();
            var lines = File.ReadLines(path).ToArray();
            int k = 0;
            foreach (var l in lines)
            {
                orderedLines.TryAdd(l, 0);
                if (++k % 10000 == 0)
                    ProgressBar(k, lines.Length);
            }

            return orderedLines;
        }

        private static void ProgressBar(int progress, int tot)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / tot;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + tot.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}

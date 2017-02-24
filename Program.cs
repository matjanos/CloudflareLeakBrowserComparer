using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var importer = new Importer("BrowserHistory.json");
            var history = importer.GetCollectionOfDomains();

            using (var reader = new StreamReader(File.Open("sorted_unique_cf.txt", FileMode.Open)))
            using (var writer = new StreamWriter(File.Open("result.txt", FileMode.Create)))
            {
                int i = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (history.BrowserHistory.Any(x =>
                    {
                        var url = new Uri(x.url);
                        return url.Host == line;
                    }))
                    {
                        writer.WriteLine($"Address {line}");
                    }
                    ProgressBar(i, 4288853);
                }
            }
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

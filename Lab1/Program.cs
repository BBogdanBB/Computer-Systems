using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            ExampleTextAnalazer(@"C:\MyFiles\1e.txt");
            ExampleBase64Encoder();


            Console.ReadLine();
        }

        public static void ExampleBase64Encoder()
        {
            Base64Encoder.Encode(@"C:\MyFiles\1.txt", @"C:\MyFiles\1e.txt");
            Console.WriteLine(Base64Encoder.CheckCorrect(@"C:\MyFiles\1.txt", @"C:\MyFiles\1e.txt"));
        }
        public static void ExampleTextAnalazer(string path)
        {
            TextAnalyzer ta = new TextAnalyzer(path);
            foreach (var item in ta.Frequency)
                Console.WriteLine(item.Key + " " + item.Value);
            Console.WriteLine("Avarage entropy " + ta.AverageEntropy);
            Console.WriteLine("Info size " + ta.Size);
        }
    }
}

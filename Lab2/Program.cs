using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 232, b = -91;
            var(def1, binAnsw1, answ1) = BoothMultiplicator.Multiply(a, b);
            Console.WriteLine(a + " * " + b);
            Console.WriteLine(def1);
            Console.WriteLine($"answ : {binAnsw1} = {answ1}");
            Console.WriteLine();
            Console.WriteLine();

            var (def2, binAnsw2, answ2) = SumFloatingPoint.Add(1231F, 0.21F);
            Console.WriteLine(def2);
            Console.WriteLine($"answ : {binAnsw2} = {answ2}");
            Console.WriteLine();
            Console.WriteLine();
            
            var (def3, r, q) = BinaryDivision.Divide(a, b);
            Console.WriteLine(def3);
            Console.WriteLine($"answ : remainder = {r} quotient = {q}");
            Console.Read();
        }

        
    }
}

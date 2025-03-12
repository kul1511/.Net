using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practise
{
    internal class PrimeNumber
    {
        static public void primeNumber()
        {
            int num;
            Console.WriteLine("Enter a Number: ");
            num = Convert.ToInt32(Console.ReadLine());

            int counter = 0;
            if (num == 1) Console.WriteLine("Not a Prime Number");
            else
            {
                for(int i=2;i<Math.Sqrt(num);i++)
                {
                    if (num % i == 0) counter++;
                }
                if (counter >= 1) Console.WriteLine("Not Prime");
                else Console.WriteLine("Prime");
            }
        }
    }
}

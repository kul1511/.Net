using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practise
{
     class AddTwoNumbers
    {
        static public void AddToNumber()
        {
            int a, b, c = 0;
            Console.WriteLine("Enter a: ");
            a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter b: ");
            b = Convert.ToInt32(Console.ReadLine());
            c = a + b;
            int d = c;
            Console.WriteLine("c: "+c);

            int length = 0;
            do 
            { 
                length++; c/= 10; 
            } while (c!= 0);
            if (length >= 2)
            {
                ReverseNumber(d);
                SumOfDigits(d);
            }
            Console.WriteLine("Length of a number: "+length);
        }
        static public void ReverseNumber(int a)
        {
            int rev = 0, rem;
            while (a > 0)
            {
                rem = a % 10;
                rev = rev * 10 + rem;
                a /= 10;
            }
            Console.WriteLine("Reverse Number: " + rev);
        }
        static public void SumOfDigits(int a)
        {
            int sum = 0;
            while (a > 0) {
                int rem = a % 10;
                sum = sum + rem;
                a /= 10;
            }
            Console.WriteLine("Sum of Digits: " + sum);
        }
    }
}

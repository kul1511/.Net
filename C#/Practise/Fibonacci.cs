using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practise
{
    internal class Fibonacci
    {
        public static int Fib(int n) { 
            if (n <= 0) { 
                return 0; 
            } 
            else if (n == 1) { 
                return 1; 
            } else { 
                return Fib(n - 1) + Fib(n - 2); 
            } 
        }
    }
}

using System;

namespace Collections
{
    class Program
    {

        public static void Swap<T>(T a, T b)
        {
            Console.WriteLine("a: " + a + " b: " + b);
        }

        static void Main()
        {
            int x = 1, y = 2;
            Swap<int>(x, y);

            string str1 = "string 1", str2 = "string 2";
            Swap<string>(str1, str2);

            List<int> num = new List<int> { 1, 2, 3, 4 };
            Console.WriteLine("Before Adding: ");
            foreach (int n in num)
            {
                Console.Write(n + " ");
            }
            num.Add(5);
            num.Remove(1);
            Console.WriteLine("\nAfter Adding: ");
            foreach (int n in num)
            {
                Console.Write(n + " ");
            }

        }
    }
}
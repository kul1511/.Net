using System;
namespace Program
{

    public class Program
    {
        public delegate void SampleDelegate();
        public static event SampleDelegate sd;
        public static void display()
        {
            Console.WriteLine("Displaying.......");
        }
        public static void Main(string[] args)
        {
            //Delegate Example
            // SampleDelegate sampleDelegate = new SampleDelegate(display);
            // sampleDelegate();

            //Events example
            sd += display;
            if (sd != null)
            {
                sd.Invoke();
            }

            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var even = numbers.Where(n => n % 2 == 0);
            foreach (int n in even)
            {
                Console.WriteLine("" + n);
            }
            //Lambda Expression that takes no parameters returns constant value
            Func<int> constant = () => 10;
            int res = constant();
            Console.WriteLine("" + res);

        }
    }
}
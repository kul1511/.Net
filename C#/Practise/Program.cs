// See https://aka.ms/new-console-template for more information
using System;
using System.Linq;
 
namespace Practise
{
    class HelloWorld
    {
        //Methods
         void display()
        {
            Console.WriteLine("Method Before Class");
        }
         static void Main(string[] args)
        {
            //int a;
            ////Console.Write("Enter a: ");
            ////a = Convert.ToInt32(Console.ReadLine());
            ////a = Convert.ToInt32(Math.Sqrt(a));
            ////Console.Write("A: " + a);
            //string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };
            //for (int i = 0; i < cars.Length; i++)
            //{
            //    Console.Write(cars[i] + " ");
            //}
            ////Console.WriteLine("Largest: "+cars.Max());
            ////Console.WriteLine("Largest: "+cars.Min());

            ////Declaring 2D Array
            //int[,] matrix = { { 1, 2, 3 },{ 4, 5, 6 } };
            //HelloWorld hw = new HelloWorld();
            //hw.display();
            //display2();
            //PrimeNumber.primeNumber();
            //AddTwoNumbers.AddToNumber();

            //Using Recursion
            //int FibSum = 0;
            //for (int i = 0; i < 5; i++)
            //    FibSum += Fibonacci.Fib(i);
            //Console.WriteLine(FibSum);

            ////Using Without Recursion
            //int first = 0, second = 1, third =0;
            //for(int i = 0; i < 5; i++)
            //{
            //    third = first+second;
            //    Console.Write(third+" ");
            //    first = second;
            //    second = third;
            //}


            string carName;
            int price;
            Console.WriteLine("Enter Car Name: ");
            carName = Console.ReadLine();
            Console.WriteLine("Enter Car Price: ");
            price = Convert.ToInt32(Console.ReadLine());

            string name;
            int vehiclePrice;
            Console.WriteLine("Enter Vehicle Name: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter Vehicle Price: ");
            vehiclePrice = Convert.ToInt32(Console.ReadLine());

            //Encapsulation
            Vehicle bike = new Vehicle();
            bike.DisplayDetails(carName,price);
            bike.DisplayCarDetails(name, vehiclePrice);
        }
        static void display2()
        {
            Console.WriteLine("Method After Class");
        }
    }
}

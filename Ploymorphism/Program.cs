using System;

namespace Ploymorphism
{
    class Program
    {

        public static void Main(string[] args)
        {
            Shapes sp = new Shapes();
            int opt = 0;
            do
            {


                Console.WriteLine("1. Calculate area of rectangle\n2. Calculate area of square\n3. Calculate area of triangle\n4. Exit\nEnter your choice");
                int choice = int.Parse(Console.ReadLine());
                opt = choice;
                if (choice == 1)
                {
                    Console.WriteLine("Enter the length");
                    int length = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the breadth");
                    int breadth = int.Parse(Console.ReadLine());
                    int area = sp.CalculateArea(length, breadth);
                    Console.WriteLine("Area of Rectangle: " + area);
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Enter the length");
                    double length = int.Parse(Console.ReadLine());
                    double area = sp.CalculateArea(length);
                    Console.WriteLine("Area of Square: " + area);
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Enter the breadth");
                    float breadth = float.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the height");
                    float height = float.Parse(Console.ReadLine());
                    float area = sp.CalculateArea(breadth, height);
                    Console.WriteLine("Area of Triangle: " + area);

                }
            } while (opt != 4);
        }

    }
}
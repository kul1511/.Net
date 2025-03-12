using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeClass
{

    public class Singleton
    {
        private static readonly Singleton sing = new Singleton();

        private Singleton()
        {
            Console.WriteLine("Singleton instance created.");
        }

        public static Singleton Instance
        {
            get
            {
                return sing;
            }
        }

        public void display()
        {
            Console.WriteLine("Singleton instance method called.");
        }
    }


    public class ConstructosTypes
    {
        public static int a;
        public int b;
        static ConstructosTypes()
        {
            a = 10;
        }

        public void display()
        {
            Console.WriteLine(a);
            Console.WriteLine(b);
        }
    }

    class A
    {
        public static void Main(string[] args)
        {
            ConstructosTypes ct = new ConstructosTypes();
            ct.display();
            Singleton.Instance.display();
        }
    }
}
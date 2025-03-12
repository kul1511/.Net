using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practise
{
    public class Car
    {
        public string baseClassCarName;
        public int baseClassCarPrice;
        
        //public Car(string name, int price) {
        //}

        public void DisplayDetails(string name, int price)
        {
            baseClassCarName = name;
            baseClassCarPrice = price;
        }
    }
    public class Vehicle : Car
    {   
        public string VehicleName;
        public int VehiclePrice;
        public void DisplayCarDetails(string name,int price)
        {
            VehicleName = name;
            VehiclePrice = price;

            Console.WriteLine("Bike");
            Console.WriteLine("Name:"+VehicleName);
            Console.WriteLine("Price:"+VehiclePrice);
            
            Console.WriteLine("Car");
            Console.WriteLine("Name:"+base.baseClassCarName);
            Console.WriteLine("Price:"+base.baseClassCarPrice);
        }
    }
}

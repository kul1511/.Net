using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practise
{
    public abstract class AbstractClass
    {
        public abstract void abstractMethod();
    }
    public class DerivedAbstract:AbstractClass
    {
        public override void abstractMethod()
        {
            Console.WriteLine("Abstraction Demonstration");
        }
    }
}

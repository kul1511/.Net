using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileDetails
{
    public class MobileUtility
    {
        public void AddMobileDetails(string model, string brand, int price)
        {
            Program.mobileDetails.Add(Program.mobileDetails.Count + 1, new Modile { Model = model, Brand = brand, Price = price });
        }

        public SortedDictionary<string, List<Mobile>> GroupMobilesByBrand()
        {
            SortedDictionary<string, List<Mobile>> groupMobiles = new SortedDictionary<string, List<Mobile>>();
            foreach (var m in Program.mobileDetails.Values)
            {
                if (!groupMobilesgroupMobiles.ContainsKey(m))
                {

                }
            }
        }
    }
}
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // FileStream f = new FileStream("sample.txt", FileMode.Create);
            // Console.WriteLine("Enter Data");
            // string data = Console.ReadLine();
            // File.WriteAllText("sample.txt", data);
            // Console.WriteLine("File Data: ");
            // string fileData = File.ReadAllText("sample.txt");
            // Console.WriteLine(fileData);
            string str = "pinky is my pet name";

            string pattern = @"^pinky";

            bool matches = Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase);
            if (matches)
            {
                Console.WriteLine("true");
            }
            else Console.WriteLine("false");
        }
    }
}
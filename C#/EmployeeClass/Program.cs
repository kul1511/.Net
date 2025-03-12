using EmployeeClass;
using System;
public class Program
{
    public static void CountChars(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            int count = 1;
            for (int j = i + 1; j < str.Length; j++)
            {
                if (str[j] == str[i])
                {
                    count++;
                    i++;
                }
            }
            Console.WriteLine(str[i] + ":" + count);
        }
    }

    // public static void Main(string[] args)
    // {

    // Console.WriteLine("Enter the ID of Product");
    // int id = Convert.ToInt32(Console.ReadLine());

    // Console.WriteLine("Enter the Product Name");
    // string name = Console.ReadLine();

    // Console.WriteLine("Enter the Product Type");
    // string department = Console.ReadLine();


    // Product pd = new Product(id, name, department);

    // pd.DisplayDetails();

    //         string str;
    //     Console.WriteLine("Enter the string");
    //         str = Console.ReadLine();
    //         CountChars(str);
    //     string[] words = str.Split(':');
    //     int stringLength = words.Length;

    //     bool flag = false;

    //         if (stringLength <= 15)
    //         {
    //             for (int i = 0; i<stringLength; i++)
    //             {
    //                 words[i] = words[i].ToUpper();
    // }

    // foreach (string word in words)
    // {
    //     int count = 0;
    //     for (int i = 0; i < stringLength; i++)
    //     {
    //         if (words[i] == word)
    //         {
    //             count++;
    //             words[i] = null;
    //         }
    //     }
    //     Console.Write(word);
    //     if (word != null)
    //     {
    //         Console.Write(":" + count);
    //         Console.WriteLine();
    //     }
    // }
    //         }
    //         else
    // {
    //     Console.WriteLine("Invalid length");
    // }

    // }
}
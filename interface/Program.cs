using System;
namespace Program
{
    public interface IAccount
    {
        string AccountNumber { get; set; }
        double Balance { get; set; }
        double InterestRate { get; set; }
        double CalculateInterest();
    }

    public class SavingsAccount : IAccount
    {
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public double InterestRate { get; set; }
        public double CalculateInterest()
        {
            double interest = Balance * InterestRate / 100;
            if (Balance < 1000)
            {
                interest -= Balance * 0.1 / 100;
            }
            return interest;
        }
    }
    public class CheckingAccount : IAccount
    {
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public double InterestRate { get; set; }
        public double CalculateInterest()
        {
            double interest = Balance * InterestRate / 100;
            if (Balance < 1000)
            {
                interest -= Balance * 0.5 / 100;
            }
            return interest;
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {


            System.Console.WriteLine("1. Savings Account\n2. Checking Account");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                SavingsAccount sa = new SavingsAccount();
                Console.WriteLine("Enter the account number");
                sa.AccountNumber = Console.ReadLine();
                System.Console.WriteLine("Enter the balance amount");
                sa.Balance = double.Parse(Console.ReadLine());
                Console.WriteLine("Enter the Interest Rate");
                sa.InterestRate = double.Parse(Console.ReadLine());
                System.Console.WriteLine("Interest amount is " + sa.CalculateInterest());
            }
            else if (choice == 2)
            {
                CheckingAccount sa = new CheckingAccount();
                Console.WriteLine("Enter the account number");
                sa.AccountNumber = Console.ReadLine();
                System.Console.WriteLine("Enter the balance amount");
                sa.Balance = double.Parse(Console.ReadLine());
                Console.WriteLine("Enter the Interest Rate");
                sa.InterestRate = double.Parse(Console.ReadLine());
                double interest = sa.CalculateInterest();
                System.Console.WriteLine("Interest amount is " + interest);

            }


        }
    }
}
using System;
using System.Collections.Generic;
namespace Task13thFeb
{
    public class Program
    {
        public static HashSet<Candidate> CandidatesSet { get; set; } = new HashSet<Candidate>();

        public static void Main(string[] args)
        {
            InterviewUtility iu = new InterviewUtility();
            while (true)
            {
                Console.WriteLine("\n1. Add Candidate: \n2. Update Candidate\n3. Get Qualified Candidates\n4. Exit\nEnter your choice");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    Console.WriteLine("Enter Name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter Skills:");
                    string skills = Console.ReadLine();
                    bool res = iu.Register(name, skills);
                    if (res)
                    {
                        Console.WriteLine($"Candidate {name} has been successfully registered");
                    }
                    else
                    {
                        Console.WriteLine($"Candidate {name} is already registered");
                    }
                    Console.WriteLine("\nHashSet: ");
                    foreach (Candidate c in CandidatesSet)
                    {
                        Console.WriteLine("FullName: " + c.FullName);
                        Console.WriteLine("Skills: " + c.Skills);
                        Console.WriteLine("IsQualified: " + c.IsQualified);
                    }
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Enter Name to Update");
                    string updateName = Console.ReadLine();
                    Console.WriteLine("Enter Skills to Update:");
                    string updateSkills = Console.ReadLine();
                    bool res2 = iu.UpdateCandidateSkills(updateName, updateSkills);
                    if (res2)
                    {
                        Console.WriteLine($"Skills of candidate {updateName} has been updated successfully\n");
                    }
                    else
                    {
                        Console.WriteLine($"Candidate {updateName} not found\n");
                    }
                    Console.WriteLine("\nHashSet: ");
                    foreach (Candidate c in CandidatesSet)
                    {
                        Console.WriteLine("FullName: " + c.FullName);
                        Console.WriteLine("Skills: " + c.Skills);
                        Console.WriteLine("IsQualified: " + c.IsQualified);
                    }
                    Console.WriteLine();
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Enter the qualified skill");
                    string skillsToUpdate = Console.ReadLine();
                    var ans = iu.MarkCandidateAsQualified(skillsToUpdate);
                    Console.WriteLine("Qualified Candidates: ");
                    foreach (var v in ans)
                    {
                        Console.WriteLine("" + v);
                    }
                }
                else { break; }
            }
        }
    }
}
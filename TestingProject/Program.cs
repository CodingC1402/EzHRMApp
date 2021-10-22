using DAL.Repos;
using System;

namespace TestingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine(AccountRepo.Instance.TableName);
        }
    }
}

using DAL.Repos;
using System;
using System.Collections.Generic;

namespace TestingProject
{
    class BaseClass<T>
    {
        public static List<T> Collections { get; set; } = new List<T>();
    }

    class ClassInt : BaseClass<int>
    {

    }

    class ClassString : BaseClass<string>
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            ClassInt.Collections.AddRange(new int[] { 0, 1, 2, 3, 4 });
            ClassString.Collections.AddRange(new string[] { "001", "002", "003", "004" });

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(ClassInt.Collections[i]);
                Console.WriteLine(ClassString.Collections[i]);
            }
        }
    }
}

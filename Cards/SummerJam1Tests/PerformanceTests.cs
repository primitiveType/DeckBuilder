using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SummerJam1Tests
{
    public class PerformanceTests
    {

        [Test]
        public void Exceptions()
        {
            Task.Run(() =>
            {
                throw new NotSupportedException();
            });
        }
        
        [Test]
        public void TestMemoryAllocationOfLinq()
        {
            GC.Collect();
            GC.Collect();
            GC.Collect();
            GC.Collect();
            long memoryFirst = GC.GetTotalMemory(true);
            object obj = new object();
            long memoryObj = GC.GetTotalMemory(true);
            List<int> SomeList = new List<int>();
            long memorySecond = GC.GetTotalMemory(true);
         
            List<int> copy = SomeList.ToList();
            long memoryThird = GC.GetTotalMemory(true);
        
            
            for (int i = 0; i < 100; i++)
            {
                SomeList.Add(i);
            }
            
            long memoryFourth = GC.GetTotalMemory(true);
       
            long memory = GC.GetTotalMemory(true);


            IEnumerable<int> ten = SomeList.Where(i => i == 10);
            long memoryFifth = GC.GetTotalMemory(true);

            Console.WriteLine($"memory at start : {memoryFirst}.");
            Console.WriteLine($"memory after object : {memoryObj} = {memoryObj - memoryFirst}.");
            Console.WriteLine($"memory after list : {memorySecond} = {memorySecond - memoryObj}.");
            Console.WriteLine($"memory after copy list : {memoryThird}  = {memoryThird - memorySecond}.");
            Console.WriteLine($"memory after adding 100 ints : {memoryFourth} = {memoryFourth - memoryThird}.");
            Console.WriteLine($"memory after using Linq : {memoryFifth} = {memoryFifth - memoryFourth}.");


        }
    }
}

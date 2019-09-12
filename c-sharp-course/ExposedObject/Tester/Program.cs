//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using IgorO.ExposedObjectProject;

//namespace Tester
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            List<int> realList = new List<int>();

//            dynamic exposedList = ExposedObject.From(realList);

//            // Read a private field - prints 0
//            Console.WriteLine(exposedList._size);

//            // Modify a private field
//            exposedList._items = new int[] { 5, 4, 3, 2, 1 };

//            // Modify another private field
//            exposedList._size = 5;

//            // Call a private method
//            exposedList.EnsureCapacity(20);


//            // Add a value to the list
//            exposedList.Add(0);

//            // Enumerate the list. Prints "5 4 3 2 1 0"
//            foreach (var x in exposedList) Console.WriteLine(x);

//            // Call a static method
//            dynamic fileType = ExposedClass.From(typeof(System.IO.File));
//            bool exists = fileType.InternalExists("somefile.txt");

//            // Call a generic method
//            dynamic enumerableType = ExposedClass.From(typeof(System.Linq.Enumerable));
//            Console.WriteLine(
//                enumerableType.Max(new[] { 1, 3, 5, 3, 1 }));
//        }
//    }
//}

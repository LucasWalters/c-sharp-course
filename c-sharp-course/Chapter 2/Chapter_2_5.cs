#define TEST

using IgorO.ExposedObjectProject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace c_sharp_course
{
    class Chapter_2_5
    {
        public static void Run()
        {

            TestConditional();

#if TEST
            TestIf();
#endif
            //TestAttributes();

            //TestReflection();

            //TestReflectionDateTime();

            //TestExposedObject();

            //TestFlags();
        }

        /// <summary>
        /// Test with conditionals, this code always compiles but only runs if "TEST" is defined
        /// in the top of the file.
        /// </summary>
        #region CONDITIONAL
        [Conditional("TEST")]
        static void TestConditional()
        {
            Console.WriteLine("\n===== CONDITIONAL");
            Console.WriteLine("Conditional: This will be printed only if TEST is defined.");
        }
        #endregion

        /// <summary>
        /// Test if statement definitions, this code only compiles if "TEST" is defined
        /// in the top of the file.
        /// </summary>
        #region IF
#if TEST
        static void TestIf()
        {
            Console.WriteLine("\n===== IF");
            Console.WriteLine("If: This will be printed only if TEST is defined.");
        }
#endif
        #endregion

        /// <summary>
        /// Test with custom attributes. Take a look at the attribute class definition below
        /// </summary>
        #region ATTRIBUTES
        static TestAttribute testAttribute;
        static void TestAttributes()
        {
            Console.WriteLine("\n===== ATTRIBUTES");

            //Use IsDefined to check whether a class has a specific attribute.
            Console.WriteLine("TestClass has Attribute Test: " +
                Attribute.IsDefined(typeof(TestClass), typeof(TestAttribute)));

            //Get the attribute
            Attribute a = Attribute.GetCustomAttribute(typeof(TestClass),
                typeof(TestAttribute));

            testAttribute = (TestAttribute)a;

            //Use methods in the attribute object
            Console.WriteLine("TestValue: {0}", testAttribute.TestValue);
        }
        #endregion

        /// <summary>
        /// In this example we change a private field value of 
        /// the attribute class defined below in the file using reflection.
        /// </summary>
        #region REFLECTION
        static void TestReflection()
        {
            Console.WriteLine("\n===== REFLECTION");

            //Type is almost always used when using reflection
            Type t = testAttribute.GetType();
            
            //Log original value
            Console.WriteLine("Original value: " + testAttribute.TestValue);

            //Try to change it with the setter - this doesn't work because the implementation is wrong (see class below)
            testAttribute.TestValue = "Something different";

            //Log to see it hasn't changed
            Console.WriteLine("After setter: " + testAttribute.TestValue);

            //Get the private field of the class
            FieldInfo fi = t.GetField("testValue", BindingFlags.NonPublic |
                                                   BindingFlags.Instance);

            //Change the private field directly
            fi.SetValue(testAttribute, "Something different now?");
            Console.WriteLine("After reflection: " + testAttribute.TestValue);
        }
        #endregion

        /// <summary>
        /// Here's a pretty complicated example of how to change value types data.
        /// We change the value inside a DateTime variable using the hidden method __makeref
        /// More info about __makeref: http://benbowen.blog/post/fun_with_makeref/
        /// </summary>
        #region REFLECTION_DATETIME
        static void TestReflectionDateTime()
        {
            DateTime dateTime = DateTime.Now;

            FieldInfo dateData = typeof(DateTime).GetField("_dateData",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Console.WriteLine(dateTime.ToString()); // Log date before change
            Console.WriteLine(dateData.GetValue(dateTime)); // Log dateData before change

            TypedReference typed = __makeref(dateTime);
            dateData.SetValueDirect(typed, (ulong)0); // Change dateData

            Console.WriteLine(dateTime.ToString()); // Log date after change - No difference
            Console.WriteLine(dateData.GetValue(dateTime)); // Log dateData after change - No difference
        }
        #endregion

        /// <summary>
        /// In this test we use a third party class called the ExposedObject
        /// Which can be used to make reflections a little bit easier.
        /// The example shows how to change private field values of a List object
        /// </summary>
        #region EXPOSED_OBJECT
        static void TestExposedObject()
        {
            Console.WriteLine("\n===== EXPOSED OBJECTS");

            List<int> realList = new List<int>();

            dynamic exposedList = ExposedObject.From(realList);

            // Read a private field - prints 0
            Console.WriteLine(exposedList._size);

            // Modify a private field
            exposedList._items = new int[] { 5, 4, 3, 2, 1 };

            // Modify another private field
            exposedList._size = 5;

            // Call a private method
            exposedList.EnsureCapacity(20);

            // Add a value to the list
            exposedList.Add(0);

            // Enumerate the list. Prints "5 4 3 2 1 0"
            foreach (var x in exposedList) Console.WriteLine(x);
        }
        #endregion

        /// <summary>
        /// This test shows how to use flags enums. 
        /// Be sure to look at the definition of the flag enum Color below
        /// </summary>
        #region FLAGS
        static void TestFlags()
        {
            Console.WriteLine((uint)Color.White);

            Color cyan = Color.Blue | Color.Green;
            // 010  Blue
            // 100  Green
            // ---  | bitwise operation
            // 110  Cyan

            Color red = Color.Magenta & Color.Yellow;
            // 011  Magenta
            // 101  Yellow
            // ---  & bitwise operation
            // 001  Red

            // uints are unsigned integers, which means 
            // no bits are used to define whether it's 
            // a positive or a negative number (the sign).
            // Unsigned variables can therefor only have 
            // a positive value.
        }
        #endregion

    }

    #region ATTRIBUTES_CLASSES
    [Test("Hello")]
    class TestClass { }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    class TestAttribute : Attribute
    {
        private string testValue;

        public TestAttribute(string test)
        {
            testValue = test;
        }

        public string TestValue {
            get { return testValue; }
            set { Console.WriteLine("Go away with your " + value); }
        }
    }
    #endregion

    #region FLAGS_ENUM
    [Flags]
    enum Color : uint {             // bits     decimal value
        Black = 0,                  // 000      0
        White = ~(uint)0,           // 111      MaxValue
        Red = (1 << 0),             // 001      1 
        Blue = (1 << 1),            // 010      2
        Green = (1 << 2),           // 100      4
        Magenta = (Red | Blue),     // 011      3     (bitwise or: 001 | 010)
        Yellow = (Red | Green),     // 101      5     (bitwise or: 001 | 100)
        Cyan = (Blue | Green),      // 110      6     (bitwise or: 010 | 100)
    }
    #endregion
}

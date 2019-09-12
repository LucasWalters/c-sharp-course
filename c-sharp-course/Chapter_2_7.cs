using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace c_sharp_course
{
    class Chapter_2_7
    {
        public static void Run()
        {
            TestStringImmutability();

            //TestStringBuilder();
        }

        /// <summary>
        /// This test shows which definitions will use the same string objects
        /// and how this can be forced by calling string.Intern().
        /// ReferenceEquals is used to check if two reference type variables point to the same object.
        /// </summary>
        #region STRING_IMMUTABILITY
        static void TestStringImmutability()
        {
            Console.WriteLine("\n===== STRING IMMUTABILITY");
            string s1 = "hello";
            string s2 = "hello";
            Console.WriteLine("s1 == s2: " + ReferenceEquals(s1, s2)); //true

            string s3 = "he" + "llo";
            Console.WriteLine("s1 == s3: " + ReferenceEquals(s1, s3)); //true

            string s4 = new string(new char[] { 'h', 'e', 'l', 'l', 'o' });
            Console.WriteLine("s1 == s4: " + ReferenceEquals(s1, s4)); //false

            string s5 = new string("hello");
            Console.WriteLine("s1 == s5: " + ReferenceEquals(s1, s5)); //false

            string he = "he";
            string llo = "llo";
            string s6 = he + llo;
            Console.WriteLine("s1 == s6: " + ReferenceEquals(s1, s6)); //false

            string s7 = string.Intern(s6);
            Console.WriteLine("s1 == s7: " + ReferenceEquals(s1, s7)); //true
        }
        #endregion

        /// <summary>
        /// This test shows an alternative to the resource heavy way of concatting lots of string:
        /// the stringbuilder class.
        /// </summary>
        #region STRING_BUILDER
        static void TestStringBuilder()
        {
            string s = "Hello: ";
            for (int i = 0; i < 10; i++)
            {
                s += i;
               
            }

            Console.WriteLine(s);


            StringBuilder sb = new StringBuilder("Hello: ");

            for (int i = 0; i < 10; i++)
            {
                sb.Append(i);
            }

            Console.WriteLine(sb.ToString());
        }
        #endregion
    }


}

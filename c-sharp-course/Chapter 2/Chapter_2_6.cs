using System;
using System.Collections.Generic;
using System.Threading;

namespace c_sharp_course
{
    class Chapter_2_6
    {
        public static void Run()
        {
            TestMemory();

            //TestUnmanagedResources();

        }

        /// <summary>
        /// Simple memory usage test, look at the graph while running.
        /// </summary>
        #region MEMORY
        static void TestMemory()
        {
            List<string> strings;
            for (;;)
            {
                strings = new List<string>();
                for (int i = 0; i < 10000; i++)
                {
                    strings.Add(i.ToString());
                }
                Thread.Sleep(10);
            }
        }
        #endregion

        /// <summary>
        /// Shows how to use different ways of handling unmanaged resources.
        /// The preferred way is using the Disposable pattern, look at the classes below.
        /// </summary>
        #region UNMANAGED_RESOURCES
        static void TestUnmanagedResources()
        {
            ClassWithFinalizer c = new ClassWithFinalizer();
            c = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Object will finalize.");

            using (DisposableClass dc = new DisposableClass())
            {
                
            }

            Console.WriteLine("Object will be disposed.");
        }
        #endregion

    }

    #region UNMANAGED_RESOURCES_CLASSES
    public class ClassWithFinalizer
    {
        ~ClassWithFinalizer()
        {
            Console.WriteLine("Finalizer called");
        }
    }

    public class DisposableClass : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Dispose called");
        }
    }


    public class DisposablePatternClass : IDisposable
    {
        // Flag to indicate when the object has been
        // disposed
        bool disposed = false;

        List<string> x;
        public void Dispose()
        {
            // Call dispose and tell it that
            // it is being called from a Dispose call
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            // Give up if already disposed
            if (disposed)
                return;
            if (disposing)
            {
                // free any managed objects here
            }
            // Free any unmanaged objects here
        }
        ~DisposablePatternClass()
        {
            // Dispose only of unmanaged objects
            Dispose(false);
        }
    }

    #endregion

}

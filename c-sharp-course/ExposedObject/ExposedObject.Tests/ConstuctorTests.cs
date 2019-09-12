using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ExposedObject.Tests
{
    [TestFixture]
    public class ConstuctorTests
    {
        private readonly Lazy<Assembly> m_TestAssembly = new Lazy<Assembly>(() => Assembly.Load(File.ReadAllBytes("TestLibrary.dll")));
            
        [Test]
        public void TestThatTypesWithInternalConstuctorWork()
        {
            var constructedType = IgorO.ExposedObjectProject.ExposedObject.New(LoadType("InternalClassWithInternalArgumentlessConstructor"));

            Assert.That(constructedType, Is.Not.Null, "Should have been able to construct the time");
        }

        private Type LoadType(string typeName)
        {
            return m_TestAssembly.Value.GetTypes().Single(type => String.Equals(type.Name, typeName));
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Coercion;

namespace Configgy.Tests.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InstanceOfTypeCoercerAttributeTests
    {
        [TestMethod]
        public void Coerce_Returns_Instance_Of_Correct_Type_For_Known_Types()
        {
            const int expected = default;
            const string value = "System.Int32";

            var coercer = new InstanceOfTypeCoercerAttribute();

            var coerced = coercer.Coerce(null, value, out int result);

            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(expected, result);
            Assert.IsTrue(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_Null_For_Unknown_Types()
        {
            const int expected = default;
            const string value = "System.Int365";

            var coercer = new InstanceOfTypeCoercerAttribute();

            var coerced = coercer.Coerce(null, value, out int result);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(coerced);
        }

        [TestMethod]
        public void Coerce_Returns_Null_For_Invlaid_Type_Name()
        {
            const int expected = default;
            const string value = "000 Banana Kerfuffle Ogre !@#$%^&*()_+";

            var coercer = new InstanceOfTypeCoercerAttribute();

            var coerced = coercer.Coerce(null, value, out int result);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(coerced);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void Coerce_Returns_Null_For_Class_With_No_Default_Constructor()
        {
            var value = typeof(ClassWithNoDefaultConstructor).AssemblyQualifiedName;

            var coercer = new InstanceOfTypeCoercerAttribute();

            coercer.Coerce(null, value, out ClassWithNoDefaultConstructor _);
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void Coerce_Throws_Exception_When_Constructor_Throws_Exception()
        {
            var value = typeof(ClassWithBrokenConstructor).AssemblyQualifiedName;

            var coercer = new InstanceOfTypeCoercerAttribute();

            coercer.Coerce(null, value, out ClassWithBrokenConstructor _);
        }
    }
}
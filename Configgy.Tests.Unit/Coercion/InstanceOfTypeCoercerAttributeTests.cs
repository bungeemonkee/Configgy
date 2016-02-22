using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    public class InstanceOfTypeCoercerAttributeTests
    {
        [TestMethod]
        public void CoerceTo_Returns_Instance_Of_Correct_Type_For_Known_Types()
        {
            var expected = default(int);
            var value = "System.Int32";

            var coercer = new InstanceOfTypeCoercerAttribute();

            var result = coercer.CoerceTo<Type>(value, null, null);

            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_For_Unknown_Types()
        {
            var value = "System.Int365";

            var coercer = new TypeCoercer();

            var result = coercer.CoerceTo<Type>(value, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_For_Invlaid_Type_Name()
        {
            var value = "000 Banana Kerfuffle Ogre !@#$%^&*()_+";

            var coercer = new TypeCoercer();

            var result = coercer.CoerceTo<Type>(value, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_For_Class_With_No_Default_Constructor()
        {
            var value = nameof(ClassWithNoDefaultConstructor);

            var coercer = new TypeCoercer();

            var result = coercer.CoerceTo<Type>(value, null, null);

            Assert.IsNull(result);
        }
    }
}

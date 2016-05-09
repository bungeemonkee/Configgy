using Configgy.Coercion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TypeCoercerTests
    {
        [TestMethod]
        public void CoerceTo_Returns_Correct_Type_Instance_For_Known_Types()
        {
            var expected = typeof(int);
            var value = "System.Int32";

            var coercer = new TypeCoercerAttribute();

            var result = coercer.CoerceTo<Type>(value, null, null);

            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_For_Unknown_Types()
        {
            var value = "System.Int365";

            var coercer = new TypeCoercerAttribute();

            var result = coercer.CoerceTo<Type>(value, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_For_Invlaid_Type_Name()
        {
            var value = "000 Banana Kerfuffle Ogre !@#$%^&*()_+";

            var coercer = new TypeCoercerAttribute();

            var result = coercer.CoerceTo<Type>(value, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CoerceTo_Returns_Null_For_Types_That_Arent_Type()
        {
            var value = "System.Int32";

            var coercer = new TypeCoercerAttribute();

            var result = coercer.CoerceTo<int>(value, null, null);

            Assert.IsNull(result);
        }
    }
}

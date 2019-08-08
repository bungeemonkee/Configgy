using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Configgy.Coercion;

namespace Configgy.Tests.Coercion
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class Base64EncodedCoercerAttributeTests
    {
        [TestMethod]
        public void Base64_Round_Trip_Works()
        {
            const int byteCount = 10000;

            var rand = new Random();

            var dataIn = new byte[byteCount];

            rand.NextBytes(dataIn);

            var base64 = Convert.ToBase64String(dataIn, Base64FormattingOptions.InsertLineBreaks);

            var coercer = new Base64EncodedCoercerAttribute();

            var success = coercer.Coerce<byte[]>(null, base64, out var dataOut);

            Assert.IsTrue(success);
            CollectionAssert.AreEquivalent(dataIn, dataOut);
        }
        
        [TestMethod]
        public void Coerce_To_Anything_But_Byte_Array_Returns_False()
        {
            const int byteCount = 10000;

            var rand = new Random();

            var dataIn = new byte[byteCount];

            rand.NextBytes(dataIn);

            var base64 = Convert.ToBase64String(dataIn, Base64FormattingOptions.InsertLineBreaks);

            var coercer = new Base64EncodedCoercerAttribute();

            var result = coercer.Coerce<DateTime>(null, base64, out _);

            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void Coerce_Null_Returns_Null()
        {
            var coercer = new Base64EncodedCoercerAttribute();

            var result = coercer.Coerce<byte[]>(null, null, out var dataOut);

            Assert.IsTrue(result);
            Assert.IsNull(dataOut);
        }
    }
}
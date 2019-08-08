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
    }
}
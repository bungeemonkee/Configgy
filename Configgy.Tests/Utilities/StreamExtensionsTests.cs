using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Configgy.Utilities;

namespace Configgy.Tests.Unit.Utilities
{
    [TestClass]
    //[ExcludeFromCodeCoverage]
    public class StreamExtensionsTests
    {
        [TestMethod]
        public void WriteInt_Writes_Int_Bytes_0()
        {
            WriteInt_Writes_Int_Bytes(0);
        }

        [TestMethod]
        public void WriteInt_Writes_Int_Bytes_10()
        {
            WriteInt_Writes_Int_Bytes(10);
        }

        [TestMethod]
        public void WriteInt_Writes_Int_Bytes_34568()
        {
            WriteInt_Writes_Int_Bytes(34568);
        }

        [Ignore]
        private void WriteInt_Writes_Int_Bytes(int value)
        {
            var expected = BitConverter.GetBytes(value);

            var streamMock = new Mock<Stream>();
            streamMock.Setup(x => x.Write(It.Is<byte[]>(y => AreEqual(y, expected)), 0, expected.Length));

            StreamExtensions.WriteInt(streamMock.Object, value);

            streamMock.VerifyAll();
        }

        [Ignore]
        private static bool AreEqual(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
        {
            if (x == null || y == null) return false;
            if (x.Count != y.Count) return false;

            for (var i = 0; i < x.Count; ++i)
            {
                if (x[i] != y[i]) return false;
            }

            return true;
        }
    }
}

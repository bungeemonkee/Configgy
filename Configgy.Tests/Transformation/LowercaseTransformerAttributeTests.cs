using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Transformation;
using Moq;

namespace Configgy.Tests.Transformation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LowercaseTransformerAttributeTests
    {
        [TestMethod]
        public void Transform_Returns_Null_For_Null_Values()
        {
            const string? value = null;
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            var transformer = new LowercaseTransformerAttribute();

            var result = transformer.Transform(property, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase()
        {
            const string value = "BLAH BLAH";
            const string expected = "blah blah";
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            var transformer = new LowercaseTransformerAttribute();

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transform_Returns_Lowercase_With_Explicit_Culture()
        {
            const string value = "BLAH BLAH";
            const string expected = "blah blah";
            var culture = CultureInfo.InvariantCulture;
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            var transformer = new LowercaseTransformerAttribute
            {
                Culture = culture
            };

            var result = transformer.Transform(property, value);

            Assert.AreEqual(expected, result);
        }
    }
}
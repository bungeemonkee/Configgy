using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;
using Moq;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public abstract class NumericishValidatorTests<TValidator, TNumericish>
        where TValidator : INumericishValidator<TNumericish>
    {
        protected abstract TNumericish TypeMin { get; }
        protected abstract TNumericish TypeMax { get; }
        protected abstract TNumericish TestingMin { get; }
        protected abstract TNumericish TestingMax { get; }
        protected abstract TNumericish[] ValidValues { get; }
        protected abstract string BelowTypeMin { get; }
        protected abstract string AboveTypeMax { get; }
        protected abstract string BelowTestingMin { get; }
        protected abstract string AboveTestingMax { get; }
        protected abstract string WithinTestingRange { get; }
        protected abstract string AValidValue { get; }
        protected abstract string AnInvalidValue { get; }
        protected abstract string UnParseable { get; }

        protected virtual string TypeMaxString => TypeMax!.ToString()!;

        protected virtual string TypeMinString => TypeMin!.ToString()!;

        protected abstract INumericishValidator<TNumericish> MakeValidator(TNumericish min, TNumericish max,
            TNumericish[]? validValues);

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TypeMax()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, TypeMaxString, out TNumericish value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TypeMin()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, TypeMinString, out TNumericish value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TestingMax()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, TestingMax!.ToString(), out TNumericish value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TestingMin()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, TestingMin!.ToString(), out TNumericish value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_WithinTestingRange()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, WithinTestingRange, out TNumericish value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_AValidValue()
        {
            var validator = MakeValidator(TypeMin, TypeMax, ValidValues);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, AValidValue, out TNumericish value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AboveTypeMax()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, AboveTypeMax, out TNumericish value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_BelowTypeMin()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, BelowTypeMin, out TNumericish value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AboveTestingMax()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, AboveTestingMax, out TNumericish value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_BelowTestingMin()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, BelowTestingMin, out TNumericish value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AnInvalidValue()
        {
            var validator = MakeValidator(TestingMin, TestingMax, ValidValues);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, AnInvalidValue, out TNumericish value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_UnParseable()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            validator.Validate(property, UnParseable, out TNumericish value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Returns_Value_For_Valid_Values()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);
            
            IConfigProperty property = new ConfigProperty("value", TestUtilities.NullableProperty, null);

            var result = validator.Validate(property, WithinTestingRange, out TNumericish value);

            Assert.IsNotNull(value);
            Assert.IsTrue(result);
        }
    }
}

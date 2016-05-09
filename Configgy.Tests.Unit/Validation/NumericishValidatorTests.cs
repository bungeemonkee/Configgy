using Configgy.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Configgy.Tests.Unit.Validation
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

        protected virtual string TypeMaxString
        {
            get
            {
                return TypeMax.ToString();
            }
        }

        protected virtual string TypeMinString
        {
            get
            {
                return TypeMin.ToString();
            }
        }

        protected abstract INumericishValidator<TNumericish> MakeValidator(TNumericish min, TNumericish max, TNumericish[] validValues);

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TypeMax()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            validator.Validate<TNumericish>(TypeMaxString, null, null);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TypeMin()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            validator.Validate<TNumericish>(TypeMinString, null, null);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TestingMax()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            validator.Validate<TNumericish>(TestingMax.ToString(), null, null);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TestingMin()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            validator.Validate<TNumericish>(TestingMin.ToString(), null, null);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_WithinTestingRange()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            validator.Validate<TNumericish>(WithinTestingRange, null, null);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_AValidValue()
        {
            var validator = MakeValidator(TypeMin, TypeMax, ValidValues);

            validator.Validate<TNumericish>(AValidValue, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AboveTypeMax()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            validator.Validate<TNumericish>(AboveTypeMax, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_BelowTypeMin()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            validator.Validate<TNumericish>(BelowTypeMin, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AboveTestingMax()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            validator.Validate<TNumericish>(AboveTestingMax, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_BelowTestingMin()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            validator.Validate<TNumericish>(BelowTestingMin, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AnInvalidValue()
        {
            var validator = MakeValidator(TestingMin, TestingMax, ValidValues);

            validator.Validate<TNumericish>(AnInvalidValue, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_UnParseable()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            validator.Validate<TNumericish>(UnParseable, null, null);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Returns_Value_For_Valid_Values()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            var result = validator.Validate<TNumericish>(WithinTestingRange, null, null);

            Assert.IsNotNull(result);
        }
    }
}

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

            TNumericish value;
            validator.Validate(TypeMaxString, null, null, out value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TypeMin()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            TNumericish value;
            validator.Validate(TypeMinString, null, null, out value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TestingMax()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            TNumericish value;
            validator.Validate(TestingMax.ToString(), null, null, out value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_TestingMin()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            TNumericish value;
            validator.Validate(TestingMin.ToString(), null, null, out value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_WithinTestingRange()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            TNumericish value;
            validator.Validate(WithinTestingRange, null, null, out value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Allows_AValidValue()
        {
            var validator = MakeValidator(TypeMin, TypeMax, ValidValues);

            TNumericish value;
            validator.Validate(AValidValue, null, null, out value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AboveTypeMax()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            TNumericish value;
            validator.Validate(AboveTypeMax, null, null, out value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_BelowTypeMin()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            TNumericish value;
            validator.Validate(BelowTypeMin, null, null, out value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AboveTestingMax()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            TNumericish value;
            validator.Validate(AboveTestingMax, null, null, out value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_BelowTestingMin()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            TNumericish value;
            validator.Validate(BelowTestingMin, null, null, out value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_AnInvalidValue()
        {
            var validator = MakeValidator(TestingMin, TestingMax, ValidValues);

            TNumericish value;
            validator.Validate(AnInvalidValue, null, null, out value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void NumericishValidator_Validate_Throws_Exception_For_UnParseable()
        {
            var validator = MakeValidator(TestingMin, TestingMax, null);

            TNumericish value;
            validator.Validate(UnParseable, null, null, out value);
        }

        [TestMethod]
        public void NumericishValidator_Validate_Returns_Value_For_Valid_Values()
        {
            var validator = MakeValidator(TypeMin, TypeMax, null);

            TNumericish value;
            var result = validator.Validate(WithinTestingRange, null, null, out value);

            Assert.IsNotNull(value);
            Assert.IsTrue(result);
        }
    }
}

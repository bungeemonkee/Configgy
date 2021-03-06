﻿using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configgy.Validation;

namespace Configgy.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RegexValidatorTests
    {
        [TestMethod]
        public void Validate_Allows_Valid_Value()
        {
            IConfigProperty property = new ConfigProperty("testing", typeof(string), null, null);
            
            var validator = new RegexValidatorAttribute("aaabbbccc");

            validator.Validate(property, "aaabbbccc", out string value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void Validate_Throws_Exception_For_Invalid_Value()
        {
            IConfigProperty property = new ConfigProperty("testing", typeof(string), null, null);
            
            var validator = new RegexValidatorAttribute("aaabbbccc");

            validator.Validate(property, "the number twelve", out string value);
        }
    }
}
using Configgy.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.Tests.Unit.Source
{
    [TestClass]
    public class AppSettingSourceTests
    {
        [TestMethod]
        public void GetRawValue_Returns_String_From_AppSettings()
        {
            var name = "name";
            var value = "value";

            var source = new AppSettingSource();

            var result = source.GetRawValue(name, null);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GetRawValue_Returns_Null_When_No_Matching_AppSettings_Value()
        {
            var name = "kfnnpa";

            var source = new AppSettingSource();

            var result = source.GetRawValue(name, null);

            Assert.IsNull(result);
        }
    }
}

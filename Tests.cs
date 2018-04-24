using System;
using NUnit.Framework;

namespace DynamicSettingsTest
{
    public class Tests
    {
        dynamic settings;
        [SetUp]
        public void Setup()
        {
            settings = new Settings();
        }

        [Test]
        public void CanGetValueFromSettings()
        {
            Assert.That(settings.Setting2, Is.EqualTo("value 2"));
        }

        [Test]
        public void CanOverrideValue()
        {
            settings = new OverridableSettings();
            settings.Setting1 = "foo";
            Assert.That(settings.Setting1, Is.EqualTo("foo"));
        }

        [Test]
        public void CanGetSettingAsBool()
        {
            Assert.That(settings.BoolSettingBool, Is.EqualTo(true));
        }

        [Test]
        public void CanGetSettingAsInt()
        {
            Assert.That(settings.IntSettingInt, Is.EqualTo(42));
        }

        [Test]
        public void GettingSettingThatDoesNotExistThrowsException()
        {
            Assert.Throws<SettingMissingException>(() => { var val = settings.DoesNotExist; });
        }

        [Test]
        public void CanHaveNonDynamicPropertiesOnChildClass()
        {
            settings = new SettingsWithProperties();
            Assert.That(settings.ConstProperty, Is.EqualTo("const value"));
        }

    }
}
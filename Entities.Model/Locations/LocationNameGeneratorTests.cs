using System.Collections.Generic;
using System.IO;
using Akka;
using Entities.obsoletedStuff;
using NUnit.Framework;
using System;

namespace Entities.Model.Locations
{
    [TestFixture]
    public class LocationNameGeneratorTests
    {
        public string worldPrefixStringFileName = AppDomain.CurrentDomain.BaseDirectory + WorldPrefixTestActor.WorldPrefixStringsFilename;

        [TestCase]
        public void PrefixFileGeneratorCreate()
        {
            if (File.Exists(worldPrefixStringFileName))
            {
                File.Delete(worldPrefixStringFileName);
            }

            var noPrefixes = 1000;
            WorldPrefixTestActor.GenerateNamePrefixFile(noPrefixes, worldPrefixStringFileName);
            var strings = WorldPrefixTestActor.LoadPrefixesFromFile(worldPrefixStringFileName);

            Assert.AreEqual(noPrefixes, strings.Count);
        }

        [TestCase]
        public void PrefixFileGeneratorCreateUnique()
        {
            if (File.Exists(worldPrefixStringFileName))
            {
                File.Delete(worldPrefixStringFileName);
            }

            var noPrefixes = 10000;
            WorldPrefixTestActor.GenerateNamePrefixFile(noPrefixes, worldPrefixStringFileName);
            var strings = WorldPrefixTestActor.LoadPrefixesFromFile(worldPrefixStringFileName);

            var uniqueStrings = new HashSet<string>(strings);

            Assert.AreEqual(noPrefixes, uniqueStrings.Count);
        }
    }
}

using System.Collections.Generic;
using System.IO;
using Akka;
using NUnit.Framework;


namespace Entities.Model.Locations
{
    [TestFixture]
    public class LocationNameGeneratorTests
    {
        [TestCase]
        public void PrefixFileGeneratorCreate()
        {
            if (File.Exists(World.WorldPrefixStringsFilename))
            {
                File.Delete(World.WorldPrefixStringsFilename);
            }

            var noPrefixes = 1000;
            World.GenerateNamePrefixFile(noPrefixes, World.WorldPrefixStringsFilename);
            var strings = World.LoadPrefixesFromFile(World.WorldPrefixStringsFilename);

            Assert.AreEqual(noPrefixes, strings.Count);
        }

        [TestCase]
        public void PrefixFileGeneratorCreateUnique()
        {
            if (File.Exists(World.WorldPrefixStringsFilename))
            {
                File.Delete(World.WorldPrefixStringsFilename);
            }

            var noPrefixes = 10000;
            World.GenerateNamePrefixFile(noPrefixes, World.WorldPrefixStringsFilename);
            var strings = World.LoadPrefixesFromFile(World.WorldPrefixStringsFilename);

            var uniqueStrings = new HashSet<string>(strings);

            Assert.AreEqual(noPrefixes, uniqueStrings.Count);
        }
    }
}

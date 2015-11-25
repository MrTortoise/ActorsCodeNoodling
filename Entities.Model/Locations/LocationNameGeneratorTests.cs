using System.Collections.Generic;
using System.IO;
using Akka;
using Entities.obsoletedStuff;
using NUnit.Framework;


namespace Entities.Model.Locations
{
    [TestFixture]
    public class LocationNameGeneratorTests
    {
        [TestCase]
        public void PrefixFileGeneratorCreate()
        {
            if (File.Exists(WorldPrefixTestActor.WorldPrefixStringsFilename))
            {
                File.Delete(WorldPrefixTestActor.WorldPrefixStringsFilename);
            }

            var noPrefixes = 1000;
            WorldPrefixTestActor.GenerateNamePrefixFile(noPrefixes, WorldPrefixTestActor.WorldPrefixStringsFilename);
            var strings = WorldPrefixTestActor.LoadPrefixesFromFile(WorldPrefixTestActor.WorldPrefixStringsFilename);

            Assert.AreEqual(noPrefixes, strings.Count);
        }

        [TestCase]
        public void PrefixFileGeneratorCreateUnique()
        {
            if (File.Exists(WorldPrefixTestActor.WorldPrefixStringsFilename))
            {
                File.Delete(WorldPrefixTestActor.WorldPrefixStringsFilename);
            }

            var noPrefixes = 10000;
            WorldPrefixTestActor.GenerateNamePrefixFile(noPrefixes, WorldPrefixTestActor.WorldPrefixStringsFilename);
            var strings = WorldPrefixTestActor.LoadPrefixesFromFile(WorldPrefixTestActor.WorldPrefixStringsFilename);

            var uniqueStrings = new HashSet<string>(strings);

            Assert.AreEqual(noPrefixes, uniqueStrings.Count);
        }
    }
}

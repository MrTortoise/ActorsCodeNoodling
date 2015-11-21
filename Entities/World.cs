using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka.Actor;

namespace Entities
{
    public class World : ReceiveActor
    {
        public const string WorldPrefixStringsFilename = "worldPrefixStrings.txt";
        private HashSet<string> _usedNames;

        public World()
        {
            Receive<TellGenerateNameMessage>(message =>
            {
                //  string name = GenerateName(usedNames);
                

            });

            Receive<TellLoadNamePrefixesMessage>(message =>
            {

            });
        }


        public class TellLoadNamePrefixesMessage
        {
        }

        public class TellGenerateNameMessage
        {
        }

        public static void GenerateNamePrefixFile(int noPrefixes, string fileName)
        {
            // var A = char.GetNumericValue('A');
            // var Z = char.GetNumericValue('Z');

            var random = new Random();

            var prefixes = new HashSet<string>();
            int total = 0;
            while (total < noPrefixes)
            {
                var prefix = GeneratePrefix(random);
                if (!prefixes.Contains(prefix))
                {
                    prefixes.Add(prefix);
                    total++;
                }
            }

            WriteNamePrefixesToFile(fileName, prefixes);
        }

        private static void WriteNamePrefixesToFile(string fileName, HashSet<string> prefixes)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (TextWriter writer = File.CreateText(fileName))
            {
                foreach (var prefix in prefixes)
                {
                    writer.WriteLine(prefix);
                }
            }
        }

        private static string GeneratePrefix(Random random)
        {
            var char1 = (char)random.Next('A', 'Z' + 1);
            var char2 = (char)random.Next('A', 'Z' + 1);
            var char3 = (char)random.Next('A', 'Z' + 1);

            string item = char1.ToString() + char2.ToString() + char3.ToString();
            return item;
        }

        public static List<string> LoadPrefixesFromFile(string fileName)
        {
            var text = File.ReadAllLines(fileName);
            var retVal = text.ToList();
            return retVal;
        }
    }
}
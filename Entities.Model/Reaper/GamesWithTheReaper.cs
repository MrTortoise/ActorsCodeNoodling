using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Configuration;
using Akka.TestKit.NUnit;
using NUnit.Framework;

namespace Entities.Model.Reaper
{
    [Ignore]
    [TestFixture]
    public class GamesWithTheReaper : TestKit
    {
        private static string Config = @"akka { loglevel=INFO,  loggers=[""Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog""]}";

        public GamesWithTheReaper()
            :base(Config,"reaperSystem")
        {
        }

    }
}
